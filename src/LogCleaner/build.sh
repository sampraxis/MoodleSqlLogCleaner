#!/bin/bash

# Linux build
dotnet publish -r linux-x64 -p:PublishSingleFile=true --self-contained false

# Windows build
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained false
