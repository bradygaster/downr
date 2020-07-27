---
title: Set up .NET Interactive kernel for Jupuyter Notebooks
slug: 8.2-set-up-jupyter
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: DataFrame and Jupyter
description: Install the .NET kernels and create a .NET Notebook.
phase: 8
step: 3
---

## Install Python and Jupyter
Using Jupyter notebooks requires you to have Python and Jupyter installed. To get both, download [Anaconda](https://www.anaconda.com/products/individual). It is recommended to get the latest download of version 3. Version 2.7 of Python will no longer have any updates to it.

![Download Anaconda](./media/anaconda-download.png)

## Install .NET Interactive
Once you have Anaconda installed, in your terminal run the below command to install .NET Interactive.

```bash
dotnet tool install -g --add-source "https://dotnet.myget.org/F/dotnet-try/api/v3/index.json" Microsoft.dotnet-interactive
```

You should get a success message once it got installed.

![Install .NET Interactive](./media/interactive-install.png)

## Install the .NET Jupyter Kernels
With the .NET Interactive installed, you also need to install the Jupyter Kernels for it. In your terminal, run the below command to install the kernels.

```bash
dotnet interactive jupyter install
```

![Installing .NET Kernels](./media/kernels-install.png)

Once completed, you can run the `jupyter kernelspec list` command to get a list of the installed kernels and make sure the .NET kernels show in the output.

![List Kernels](./media/kernel-list.png)

## Create .NET Notebook
With everything installed, we can now create a C# notebook in Jupyter. 

Search for the program "Anaconda Prompt" and open it. In that terminal, type `jupyter notebook` and wait for the browser tab to load.

> NOTE: You can also use the `jupyter lab` command for this, if you prefer.

Once Jupyter loads, click the "New" dropdown and select ".NET (C#)" to create a new C# notebook.

![New C# Notebook](./media/new-notebook.png)

A new notebook will be created and the kernel should say that it's using C#.

![Empty Notebook](./media/empty-notebook.png)

## Test C# Kernel
To make sure the notebook runs C#, we can give it some C# code to test it out.

Type the below code in the first cell and click the Run button in the top menu.

![Run Menu Button](./media/run-button.png)

```csharp
Console.WriteLine("Hello C#")
```

> NOTE: You can also use the `CTRL+Enter` keyboard shortcut to run the active cell.

![Run Notebook Cell](./media/run-cell.png)

Congratulations! You have now run C# inside a Jupyter notebook!