---
title: Install NuGet packages
slug: 10.2-install-nuget-packages
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Deep learning training
description: Add the ML.NET Vision NuGet package to your project.
phase: 10
step: 3
---

First, we need to add a few NuGet packages to the `ImageTrainConsole` project.

If you're using Visual Studio, right click on the project name and select **Manage NuGet Dependencies**. Then click the "Browse" tab and search for `Microsoft.ML.Vision`. Make sure to install version **1.5.1**.

![Install Microsoft.ML.Vision NuGet package](./media/install-dl-nuget.png)

Repeat these steps for the `Microsoft.ML.ImageAnalytics` version `1.5.1` and `SciSharp.TensorFlow.Redist` version `2.2.0.2`.

Alternately if you prefer working from the command line, you can run this command from the *src/ImageTrainConsole* folder:

```dotnetcli
dotnet add package Microsoft.ML.Vision -v 1.5.1
dotnet add package Microsoft.ML.ImageAnalytics -v 1.5.1
dotnet add package SciSharp.TensorFlow.Redist -v 2.2.0.2
```