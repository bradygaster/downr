---
title: Development machine setup
slug: machine-setup
author: Brady
lastModified: 2020-05-25 08:46:36
pubDate: 2020-05-25 08:46:36
categories: Setup
description: In this step you'll install some things on your machine that will make developing with .NET Core, Azure, and Kubernetes easier.
phase: 1
step: 2
---

You'll need to make sure you have the following things installed on your machine. If you're on Windows, you'll want to install [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) , with the web and cloud workloads installed.

1. [.NET Core 3.1 runtime and SDK](https://dotnet.microsoft.com/download)
1. [Docker Desktop](https://www.docker.com/get-started) - when asked, opt to install Kubernetes.
1. [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) - you'll need to use the Azure CLI to manage Azure resources.
1. [Visual Studio Code](https://code.visualstudio.com/), and then install these Visual Studio Code extensions:
    1. [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
    1. [Docker](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
    1. [Kubernetes](https://marketplace.visualstudio.com/items?itemName=ms-kubernetes-tools.vscode-kubernetes-tools) - this extension will enable you to manage K8S clusters within Visual Studio Code.
    1. [Kubernetes Support](https://marketplace.visualstudio.com/items?itemName=ipedrazas.kubernetes-snippets) - this extension adds some handy Kubernetes HELM YAML snippets to Visual Studio Code.
    1. [Azure Kubernetes Service](https://marketplace.visualstudio.com/items?itemName=ms-kubernetes-tools.vscode-aks-tools) - this extension makes it easy to add instances of AKS clusters to Visual Studio Code's Kubernetes extension cluster list.
    1. [Azure CLI Tools](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azurecli) - will make it easier to write AZ CLI scripts with colorization and intellisense.
    1. [Azure App Service](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice) - we may use this to deploy a container as an App Service.
    1. [Azure Storage](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurestorage) - we may use Storage to store some blobs or configuration
1. [Helm](https://helm.sh/docs/intro/install/) - there are instructions for each OS on the Helm site
