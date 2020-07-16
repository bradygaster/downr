---
title: Install ML.NET NuGet
slug: 1.3-install-nuget
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Get started
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 1
step: 3
---

In this session, you'll add install the Microsoft.ML NuGet package.

## Install the ML.NET Nuget Package

First, we need to add the ML.NET NuGet package to the `Shared` project. If you're using Visual Studio, right click on the project name and select **Manage NuGet Dependencies**. Then click the "Browse" tab and search for `Microsoft.ML`. Make sure to install version **1.5.0**.

![Install Microsoft.ML NuGet package](./media/install-microsoft-ml-nuget.png)

Alternately if you prefer working from the command line, you can run this command from the *src/TrainConsole* folder:

```powershell
dotnet add package Microsoft.ML -v 1.5.0
```