@echo off
echo ********** Build And Package Batch. **********

echo ********** Build... **********
rem set msbuild.exe path
set PATH=%PATH%;C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\

echo ********** Build Release_x86 **********
msbuild ytgdq.sln -p:Configuration=Release -p:Platform=x86

echo ********** Build Release_x64 **********
msbuild ytgdq.sln -p:Configuration=Release -p:Platform=x64

echo ********** Build Done. **********
echo ********** Package... **********

echo ********** Package Release_x86 **********
"./Tools/7-Zip/7zr.exe" a -r ./Release/ytgdq_x86.zip ./Release/x86/*

echo ********** Package Release_x64 **********
"./Tools/7-Zip/7zr.exe" a -r ./Release/ytgdq_x64.zip ./Release/x64/*

echo ********** Package Done. **********
pause>NUL
exit
