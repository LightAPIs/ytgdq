# Build And Package PowerShell Script

# 函数：检测命令是否存在
function Test-CommandExists {
    param($command)
    $oldPreference = $ErrorActionPreference
    $ErrorActionPreference = 'stop'
    try {
        if (Get-Command $command) { return $true }
    }
    catch {
        return $false
    }
    finally {
        $ErrorActionPreference = $oldPreference
    }
}

# 函数：使用 7z 压缩
function Compress-With7z {
    param($sourcePath, $destinationPath)
    Write-Host "Using 7z LZMA2 for compression..." -ForegroundColor Cyan
    & 7z a -r -mx=9 "$destinationPath" "$sourcePath\*" | Out-Null
    return $LASTEXITCODE -eq 0
}

# 函数：使用 Compress-Archive 压缩
function Compress-WithBuiltin {
    param($sourcePath, $destinationPath)
    Write-Host "Using built-in Compress-Archive for compression..." -ForegroundColor Cyan
    Compress-Archive -Path "$sourcePath\*" -DestinationPath $destinationPath -CompressionLevel Optimal -Force
    return $true
}

Write-Host "********** Build And Package PowerShell. **********" -ForegroundColor Green

Write-Host "********** Build... **********" -ForegroundColor Yellow

# 自动查找 MSBuild.exe（优先使用最新 Visual Studio 版本）
$msbuild = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\VSWhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | Select-Object -First 1
if (-not $msbuild -or -not (Test-Path $msbuild)) {
    Write-Error "MSBuild.exe not found. Please ensure Visual Studio is installed."
    exit 1
}
Write-Host "Using MSBuild: $msbuild" -ForegroundColor Cyan

Write-Host "********** Build Release_x86 **********" -ForegroundColor Yellow
& $msbuild "ytgdq.sln" /p:Configuration=Release /p:Platform=x86 /t:Rebuild /m
if ($LASTEXITCODE -ne 0) { Write-Error "x86 build failed"; exit $LASTEXITCODE }

Write-Host "********** Build Release_x64 **********" -ForegroundColor Yellow
& $msbuild "ytgdq.sln" /p:Configuration=Release /p:Platform=x64 /t:Rebuild /m
if ($LASTEXITCODE -ne 0) { Write-Error "x64 build failed"; exit $LASTEXITCODE }

Write-Host "********** Build Done. **********" -ForegroundColor Green
Write-Host "********** Package... **********" -ForegroundColor Yellow

# 确保 Release 目录存在
$releaseDir = "./Release"
if (-not (Test-Path $releaseDir)) { New-Item -ItemType Directory -Path $releaseDir -Force }

Write-Host "********** Package Release_x86 **********" -ForegroundColor Yellow
$zip86 = Join-Path $releaseDir "ytgdq_x86.zip"
$x86Source = Join-Path $releaseDir "x86"
if (Test-Path $zip86) { Remove-Item $zip86 -Force }
if (Test-CommandExists "7z") {
    Compress-With7z "$x86Source" "$zip86"
} else {
    Compress-WithBuiltin "$x86Source" "$zip86"
}
Write-Host "Created: $zip86" -ForegroundColor Green

Write-Host "********** Package Release_x64 **********" -ForegroundColor Yellow
$zip64 = Join-Path $releaseDir "ytgdq_x64.zip"
$x64Source = Join-Path $releaseDir "x64"
if (Test-Path $zip64) { Remove-Item $zip64 -Force }
if (Test-CommandExists "7z") {
    Compress-With7z "$x64Source" "$zip64"
} else {
    Compress-WithBuiltin "$x64Source" "$zip64"
}
Write-Host "Created: $zip64" -ForegroundColor Green

Write-Host "********** Package Done. **********" -ForegroundColor Green

# 暂停等待用户输入
Read-Host "Press Enter to exit"
