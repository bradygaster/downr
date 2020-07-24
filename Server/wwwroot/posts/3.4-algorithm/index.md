---
title: Add an algorithm to the pipeline
slug: 3.4-algorithm
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Training
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 3
step: 4
---

Next, we need to choose an algorithm and add it to the pipeline. Add the following code after the code we just added to set up the transformation pipeline.

```csharp
// Choose an algorithm and add to the pipeline
var trainer = mlContext.Regression.Trainers.LbfgsPoissonRegression();
var trainingPipeline = dataProcessPipeline.Append(trainer);
```

This code sets up an instance of the trainer using a linear regression model, `LbfgsPoissonRegression`. Learn about the different [algorithms](https://docs.microsoft.com/dotnet/machine-learning/how-to-choose-an-ml-net-algorithm) in ML.NET.