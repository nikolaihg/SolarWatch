FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file (from repo root)
COPY backend/SolarWatch.Api/SolarWatch.Api.csproj ./Api/

# Restore dependencies
RUN dotnet restore ./Api/SolarWatch.Api.csproj

# Copy source code
COPY backend/SolarWatch.Api ./Api/

# Build and publish
WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Set environment to Docker to load appsettings.Docker.json
ENV ASPNETCORE_ENVIRONMENT=Docker

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "SolarWatch.Api.dll"]
