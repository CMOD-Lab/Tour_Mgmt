# Tour_Management - Deployment Guide

## Overview

This guide covers building, containerizing, and deploying the **Tour_Management** ASP.NET Web Forms application (.NET Framework 4.7.2) to **Azure Kubernetes Service (AKS)** using Windows node pools.

| Property | Value |
|---|---|
| Application | Tour_Management |
| Framework | .NET Framework 4.7.2 |
| Application Type | ASP.NET Web Forms (IIS-hosted) |
| Container OS | Windows Server 2019 |
| Build Image | `mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019` |
| Runtime Image | `mcr.microsoft.com/dotnet/framework/runtime:4.8` |
| Port | 80 (HTTP) |
| Health Endpoint | `GET /health` |
| Target Platform | Azure AKS (Windows node pool) |

---

## Prerequisites

### Local Development
- **Docker Desktop** (Windows containers mode enabled)
- **Windows 10/11** or **Windows Server 2019+** (required for Windows containers)
- **Visual Studio 2019+** or **MSBuild 16+**
- **NuGet CLI** (`nuget.exe` in PATH)

### Azure AKS Deployment
- **Azure CLI** (`az`) – [Install](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- **kubectl** – [Install](https://kubernetes.io/docs/tasks/tools/)
- **Azure Subscription** with permissions to create AKS clusters and ACR
- **Azure Container Registry (ACR)** or Docker Hub account
- **AKS cluster with Windows node pool** (see AKS Setup section)

---

## Project Structure

```
Tour_Management/
├── Dockerfile                    # Multi-stage Windows container build
├── .dockerignore                 # Excludes build artifacts from Docker context
├── docker-compose.yml            # Local development with Docker Compose
├── Web.config                    # ASP.NET configuration (DB via env var)
├── HealthCheckHandler.cs         # Health check endpoint at GET /health
├── kubernetes/
│   ├── namespace.yaml            # Kubernetes namespace: tour-management
│   ├── deployment.yaml           # Deployment with 2 replicas, health probes
│   ├── service.yaml              # ClusterIP service on port 80
│   └── ingress.yaml              # Azure Application Gateway Ingress
└── scripts/
    ├── build-push.sh             # Linux/macOS: build and push to ACR/Docker Hub
    ├── build-push.bat            # Windows: build and push to ACR/Docker Hub
    ├── deploy-image.sh           # Linux/macOS: deploy to AKS
    └── deploy-image.bat          # Windows: deploy to AKS
```

---

## Environment Variables

The application reads the following environment variables at runtime:

| Variable | Required | Description |
|---|---|---|
| `DB_CONNECTION_STRING` | Yes | SQL Server connection string |
| `AZURE_STORAGE_CONNECTION_STRING` | No | Azure Blob Storage connection string for tour image uploads |
| `AZURE_BLOB_CONTAINER_NAME` | No | Azure Blob Storage container name (default: `tour-pics`) |

### Example Connection String
```
Server=tcp:myserver.database.windows.net,1433;Initial Catalog=tourdb;Persist Security Info=False;User ID=myuser;Password=mypassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

---

## Local Development with Docker Compose

### Step 1: Switch Docker Desktop to Windows Containers
Right-click the Docker Desktop tray icon → **Switch to Windows containers**.

### Step 2: Create a `.env` file
```env
DB_CONNECTION_STRING=Server=localhost;Database=tourdb;User Id=sa;Password=YourPassword123;
AZURE_STORAGE_CONNECTION_STRING=DefaultEndpointsProtocol=https;AccountName=...
AZURE_BLOB_CONTAINER_NAME=tour-pics
```

### Step 3: Build and run
```bash
docker-compose up --build
```

### Step 4: Access the application
- Application: http://localhost/
- Health check: http://localhost/health

### Step 5: Stop the application
```bash
docker-compose down
```

---

## Building and Pushing the Docker Image

### Linux/macOS
```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

### Windows
```cmd
scripts\build-push.bat
```

The script will prompt you to:
1. Select registry type (Azure ACR or Docker Hub)
2. Enter registry credentials
3. Enter an image tag (defaults to `latest`)

The image is built from the **repository root** using:
```bash
docker build -f DotNetFrameworkProject_CE040_CE087/Tour_Management/Dockerfile -t <registry>/tour-management:<tag> .
```

> **Note**: Windows containers can only be built on Windows hosts with Docker Desktop in Windows container mode.

---

## Azure AKS Setup

### Step 1: Login to Azure
```bash
az login
az account set --subscription "<your-subscription-id>"
```

### Step 2: Create Azure Container Registry (if not existing)
```bash
az acr create \
  --resource-group <resource-group> \
  --name <acr-name> \
  --sku Basic
```

### Step 3: Create AKS Cluster with Windows Node Pool
```bash
# Create AKS cluster with a Linux system node pool
az aks create \
  --resource-group <resource-group> \
  --name <cluster-name> \
  --node-count 1 \
  --generate-ssh-keys \
  --windows-admin-username azureuser \
  --windows-admin-password "YourPassword123!" \
  --network-plugin azure \
  --enable-addons ingress-appgw \
  --appgw-name tour-management-agw \
  --appgw-subnet-cidr "10.225.0.0/16"

# Add Windows node pool
az aks nodepool add \
  --resource-group <resource-group> \
  --cluster-name <cluster-name> \
  --os-type Windows \
  --name winnp \
  --node-count 2 \
  --node-vm-size Standard_D4s_v3
```

### Step 4: Attach ACR to AKS
```bash
az aks update \
  --resource-group <resource-group> \
  --name <cluster-name> \
  --attach-acr <acr-name>
```

### Step 5: Configure kubectl
```bash
az aks get-credentials \
  --resource-group <resource-group> \
  --name <cluster-name>
kubectl cluster-info
```

---

## Deploying to AKS

### Linux/macOS
```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

### Windows
```cmd
scripts\deploy-image.bat
```

The script will prompt for:
- Azure Resource Group name
- AKS Cluster name
- Full Docker image URI (e.g., `myregistry.azurecr.io/tour-management:latest`)
- Environment variable values (`DB_CONNECTION_STRING`, `AZURE_STORAGE_CONNECTION_STRING`, `AZURE_BLOB_CONTAINER_NAME`)

### Manual Deployment
```bash
# Update deployment.yaml with your image URI
sed -i 's|{{IMAGE_URI}}|myregistry.azurecr.io/tour-management:latest|g' kubernetes/deployment.yaml
sed -i 's|{{DB_CONNECTION_STRING}}|<your-connection-string>|g' kubernetes/deployment.yaml

# Apply manifests
kubectl apply -f kubernetes/namespace.yaml
kubectl apply -f kubernetes/deployment.yaml
kubectl apply -f kubernetes/service.yaml
kubectl apply -f kubernetes/ingress.yaml

# Wait for rollout
kubectl rollout status deployment/tour-management -n tour-management
```

---

## Kubernetes Manifest Descriptions

### namespace.yaml
Creates the `tour-management` Kubernetes namespace to isolate all application resources.

### deployment.yaml
- **Replicas**: 2 (for high availability)
- **Node Selector**: `kubernetes.io/os: windows` (targets Windows node pool)
- **Image**: Placeholder `{{IMAGE_URI}}` replaced at deploy time
- **Liveness Probe**: `GET /health` — restarts container if unhealthy (after 90s initial delay)
- **Readiness Probe**: `GET /health` — removes pod from load balancer if not ready (after 60s initial delay)
- **Resources**: requests `250m CPU / 512Mi RAM`, limits `500m CPU / 1Gi RAM`

### service.yaml
- **Type**: ClusterIP — internal cluster access only
- **Port**: 80 → container port 80

### ingress.yaml
- **Class**: `azure/application-gateway` — uses Azure Application Gateway Ingress Controller (AGIC)
- **Host**: `tour-management.example.com` — update to your actual domain
- **Path**: `/` (all traffic routed to the application)

---

## Verifying the Deployment

```bash
# Check pod status
kubectl get pods -n tour-management

# Check pod logs
kubectl logs -l app=tour-management -n tour-management

# Check service
kubectl get svc -n tour-management

# Check ingress and get external IP
kubectl get ingress -n tour-management

# Test health endpoint
curl http://<ingress-ip>/health
# Expected: {"status":"healthy","application":"Tour_Management"}
```

---

## Scaling and Management

### Scale replicas
```bash
kubectl scale deployment/tour-management --replicas=3 -n tour-management
```

### Rolling update (new image)
```bash
kubectl set image deployment/tour-management \
  tour-management=myregistry.azurecr.io/tour-management:v2.0 \
  -n tour-management
kubectl rollout status deployment/tour-management -n tour-management
```

### Rollback
```bash
kubectl rollout undo deployment/tour-management -n tour-management
kubectl rollout status deployment/tour-management -n tour-management
```

### Horizontal Pod Autoscaler
```bash
kubectl autoscale deployment/tour-management \
  --cpu-percent=70 \
  --min=2 \
  --max=10 \
  -n tour-management
```

---

## Troubleshooting

### Pod not starting
```bash
kubectl describe pod -l app=tour-management -n tour-management
kubectl logs -l app=tour-management -n tour-management --previous
```

**Common causes**:
- `DB_CONNECTION_STRING` not set or incorrect → check environment variable
- Windows node pool not available → verify `kubectl get nodes -l kubernetes.io/os=windows`
- Image pull failure → verify ACR attachment: `az aks check-acr --name <cluster> --resource-group <rg> --image <image>`

### Health check failing
```bash
# Exec into pod (Windows)
kubectl exec -it <pod-name> -n tour-management -- powershell
# Inside pod:
Invoke-WebRequest -Uri http://localhost/health -UseBasicParsing
```

**Common causes**:
- IIS not started → check `w3svc` service in container
- Application pool crashed → check Windows Event Log in container
- Database connection failure → verify `DB_CONNECTION_STRING`

### Ingress not getting external IP
```bash
kubectl get ingress -n tour-management
kubectl describe ingress tour-management-ingress -n tour-management
```

**Common causes**:
- AGIC not installed → verify `az aks addon show --name ingress-appgw`
- Application Gateway not provisioned → check Azure portal

### Database connection issues
- Ensure SQL Server allows connections from AKS pod CIDR
- For Azure SQL Database, add AKS outbound IP to firewall rules
- Verify connection string format for Azure SQL

---

## Security Considerations

1. **Secrets Management**: Store `DB_CONNECTION_STRING` and `AZURE_STORAGE_CONNECTION_STRING` in Azure Key Vault and use the [Key Vault CSI Driver](https://docs.microsoft.com/en-us/azure/aks/csi-secrets-store-driver) to inject them as environment variables.

2. **Network Policies**: Restrict pod-to-pod communication using Kubernetes NetworkPolicies.

3. **HTTPS**: Configure TLS termination at the Application Gateway level with a certificate from Azure Key Vault.

4. **Image Scanning**: Enable Azure Defender for container registries to scan images for vulnerabilities.

5. **Managed Identity**: Use AKS Managed Identity for ACR pull and Azure Blob Storage access instead of connection strings where possible.

6. **Ingress Host**: Update `tour-management.example.com` in `kubernetes/ingress.yaml` to your actual domain before deploying.

---

## .NET Framework Specific Notes

- **Windows Containers Only**: .NET Framework 4.7.2 requires Windows containers. Ensure your AKS cluster has a Windows node pool.
- **IIS Hosting**: The application runs under IIS inside the container. The `ServiceMonitor.exe` entrypoint keeps the container alive by monitoring the `w3svc` Windows service.
- **Application Pool**: Configured for .NET 4.0 CLR, 64-bit, integrated pipeline mode.
- **Health Check**: Implemented via `HealthCheckHandler.cs` registered in `Web.config` at path `/health`. Returns `{"status":"healthy","application":"Tour_Management"}`.
- **Database**: Connection string is read from the `DB_CONNECTION_STRING` environment variable (not `Web.config`) to support AKS ConfigMaps and Azure Key Vault CSI Driver.
- **Azure Blob Storage**: Tour images are stored in Azure Blob Storage using the `Azure.Storage.Blobs` SDK. Configure `AZURE_STORAGE_CONNECTION_STRING` and `AZURE_BLOB_CONTAINER_NAME`.
- **Startup Time**: Windows containers take longer to start than Linux containers. The liveness probe has a 90-second initial delay and the readiness probe has a 60-second initial delay to accommodate IIS startup.
