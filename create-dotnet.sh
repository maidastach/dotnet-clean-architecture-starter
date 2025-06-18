#!/bin/bash

# ************************************* create-dotnet.sh ************************************* #
# A script to create a .NET Web API project

# Pre-Requisites:
# 1.  Git
# 2.  dotnet sdk
# 3.  aspnetcore runtime

# ************************************************************************************* #
# ************************************* Functions ************************************* #
# ************************************************************************************* #

# Function to display usage instructions
usage() {
    echo "Usage: create-dotnet.sh <solution-name> [target-directory]"
    echo "  solution-name     : The name of the solution to create."
    echo "  target-directory  : Optional. Directory where the solution will be created (defaults to current directory)."
    exit 1
}

# Function to validate the Target directory
validateInputDir() {
  # Ensure the target directory exists
  if [ ! -d "$TARGET_DIR" ]; then
      echo "Error: Target directory '$TARGET_DIR' does not exist."
      exit 1
  fi

  # Check if the solution directory already exists
  if [ -d "$SOLUTION_PATH" ]; then
      echo "Error: Solution directory '$SOLUTION_PATH' already exists."
      exit 1
  fi

  # Create the solution directory
  mkdir -p "$SOLUTION_PATH"
  cd "$SOLUTION_PATH"

  # Print where the solution will be created
  echo "Solution '$SOLUTION_NAME' will be created in '$TARGET_DIR'"
}

# Function to create sln and projects
createFullSolution() {
  # Create the Solution
  dotnet new sln -n $SOLUTION_NAME

  # Create the projects
  dotnet new webapi -n "${SOLUTION_NAME}.WebApi"
  dotnet new classlib -n "${SOLUTION_NAME}.Application"
  dotnet new classlib -n "${SOLUTION_NAME}.Domain"
  dotnet new classlib -n "${SOLUTION_NAME}.Infrastructure"

  # Add projects to solution
  dotnet sln add **/*.csproj
}

# Function to create references between projects
addProjectReferences() {
  dotnet add "${SOLUTION_NAME}.WebApi/${SOLUTION_NAME}.WebApi.csproj" reference "${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj"
  dotnet add "${SOLUTION_NAME}.WebApi/${SOLUTION_NAME}.WebApi.csproj" reference "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj"
  dotnet add "${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj" reference "${SOLUTION_NAME}.Domain/${SOLUTION_NAME}.Domain.csproj"
  dotnet add "${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj" reference "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj"
  dotnet add "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj" reference "${SOLUTION_NAME}.Domain/${SOLUTION_NAME}.Domain.csproj"
}

# Function to create Folder structure
createFolderStructure() {
  rm -f **/Class1.cs
  # rm -f ${SOLUTION_NAME}.WebApi.http
  cp -r $BASE_FILES_PATH/WebApi/* ./${SOLUTION_NAME}.WebApi
  cp -r $BASE_FILES_PATH/Application/* ./${SOLUTION_NAME}.Application
  cp -r $BASE_FILES_PATH/Domain/* ./${SOLUTION_NAME}.Domain
  cp -r $BASE_FILES_PATH/Infrastructure/* ./${SOLUTION_NAME}.Infrastructure
}

# Funtion to update placeholders on namespaces
updateNameSpaces() {
  TARGET_WORD="{ SolutionName }"
  find "$SOLUTION_PATH" -type f | while read -r FILE; do
    sed -i "s/${TARGET_WORD}/${SOLUTION_NAME}/g" "$FILE"
  done
}

# Function to add Nuget packages
installPackages() {
  dotnet add "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj" package Microsoft.Extensions.Caching.Memory
  dotnet add "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj" package Microsoft.Extensions.DependencyInjection
  dotnet add "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj" package Microsoft.Extensions.Configuration.Abstractions
  dotnet add "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj" package Dapper
  dotnet add "${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj" package Npgsql
}

# Funtion to init Git repository
initGit() {
  dotnet new gitignore
  git init
  git add .
  git commit -m "Initial"
}

# ************************************************************************************* #
# ************************************** Script *************************************** #
# ************************************************************************************* #

# Check for required parameters
if [ -z "$1" ]; then
    echo "Error: Solution name is required."
    usage
fi

# Variables
SOLUTION_NAME=$1
TARGET_DIR=${2:-$(pwd)} # If no target directory is provided, use the current working directory
SOLUTION_PATH="$TARGET_DIR/$SOLUTION_NAME"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BASE_FILES_PATH="$SCRIPT_DIR/base_files"

# Steps
# 1
  validateInputDir
# 2
  createFullSolution
# 3
  addProjectReferences
# 4
  createFolderStructure
# 5
  updateNameSpaces
# 6
  installPackages
# 7
  initGit

# Print out the output
echo "Structure:"
tree -L 2
echo "Solution and projects created successfully"





# Create folder structure
# cd "$BASE_PATH/$SOLUTION_NAME/${SOLUTION_NAME}.Api"
# mkdir Controllers DependencyInjection Extensions Middleware

# cd "$BASE_PATH/$SOLUTION_NAME/${SOLUTION_NAME}.Application"
# mkdir DependencyInjection Services Interfaces

# cd "$BASE_PATH/$SOLUTION_NAME/${SOLUTION_NAME}.Domain"
# mkdir Abstractions Configs DependencyInjection Extensions Entities

# cd "$BASE_PATH/$SOLUTION_NAME/${SOLUTION_NAME}.Infrastructure"
# mkdir DependencyInjection


# cd "$BASE_PATH/$SOLUTION_NAME/${SOLUTION_NAME}.Repository"
# mkdir DependencyInjection Configurations Extensions Migrations Repositories
# cp -r $BASE_FILES_PATH/Repository/* .
