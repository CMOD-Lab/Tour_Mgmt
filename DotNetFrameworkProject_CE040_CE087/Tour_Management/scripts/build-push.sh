#!/bin/bash
# =============================================================
# build-push.sh - Build and Push Docker Image
# Application : Tour_Management (.NET Framework 4.7.2 Web Forms)
# Platform    : Linux/macOS
# Target      : Azure ACR or Docker Hub
# =============================================================
set -e

PROJECT_NAME="tour-management"

# Sanitize image name: lowercase, replace non-alphanumeric with hyphens, trim hyphens
IMAGE_NAME=$(echo "$PROJECT_NAME" | tr '[:upper:]' '[:lower:]' | tr -cs 'a-z0-9' '-' | sed 's/^-*//;s/-*$//')

echo "=============================================="
echo " Tour_Management - Docker Build & Push"
echo "=============================================="
echo ""

# Prompt for image tag
read -p "Enter image tag [latest]: " IMAGE_TAG
IMAGE_TAG=$(echo "${IMAGE_TAG:-latest}" | tr '[:upper:]' '[:lower:]' | tr -cs 'a-z0-9._-' '-' | sed 's/^-*//;s/-*$//')
if [ -z "$IMAGE_TAG" ]; then
  IMAGE_TAG="latest"
fi
echo "Using image tag: $IMAGE_TAG"
echo ""

# Registry selection
echo "Select container registry:"
echo "  1. Azure Container Registry (ACR)"
echo "  2. Docker Hub"
read -p "Enter choice [1]: " REGISTRY_CHOICE
REGISTRY_CHOICE="${REGISTRY_CHOICE:-1}"

if [ "$REGISTRY_CHOICE" = "1" ]; then
  # ---- Azure Container Registry ----
  echo ""
  echo "--- Azure Container Registry ---"
  read -p "Enter ACR name (e.g. myregistry): " ACR_NAME
  if [ -z "$ACR_NAME" ]; then
    echo "ERROR: ACR name cannot be empty."
    exit 1
  fi
  ACR_NAME=$(echo "$ACR_NAME" | tr '[:upper:]' '[:lower:]')
  REGISTRY="${ACR_NAME}.azurecr.io"
  FULL_IMAGE_NAME="${REGISTRY}/${IMAGE_NAME}:${IMAGE_TAG}"

  echo ""
  echo "Logging in to Azure Container Registry: $REGISTRY"
  az acr login --name "$ACR_NAME"

elif [ "$REGISTRY_CHOICE" = "2" ]; then
  # ---- Docker Hub ----
  echo ""
  echo "--- Docker Hub ---"
  read -p "Enter Docker Hub username: " DOCKER_USERNAME
  if [ -z "$DOCKER_USERNAME" ]; then
    echo "ERROR: Docker Hub username cannot be empty."
    exit 1
  fi
  read -s -p "Enter Docker Hub password/token: " DOCKER_PASSWORD
  echo ""
  if [ -z "$DOCKER_PASSWORD" ]; then
    echo "ERROR: Docker Hub password cannot be empty."
    exit 1
  fi
  REGISTRY="docker.io"
  FULL_IMAGE_NAME="${DOCKER_USERNAME}/${IMAGE_NAME}:${IMAGE_TAG}"

  echo ""
  echo "Logging in to Docker Hub..."
  echo "$DOCKER_PASSWORD" | docker login --username "$DOCKER_USERNAME" --password-stdin

else
  echo "ERROR: Invalid registry choice. Please enter 1 or 2."
  exit 1
fi

echo ""
echo "Building Docker image: $FULL_IMAGE_NAME"
echo "Build context: repository root (.)"
echo "Dockerfile: DotNetFrameworkProject_CE040_CE087/Tour_Management/Dockerfile"
echo ""

# Build from repository root so all project files are in context
docker build \
  -f DotNetFrameworkProject_CE040_CE087/Tour_Management/Dockerfile \
  -t "$FULL_IMAGE_NAME" \
  .

echo ""
echo "Successfully built: $FULL_IMAGE_NAME"
echo ""
echo "Pushing image to registry..."
docker push "$FULL_IMAGE_NAME"

echo ""
echo "=============================================="
echo " Build and push completed successfully!"
echo " Image: $FULL_IMAGE_NAME"
echo "=============================================="
