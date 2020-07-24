---
title: Cross validation (optional)
slug: 4.2-cross-val
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Evaluation
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 4
step: 2
---

Cross validation is a training and evaluation technique. It folds or splits the data into n-partitions and trains multiple models on these partitions. This helps improve the robustness by holding out data from the training process. Cross-validation may help with overfitting.

To train and evaluate a model with cross validation, use the `CrossValidate` method for the respective task.

Start by adding the following `using` statement to the top of the *Program.cs* file.

```csharp
using System.Linq;
```

```csharp
var crossValidationResults = mlContext.Regression.CrossValidate(trainingData, trainingPipeline, numberOfFolds: 5);
var avgRSquared = crossValidationResults.Select(model => model.Metrics.RSquared).Average();
Console.WriteLine($"Cross Validated R-Squared: {avgRSquared}");
```

When using cross validation, because the data is automatically partitioned for you, there's no need to split into train and test sets (although you can if you want to). In the previous example, the data is partitioned into 5 folds. The overall accuracy of the models is measured by looking at the average metrics (in this case R-Squared).

Set the startup project to *TrainConsole* and run the application. The result should look similar to the output below:

```text
Cross Validated R-Squared: 0.8736620547207405
```