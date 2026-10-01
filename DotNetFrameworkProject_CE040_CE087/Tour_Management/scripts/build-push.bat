@echo off
setlocal enabledelayedexpansion

REM =============================================================
REM build-push.bat - Build and Push Docker Image
REM Application : Tour_Management (.NET Framework 4.7.2 Web Forms)
REM Platform    : Windows
REM Target      : Azure ACR or Docker Hub
REM =============================================================

set "PROJECT_NAME=tour-management"

REM Sanitize image name using PowerShell
for /f "delims=" %%i in ('powershell -NoProfile -Command "$n = 'tour-management' -replace '[^a-z0-9]','-'; $n = $n.ToLower().Trim('-'); while($n -match '--') { $n = $n -replace '--','-' }; $n"') do set "IMAGE_NAME=%%i"

echo ==============================================
echo  Tour_Management - Docker Build ^& Push
echo ==============================================
echo.

REM Prompt for image tag
set /p "IMAGE_TAG=Enter image tag [latest]: "
if "!IMAGE_TAG!"=="" set "IMAGE_TAG=latest"

REM Sanitize tag
for /f "delims=" %%i in ('powershell -NoProfile -Command "$t = '!IMAGE_TAG!' -replace '[^a-z0-9._-]','-'; $t = $t.ToLower().Trim('-'); if($t -eq '') { $t = 'latest' }; $t"') do set "IMAGE_TAG=%%i"
echo Using image tag: !IMAGE_TAG!
echo.

REM Registry selection
echo Select container registry:
echo   1. Azure Container Registry (ACR)
echo   2. Docker Hub
set /p "REGISTRY_CHOICE=Enter choice [1]: "
if "!REGISTRY_CHOICE!"=="" set "REGISTRY_CHOICE=1"

if "!REGISTRY_CHOICE!"=="1" goto :acr_login
if "!REGISTRY_CHOICE!"=="2" goto :dockerhub_login
echo ERROR: Invalid registry choice. Please enter 1 or 2.
exit /b 1

:acr_login
echo.
echo --- Azure Container Registry ---
set /p "ACR_NAME=Enter ACR name (e.g. myregistry): "
if "!ACR_NAME!"=="" (
    echo ERROR: ACR name cannot be empty.
    exit /b 1
)
for /f "delims=" %%i in ('powershell -NoProfile -Command "'!ACR_NAME!'.ToLower()"') do set "ACR_NAME=%%i"
set "REGISTRY=!ACR_NAME!.azurecr.io"
set "FULL_IMAGE_NAME=!REGISTRY!/!IMAGE_NAME!:!IMAGE_TAG!"

echo.
echo Logging in to Azure Container Registry: !REGISTRY!
az acr login --name !ACR_NAME!
if !ERRORLEVEL! neq 0 (
    echo ERROR: ACR login failed.
    exit /b 1
)
goto :build_image

:dockerhub_login
echo.
echo --- Docker Hub ---
set /p "DOCKER_USERNAME=Enter Docker Hub username: "
if "!DOCKER_USERNAME!"=="" (
    echo ERROR: Docker Hub username cannot be empty.
    exit /b 1
)
set /p "DOCKER_PASSWORD=Enter Docker Hub password/token: "
if "!DOCKER_PASSWORD!"=="" (
    echo ERROR: Docker Hub password cannot be empty.
    exit /b 1
)
set "FULL_IMAGE_NAME=!DOCKER_USERNAME!/!IMAGE_NAME!:!IMAGE_TAG!"

echo.
echo Logging in to Docker Hub...
echo !DOCKER_PASSWORD! | docker login --username !DOCKER_USERNAME! --password-stdin
if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker Hub login failed.
    exit /b 1
)
goto :build_image

:build_image
echo.
echo Building Docker image: !FULL_IMAGE_NAME!
echo Build context: repository root (.)
echo Dockerfile: DotNetFrameworkProject_CE040_CE087\Tour_Management\Dockerfile
echo.

docker build -f DotNetFrameworkProject_CE040_CE087\Tour_Management\Dockerfile -t "!FULL_IMAGE_NAME!" .
if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker build failed.
    exit /b 1
)

echo.
echo Successfully built: !FULL_IMAGE_NAME!
echo.
echo Pushing image to registry...
docker push "!FULL_IMAGE_NAME!"
if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker push failed.
    exit /b 1
)

echo.
echo ==============================================
echo  Build and push completed successfully!
echo  Image: !FULL_IMAGE_NAME!
echo ==============================================

endlocal
