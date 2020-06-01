---
title: Connect Visual Studio Code to AKS
slug: connect-vs-code
author: Brady
lastModified: 2020-05-26 09:30:32
pubDate: 2020-05-26 09:30:32
categories: Setup
description: This final setup step will connect the Kubernetes tools for Visual Studio Code to your new AKS cluster.
phase: 1
step: 7
---

The final step in the setup process is to get Visual Studio Code connected directly to the AKS cluster. The Visual Studio Code tools for Kubernetes support **any** Kubernetes cluster on **most** clouds, so the step of connecting a Visual Studio Code to a Kubernetes cluster is specific and not associated with your Azure login.

Click the Kubernetes logo in the Visual Studio Code command bar.

![The Kubernetes tools icon](media/click-kubernetes-tools.png)

We won't be using the Helm tools for this exercise, so you can click the panel to hide this segment of the Kubernetes tool window.

![Hide the Helm repos window](media/click-helm-repos.png)

Expand the Azure node, then right-click the AKS cluster you created earlier and select the **Merge into Kubeconfig** menu option.

![Merge into Kubernetes config](media/right-click-cluster.png)

The cluster will appear in the list of clusters. Right-click the cluster and make it the current cluster.

![Set as current cluster](media/set-as-current-cluster.png)

Once you've set the AKS cluster you created as the current cluster, you should be able to see all of the services and configuration currently in the cluster within Visual Studio Code.

![Current cluster objects](media/current-cluster.png)

---

Now that your development environment is all set up and you're connected to your cluster, you can take a break. Next, you'll deploy your first C# Worker Service into AKS.
