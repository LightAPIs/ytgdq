# Build And Package PowerShell Script for .NET 8

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

# 函数：获取当前 Git 仓库的最新 tag
function Get-LatestGitTag {
    try {
        $latestTag = git describe --tags $(git rev-list --tags --max-count=1) 2>$null
        if ($latestTag -and $latestTag -notmatch '^fatal|error') {
            return $latestTag.Trim()
        }

        $latestTag = git tag --sort=committerdate | Select-Object -Last 1
        if ($latestTag) {
            return $latestTag.Trim()
        }

        Write-Warning "Unable to get Git latest tag, default filename used"
        return $null
    }
    catch {
        Write-Warning "Git command execution failed: $($_.Exception.Message)"
        return $null
    }
}

# 函数：使用 7zr 或 7z 压缩（优先 7zr）
function Compress-With7z {
    param($sourcePath, $destinationPath)

    # 优先尝试 7zr (从环境变量获取路径)
    $sevenZrPath = $env:7ZR_PATH
    if ($sevenZrPath -and (Test-Path $sevenZrPath)) {
        Write-Host "Using 7zr LZMA2 for compression (path: $sevenZrPath)..." -ForegroundColor Cyan
        & $sevenZrPath a -r -mx=9 "$destinationPath" "$sourcePath\*" | Out-Null
        if ($LASTEXITCODE -eq 0) { return $true }
        Write-Host "7zr failed (exit code: $LASTEXITCODE), falling back to 7z..." -ForegroundColor Yellow
    }

    # 回退到 7z
    Write-Host "Using 7z LZMA for compression..." -ForegroundColor Cyan
    if (Test-CommandExists "7z") {
        & 7z a -r -mx=9 "$destinationPath" "$sourcePath\*" | Out-Null
        return $LASTEXITCODE -eq 0
    }
    return $false
}

# 函数：使用 Compress-Archive 压缩
function Compress-WithBuiltin {
    param($sourcePath, $destinationPath)
    Write-Host "Using built-in Compress-Archive for compression..." -ForegroundColor Cyan
    Compress-Archive -Path "$sourcePath\*" -DestinationPath $destinationPath -CompressionLevel Optimal -Force
    return $true
}

Write-Host "********** Build And Package PowerShell (.NET 8) **********" -ForegroundColor Green

Write-Host "********** Get latest git tag... **********" -ForegroundColor Cyan
$latestTag = Get-LatestGitTag
if ($latestTag) {
    Write-Host "latest tag: $latestTag" -ForegroundColor Green
    $versionSuffix = "_$latestTag"
} else {
    Write-Host "Tag not found, default filename used" -ForegroundColor Yellow
    $versionSuffix = ""
}

Write-Host "********** Build... **********" -ForegroundColor Yellow

# 确保 Release 目录存在
$releaseDir = "./Release"
if (-not (Test-Path $releaseDir)) { New-Item -ItemType Directory -Path $releaseDir -Force }

Write-Host "********** Build Release_x86 **********" -ForegroundColor Yellow
$x86Output = Join-Path $releaseDir "x86"
dotnet publish "WindowsFormsApplication2/FollowingTypingApplication.csproj" -c Release -r win-x86 --self-contained false -o $x86Output
if ($LASTEXITCODE -ne 0) { Write-Error "x86 build failed"; exit $LASTEXITCODE }

Write-Host "********** Build Release_x64 **********" -ForegroundColor Yellow
$x64Output = Join-Path $releaseDir "x64"
dotnet publish "WindowsFormsApplication2/FollowingTypingApplication.csproj" -c Release -r win-x64 --self-contained false -o $x64Output
if ($LASTEXITCODE -ne 0) { Write-Error "x64 build failed"; exit $LASTEXITCODE }

Write-Host "********** Build Done. **********" -ForegroundColor Green
Write-Host "********** Package... **********" -ForegroundColor Yellow

Write-Host "********** Package Release_x86 **********" -ForegroundColor Yellow
$zip86 = Join-Path $releaseDir "ytgdq_x86$versionSuffix.zip"
if (Test-Path $zip86) { Remove-Item $zip86 -Force }
if (Compress-With7z "$x86Output" "$zip86") {
    Write-Host "Created: $zip86" -ForegroundColor Green
} else {
    Write-Host "7z/7zr failed, trying built-in Compress-Archive..." -ForegroundColor Yellow
    Compress-WithBuiltin "$x86Output" "$zip86"
    Write-Host "Created: $zip86 (using built-in)" -ForegroundColor Green
}

Write-Host "********** Package Release_x64 **********" -ForegroundColor Yellow
$zip64 = Join-Path $releaseDir "ytgdq_x64$versionSuffix.zip"
if (Test-Path $zip64) { Remove-Item $zip64 -Force }
if (Compress-With7z "$x64Output" "$zip64") {
    Write-Host "Created: $zip64" -ForegroundColor Green
} else {
    Write-Host "7z/7zr failed, trying built-in Compress-Archive..." -ForegroundColor Yellow
    Compress-WithBuiltin "$x64Output" "$zip64"
    Write-Host "Created: $zip64 (using built-in)" -ForegroundColor Green
}

Write-Host "********** Package Done. **********" -ForegroundColor Green

# 暂停等待用户输入
Read-Host "Press Enter to exit"
