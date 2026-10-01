# ============================================================
# Dockerfile for Tour_Management ASP.NET Web Forms Application
# .NET Framework 4.7.2 | IIS-hosted | Windows Container
# Build Stage : mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019
# Runtime Stage: mcr.microsoft.com/dotnet/framework/runtime:4.8
# Target Platform: Azure AKS (Windows node pool)
# ============================================================

# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build
WORKDIR /app/src

# Copy solution and project files first for NuGet layer caching
COPY ["Tour_Management.sln", "./"]
COPY ["Tour_Management.csproj", "./"]
COPY ["packages.config", "./"]

# Restore NuGet packages (cached layer - only invalidated when project/packages files change)
RUN nuget restore Tour_Management.csproj -PackagesDirectory ..\packages

# Copy remaining source files
COPY . .

# Build and publish the Web Forms application in Release mode
# /p:DebugSymbols=false and /p:DebugType=none ensure no PDB files are generated
# /p:DeleteExistingFiles=True ensures a clean publish output
RUN msbuild Tour_Management.csproj \
    /p:Configuration=Release \
    /p:DeployOnBuild=true \
    /p:WebPublishMethod=FileSystem \
    /p:PublishUrl=C:\publish \
    /p:DeleteExistingFiles=True \
    /p:DebugSymbols=false \
    /p:DebugType=none \
    /p:platform=AnyCPU \
    /p:VisualStudioVersion=16.0

# Remove PDB files, source code, and temp files from publish output to minimize image size
RUN powershell -Command \
    Get-ChildItem -Path C:\publish -Recurse -Include *.pdb,*.mdb,*.cs,*.vb | Remove-Item -Force; \
    if (Test-Path C:\publish\bin\roslyn) { Remove-Item -Recurse -Force C:\publish\bin\roslyn }

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8

# Set working directory to IIS default web root
WORKDIR /inetpub/wwwroot

# Remove default IIS welcome page
RUN powershell -Command Remove-Item -Recurse -Force C:\inetpub\wwwroot\*

# Copy only the published application output from build stage (no source, no PDB files)
COPY --from=build C:\publish .

# Configure IIS: Set application pool to use .NET 4.0 CLR
RUN powershell -Command \
    Import-Module WebAdministration; \
    Set-ItemProperty 'IIS:\AppPools\DefaultAppPool' managedRuntimeVersion v4.0; \
    Set-ItemProperty 'IIS:\AppPools\DefaultAppPool' enable32BitAppOnWin64 False; \
    Set-WebConfigurationProperty -Filter '/system.webServer/security/requestFiltering' \
        -Name 'allowDoubleEscaping' -Value 'true' -PSPath 'IIS:\Sites\Default Web Site'

# Expose HTTP port
EXPOSE 80

# Start IIS in the foreground to keep the container running
ENTRYPOINT ["C:\\ServiceMonitor.exe", "w3svc"]
