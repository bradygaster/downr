---
title: Install ML.NET NuGet
slug: 1.3-install-nuget
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Get started
description: Add the ML.NET NuGet package to your project.
phase: 1
step: 4
---

First, we need to add the ML.NET NuGet package to the `Shared` project. If you're using Visual Studio, right click on the project name and select **Manage NuGet Packages**. Then click the "Browse" tab and search for `Microsoft.ML`. Make sure to install version **1.5.1**.

![Install Microsoft.ML NuGet package](./media/install-microsoft-ml-nuget.png)

Alternately if you prefer working from the command line, you can run this command from the *src/TrainConsole* folder:

```powershell
dotnet add package Microsoft.ML -v 1.5.1
```