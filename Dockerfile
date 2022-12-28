# Two Stage Docker Build 
# Build from DOTNET SDK 7 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build from DOTNET Runtime 7
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "RedisAPI.dll"]