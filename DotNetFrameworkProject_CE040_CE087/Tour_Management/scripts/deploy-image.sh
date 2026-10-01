#!/bin/bash
# =============================================================
# deploy-image.sh - Deploy Tour_Management to Azure AKS
# Application : Tour_Management (.NET Framework 4.7.2 Web Forms)
# Platform    : Linux/macOS
# Target      : Azure AKS (Windows node pool)
# =============================================================
set -e
set -o pipefail

NAMESPACE="tour-management"
APP_NAME="tour-management"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
K8S_DIR="${SCRIPT_DIR}/../kubernetes"

echo "=============================================="
echo " Tour_Management - Deploy to Azure AKS"
echo "=============================================="
echo ""

# ---- Azure / AKS Configuration ----
read -p "Enter Azure Resource Group name: " RESOURCE_GROUP
if [ -z "$RESOURCE_GROUP" ]; then
  echo "ERROR: Resource group cannot be empty."
  exit 1
fi

read -p "Enter AKS Cluster name: " CLUSTER_NAME
if [ -z "$CLUSTER_NAME" ]; then
  echo "ERROR: AKS cluster name cannot be empty."
  exit 1
fi

# ---- Docker Image URI ----
read -p "Enter full Docker image URI (e.g. myregistry.azurecr.io/tour-management:latest): " IMAGE_URI
if [ -z "$IMAGE_URI" ]; then
  echo "ERROR: Image URI cannot be empty."
  exit 1
fi

echo ""
echo "--- Application Environment Variables ---"
echo "Press Enter to skip any variable (placeholder will remain in manifest)."
echo ""

# DB_CONNECTION_STRING
read -p "Enter DB_CONNECTION_STRING (SQL Server connection string): " DB_CONNECTION_STRING
DB_CONNECTION_STRING="${DB_CONNECTION_STRING:-}"

# AZURE_STORAGE_CONNECTION_STRING
read -p "Enter AZURE_STORAGE_CONNECTION_STRING (Azure Blob Storage): " AZURE_STORAGE_CONNECTION_STRING
AZURE_STORAGE_CONNECTION_STRING="${AZURE_STORAGE_CONNECTION_STRING:-}"

# AZURE_BLOB_CONTAINER_NAME
read -p "Enter AZURE_BLOB_CONTAINER_NAME [tour-pics]: " AZURE_BLOB_CONTAINER_NAME
AZURE_BLOB_CONTAINER_NAME="${AZURE_BLOB_CONTAINER_NAME:-tour-pics}"

echo ""
echo "--- Configuring kubectl for AKS cluster ---"
az aks get-credentials --resource-group "$RESOURCE_GROUP" --name "$CLUSTER_NAME" --overwrite-existing
echo "kubectl configured for cluster: $CLUSTER_NAME"

echo ""
echo "--- Verifying cluster connectivity ---"
kubectl cluster-info || { echo "ERROR: Cannot connect to AKS cluster."; exit 1; }

echo ""
echo "--- Updating Kubernetes manifests with deployment values ---"

# Work on copies to avoid modifying originals
cp "${K8S_DIR}/deployment.yaml" /tmp/tour-management-deployment.yaml
cp "${K8S_DIR}/service.yaml"    /tmp/tour-management-service.yaml
cp "${K8S_DIR}/ingress.yaml"    /tmp/tour-management-ingress.yaml
cp "${K8S_DIR}/namespace.yaml"  /tmp/tour-management-namespace.yaml

# Replace placeholders using pipe delimiter
sed -i 's|{{IMAGE_URI}}|'"$IMAGE_URI"'|g'                                         /tmp/tour-management-deployment.yaml
sed -i 's|{{DB_CONNECTION_STRING}}|'"$DB_CONNECTION_STRING"'|g'                   /tmp/tour-management-deployment.yaml
sed -i 's|{{AZURE_STORAGE_CONNECTION_STRING}}|'"$AZURE_STORAGE_CONNECTION_STRING"'|g' /tmp/tour-management-deployment.yaml
sed -i 's|{{AZURE_BLOB_CONTAINER_NAME}}|'"$AZURE_BLOB_CONTAINER_NAME"'|g'         /tmp/tour-management-deployment.yaml

echo "Manifests updated successfully."

echo ""
echo "--- Applying Kubernetes manifests ---"

echo "[1/4] Applying namespace..."
kubectl apply -f /tmp/tour-management-namespace.yaml

echo "[2/4] Applying deployment..."
kubectl apply -f /tmp/tour-management-deployment.yaml

echo "[3/4] Applying service..."
kubectl apply -f /tmp/tour-management-service.yaml

echo "[4/4] Applying ingress..."
kubectl apply -f /tmp/tour-management-ingress.yaml

echo ""
echo "--- Waiting for deployment rollout ---"
kubectl rollout status deployment/${APP_NAME} -n ${NAMESPACE} --timeout=300s

echo ""
echo "--- Verifying deployed resources ---"
kubectl get pods,svc,ingress -n ${NAMESPACE}

echo ""
echo "--- Application Access ---"
INGRESS_IP=$(kubectl get ingress ${APP_NAME}-ingress -n ${NAMESPACE} -o jsonpath='{.status.loadBalancer.ingress[0].ip}' 2>/dev/null || echo "pending")
if [ "$INGRESS_IP" != "pending" ] && [ -n "$INGRESS_IP" ]; then
  echo "Application URL: http://${INGRESS_IP}/"
  echo "Health Check   : http://${INGRESS_IP}/health"
else
  echo "Ingress IP is still being assigned. Run the following to check:"
  echo "  kubectl get ingress ${APP_NAME}-ingress -n ${NAMESPACE}"
fi

echo ""
echo "--- Rollback Instructions ---"
echo "If the deployment fails, run:"
echo "  kubectl rollout undo deployment/${APP_NAME} -n ${NAMESPACE}"
echo ""
echo "=============================================="
echo " Deployment completed successfully!"
echo " Namespace : ${NAMESPACE}"
echo " Image     : ${IMAGE_URI}"
echo "=============================================="

# Cleanup temp files
rm -f /tmp/tour-management-deployment.yaml /tmp/tour-management-service.yaml \
      /tmp/tour-management-ingress.yaml /tmp/tour-management-namespace.yaml
