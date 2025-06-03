#!/bin/bash
set -e

echo "Waiting for MinIO to be ready at http://localhost:9000..."

until mcli alias set localminio http://localhost:9000 minioadmin minioadmin; do
  echo "Waiting for MinIO alias setup..."
  sleep 3
done

echo "Creating bucket 'socialcampus' (if not exists)..."
mcli mb --ignore-existing localminio/socialcampus

echo "Setting bucket policy to public read..."
mcli anonymous set download localminio/socialcampus

echo "Setup complete!"
