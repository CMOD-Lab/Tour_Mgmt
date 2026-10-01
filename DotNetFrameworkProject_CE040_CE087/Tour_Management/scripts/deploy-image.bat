@echo off
setlocal enabledelayedexpansion

REM =============================================================
REM deploy-image.bat - Deploy Tour_Management to Azure AKS
REM Application : Tour_Management (.NET Framework 4.7.2 Web Forms)
REM Platform    : Windows
REM Target      : Azure AKS (Windows node pool)
REM =============================================================

set "NAMESPACE=tour-management"
set "APP_NAME=tour-management"
set "SCRIPT_DIR=%~dp0"
set "K8S_DIR=%SCRIPT_DIR%..\kubernetes"
set "TEMP_DIR=%TEMP%\tour-management-deploy"

echo ==============================================
echo  Tour_Management - Deploy to Azure AKS
echo ==============================================
echo.

REM ---- Azure / AKS Configuration ----
set /p "RESOURCE_GROUP=Enter Azure Resource Group name: "
if "!RESOURCE_GROUP!"=="" (
    echo ERROR: Resource group cannot be empty.
    exit /b 1
)

set /p "CLUSTER_NAME=Enter AKS Cluster name: "
if "!CLUSTER_NAME!"=="" (
    echo ERROR: AKS cluster name cannot be empty.
    exit /b 1
)

REM ---- Docker Image URI ----
set /p "IMAGE_URI=Enter full Docker image URI (e.g. myregistry.azurecr.io/tour-management:latest): "
if "!IMAGE_URI!"=="" (
    echo ERROR: Image URI cannot be empty.
    exit /b 1
)

echo.
echo --- Application Environment Variables ---
echo Press Enter to skip any variable.
echo.

set /p "DB_CONNECTION_STRING=Enter DB_CONNECTION_STRING (SQL Server connection string): "
set /p "AZURE_STORAGE_CONNECTION_STRING=Enter AZURE_STORAGE_CONNECTION_STRING (Azure Blob Storage): "
set /p "AZURE_BLOB_CONTAINER_NAME=Enter AZURE_BLOB_CONTAINER_NAME [tour-pics]: "
if "!AZURE_BLOB_CONTAINER_NAME!"=="" set "AZURE_BLOB_CONTAINER_NAME=tour-pics"

echo.
echo --- Configuring kubectl for AKS cluster ---
az aks get-credentials --resource-group "!RESOURCE_GROUP!" --name "!CLUSTER_NAME!" --overwrite-existing
if !ERRORLEVEL! neq 0 (
    echo ERROR: Failed to get AKS credentials.
    exit /b 1
)
echo kubectl configured for cluster: !CLUSTER_NAME!

echo.
echo --- Verifying cluster connectivity ---
kubectl cluster-info
if !ERRORLEVEL! neq 0 (
    echo ERROR: Cannot connect to AKS cluster.
    exit /b 1
)

echo.
echo --- Preparing manifest copies ---
if not exist "!TEMP_DIR!" mkdir "!TEMP_DIR!"

copy /Y "!K8S_DIR!\deployment.yaml" "!TEMP_DIR!\deployment.yaml" >nul
copy /Y "!K8S_DIR!\service.yaml"    "!TEMP_DIR!\service.yaml"    >nul
copy /Y "!K8S_DIR!\ingress.yaml"    "!TEMP_DIR!\ingress.yaml"    >nul
copy /Y "!K8S_DIR!\namespace.yaml"  "!TEMP_DIR!\namespace.yaml"  >nul

echo --- Updating manifests with deployment values ---
powershell -NoProfile -Command ^
  "(Get-Content '!TEMP_DIR!\deployment.yaml') ^
   -replace '\{\{IMAGE_URI\}\}','!IMAGE_URI!' ^
   -replace '\{\{DB_CONNECTION_STRING\}\}','!DB_CONNECTION_STRING!' ^
   -replace '\{\{AZURE_STORAGE_CONNECTION_STRING\}\}','!AZURE_STORAGE_CONNECTION_STRING!' ^
   -replace '\{\{AZURE_BLOB_CONTAINER_NAME\}\}','!AZURE_BLOB_CONTAINER_NAME!' ^
   | Set-Content '!TEMP_DIR!\deployment.yaml'"
if !ERRORLEVEL! neq 0 (
    echo ERROR: Failed to update deployment manifest.
    exit /b 1
)
echo Manifests updated successfully.

echo.
echo --- Applying Kubernetes manifests ---

echo [1/4] Applying namespace...
kubectl apply -f "!TEMP_DIR!\namespace.yaml"
if !ERRORLEVEL! neq 0 ( echo ERROR: Failed to apply namespace. & exit /b 1 )

echo [2/4] Applying deployment...
kubectl apply -f "!TEMP_DIR!\deployment.yaml"
if !ERRORLEVEL! neq 0 ( echo ERROR: Failed to apply deployment. & exit /b 1 )

echo [3/4] Applying service...
kubectl apply -f "!TEMP_DIR!\service.yaml"
if !ERRORLEVEL! neq 0 ( echo ERROR: Failed to apply service. & exit /b 1 )

echo [4/4] Applying ingress...
kubectl apply -f "!TEMP_DIR!\ingress.yaml"
if !ERRORLEVEL! neq 0 ( echo ERROR: Failed to apply ingress. & exit /b 1 )

echo.
echo --- Waiting for deployment rollout ---
kubectl rollout status deployment/!APP_NAME! -n !NAMESPACE! --timeout=300s
if !ERRORLEVEL! neq 0 (
    echo ERROR: Deployment rollout failed.
    echo To rollback run: kubectl rollout undo deployment/!APP_NAME! -n !NAMESPACE!
    exit /b 1
)

echo.
echo --- Verifying deployed resources ---
kubectl get pods,svc,ingress -n !NAMESPACE!

echo.
echo --- Rollback Instructions ---
echo If the deployment fails, run:
echo   kubectl rollout undo deployment/!APP_NAME! -n !NAMESPACE!

echo.
echo ==============================================
echo  Deployment completed successfully!
echo  Namespace : !NAMESPACE!
echo  Image     : !IMAGE_URI!
echo ==============================================

REM Cleanup temp files
if exist "!TEMP_DIR!" rmdir /s /q "!TEMP_DIR!"

endlocal
