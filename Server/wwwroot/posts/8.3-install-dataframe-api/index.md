---
title: Install and reference the DataFrame API in a Notebook
slug: 8.3-install-dataframe-api
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: DataFrame and Jupyter
description: Add the DataFrame NuGet package to your C# Notebook.
phase: 8
step: 4
---

The DataFrame API is a [NuGet package](https://www.nuget.org/packages/Microsoft.Data.Analysis/) that you can download and start using. To install it in a C# Jupyter notebook, use the below command:

```bash
#r "nuget:Microsoft.Data.Analysis"
```

The `#r` command tells the kernel that you want to reference a file or, in this case, a NuGet package. The "nuget" prefix in the string tells it that you want to reference a package from NuGet. After the colon is where you specify the full name of the package you want to reference.


## Reference the DataFrame and Plotting APIs

In another cell, use the below code to reference the DataFrame and plotting APIs. This allows access to the classes and methods from those libraries into the notebook.

```csharp
using Microsoft.Data.Analysis;
using XPlot.Plotly;
```