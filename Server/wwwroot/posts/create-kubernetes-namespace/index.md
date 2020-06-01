---
title: Create a Kubernetes namespace
slug: create-kubernetes-namespace
author: Brady
lastModified: 2020-05-28 08:54:07
pubDate: 2020-05-28 08:54:07
categories: Create a Worker
description: In this step you'll use Visual Studio Code's Kubernetes tools to create a new namespace in the AKS cluster.
phase: 2
step: 5
---

A namespace in a cluster is a logical grouping of services within the cluster. Think of a namespace in a cluster as similar to a resource group in Azure - it logically groups together a collection of microservices.

Expand the namespaces node using the Visual Studio Code tools for Kubernetes. Click the `default` namespace. You'll see the YAML representation of the `default` namespace open in Visual Studio Code.

![Open the default namespace](media/open-default-namespace.png)

Delete these items from the YAML file open in Visual Studio Code:

1. `creationTimestamp`
1. `resourceVersion`
1. `selfLink`
1. `uid`

Then, change the `default` namespace in the file to be `workshop`.

![New namespace file](media/create-workshop-namespace.png)

Use the Visual Studio Code command palette to find the **Kubernete:Apply** command by searching for `apply` in the palette.

![Apply command](media/select-apply.png)

Click the **Save** button to save the `.yml` file, then click the **Apply** button to apply the changes to the cluster.

![Save and create](media/save-and-create.png)

Once the apply command completes and you refresh the Kubernetes cluster explorer panel, you should see the new `workshop` namespace in the tree view.

![Use the new namespace](media/use-workshop-namespace.png)

Right-click the `workshop` namespace and select the **Use namespace** option to set the `workshop` namespace as the default, selected namespace for the rest of the workshop.