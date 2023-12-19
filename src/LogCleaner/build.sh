#!/bin/bash

# Linux build
dotnet publish -r linux-x64 -p:PublishSingleFile=true --self-contained true

# Windows build
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true
