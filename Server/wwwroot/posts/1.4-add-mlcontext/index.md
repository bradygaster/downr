---
title: Add ML Context
slug: 1.4-add-mlcontext
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Get started
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 1
step: 4
---

In this section, you'll add an `MLContext` to the `TrainConsole` project. `MLContext` is the starting point for all ML.NET operations. It provides a way to create components for:

- Data preparation
- Feature engineering
- Training
- Prediction
- Model evaluation
- Logging
- Execution control
- Seeding

## Initialize MLContext

Open the `Program.cs` file in the `TrainConsole` project and add the following `using` statement at the top of the file to reference the `Microsoft.ML` package.

```csharp
using Microsoft.ML;
```

Next, create an instance of `MLContext` in the `Main` method (replace the "Hello World" line):

```csharp
static void Main(string[] args)
{
    MLContext mlContext = new MLContext();
    //...
}
```

![Add MLContext](./media/add-ml-context.png)

At this point we're not yet ready to work with the `MLContext`, but you should be able to successfully build the application once more.