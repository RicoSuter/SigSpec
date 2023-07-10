rem @echo off

setlocal EnableDelayedExpansion

for /d %%d in (*) do (
    if exist "%%d\bin" (
        echo Deleting %%d\bin
        rd /s /q "%%d\bin"
    )
    if exist "%%d\obj" (
        echo Deleting %%d\obj
        rd /s /q "%%d\obj"
    )
)