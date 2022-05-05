@echo off
title Publishing
call dotnet publish --configuration Release -r linux-x64 --self-contained false

cd bin\Release\net6.0\linux-x64\publish
start %cd%
