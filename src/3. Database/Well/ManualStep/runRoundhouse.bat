@echo off

SET DIR=%~d0%~p0%
SET sql.files.directory="%DIR%scripts"

"%DIR%roundhouse.0.8.6\bin\rh.exe" /d=Well /f=%sql.files.directory% /s=.\SQLSERVER2014 --dnc=true --silent=true

