---
title: Create the Azure Container Registry
slug: create-registry
author: Brady
lastModified: 2020-05-26 01:33:56
pubDate: 2020-05-26 01:33:56
categories: Setup
description: The container registry will be used to house your container images. Other resources, like App Service, Azure Container Instances, and AKS all have access to Azure Container Registry repositories.
phase: 1
step: 5
---

On this step you'll create the Azure Container Registry (ACR) instance in which your container images will be stored. Once we start integrating the Visual Studio and Visual Studio Code tools with ACR, you'll begin to see how it's the "first step" on the way for your code during the deployment phase.

Check to see if you have any existing registries in your subscription by running the `az acr list` AZ CLI command. Using the `-o table` switch makes the list a bit easier to digest visually. You'll see a list of the registries in your subscription in the terminal output if you have any registries in your subscription.

```bash
az acr list -o table
```

Next, use the `az acr create` command to create a new Azure Container Registry instance in your subscription. You'll want to replace the `--name` and `--resource-group` parameter values with values matching your own environment.

> Note: You'll want to make sure you use the same resource group as the one you used when you created the AKS cluster.

```bash
az acr create --name cloudnativedotnetregistry --resource-group dotnetworkshops --sku Standard
```

The Azure CLI will dump the JSON response of the creation call to the terminal window. It will look something like the following example JSON:

```json
{
  "adminUserEnabled": false,
  "creationDate": "2020-05-26T19:45:39.775938+00:00",
  "dataEndpointEnabled": false,
  "dataEndpointHostNames": [],
  "encryption": {
    "keyVaultProperties": null,
    "status": "disabled"
  },
  "id": "/subscriptions/{GUID}/resourceGroups/dotnetworkshops/providers/Microsoft.ContainerRegistry/registries/cloudnativedotnetregistry",
  "identity": null,
  "location": "westus",
  "loginServer": "cloudnativedotnetregistry.azurecr.io",
  "name": "cloudnativedotnetregistry",
  "networkRuleSet": null,
  "policies": {
    "quarantinePolicy": {
      "status": "disabled"
    },
    "retentionPolicy": {
      "days": 7,
      "lastUpdatedTime": "2020-05-26T19:45:40.837969+00:00",
      "status": "disabled"
    },
    "trustPolicy": {
      "status": "disabled",
      "type": "Notary"
    }
  },
  "privateEndpointConnections": [],
  "provisioningState": "Succeeded",
  "resourceGroup": "dotnetworkshops",
  "sku": {
    "name": "Standard",
    "tier": "Standard"
  },
  "status": null,
  "storageAccount": null,
  "tags": {},
  "type": "Microsoft.ContainerRegistry/registries"
}
```

What you want to see is the `provisioningState` value as "Succeeded":

```json
...

"provisioningState": "Succeeded",
  "resourceGroup": "dotnetworkshops",
  "sku": {
    "name": "Standard",
    "tier": "Standard"
  },

...
```

Once you see this, go on to the next step to set up a trust relationship between the cluster and the registry so deployments will be automatic.