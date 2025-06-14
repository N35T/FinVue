#!/bin/bash
set -e

# Required environment variables:
# REGISTRY_URL - registry host (e.g. docker.io/yourusername or registry.example.com)
# REGISTRY_USERNAME - registry login username
# REGISTRY_PASSWORD - registry login password
# VERSION - this version

if [[ -z "$REGISTRY_URL" || -z "$REGISTRY_USERNAME" || -z "$REGISTRY_PASSWORD" || -z "$VERSION" ]]; then
  echo "ERROR: REGISTRY_URL, REGISTRY_USERNAME, REGISTRY_PASSWORD and VERSION environment variables must be set."
  exit 1
fi

echo "Starting publish for version $VERSION... "

BACKEND_IMAGE="$REGISTRY_URL/finvue-backend"
FRONTEND_IMAGE="$REGISTRY_URL/finvue-frontend"

echo "Logging into container registry $REGISTRY_URL..."
echo $REGISTRY_PASSWORD | docker login -u "$REGISTRY_USERNAME" --password-stdin

echo "Building backend image... "
docker build -t $BACKEND_IMAGE:latest -t $BACKEND_IMAGE:$VERSION -f ../FinVue.Api/FinVue.Api/Dockerfile ../FinVue.Api

echo "Building frontend image... "
docker build -t $FRONTEND_IMAGE:latest -t $FRONTEND_IMAGE:$VERSION -f ../FinVue.UI/Dockerfile ../FinVue.UI

echo "Pushing artefacts... "
docker push $BACKEND_IMAGE
docker push $FRONTEND_IMAGE

echo "Build and push completed successfully for version $VERSION."