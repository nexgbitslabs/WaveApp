# ============================================================
# WaveApp Backend - Production Image
# .NET 8 / ASP.NET Core
# ============================================================


# ============================================================
# STAGE 1 - BUILD
# ============================================================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src


# ------------------------------------------------------------
# Copy project files first for Docker layer caching
# ------------------------------------------------------------

COPY ["WaveApp.Api/WaveApp.Api.csproj", "WaveApp.Api/"]
COPY ["WaveApp.Core/WaveApp.Core.csproj", "WaveApp.Core/"]
COPY ["WaveApp.Infrastructure/WaveApp.Infrastructure.csproj", "WaveApp.Infrastructure/"]


# ------------------------------------------------------------
# Restore NuGet packages
# ------------------------------------------------------------

RUN dotnet restore "WaveApp.Api/WaveApp.Api.csproj"


# ------------------------------------------------------------
# Copy application source
# ------------------------------------------------------------

COPY . .


# ------------------------------------------------------------
# Publish
# ------------------------------------------------------------

WORKDIR "/src/WaveApp.Api"

RUN dotnet publish \
    "WaveApp.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


# ============================================================
# STAGE 2 - PRODUCTION RUNTIME
# ============================================================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app


# ------------------------------------------------------------
# Production environment
# ------------------------------------------------------------

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8085


# ------------------------------------------------------------
# Internal application port
# ------------------------------------------------------------

EXPOSE 8085


# ------------------------------------------------------------
# Copy only published output
# ------------------------------------------------------------

COPY --from=build /app/publish .


# ------------------------------------------------------------
# Run as the built-in non-root .NET container user
# ------------------------------------------------------------

USER app


# ------------------------------------------------------------
# Start WaveApp
# ------------------------------------------------------------

ENTRYPOINT ["dotnet", "WaveApp.Api.dll"]