# Two Stage Docker Build 
# Build from DOTNET SDK 7 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app