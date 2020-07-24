---
title: Split your dataset into train and test sets
slug: 3.2-split-dataset
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Training
description: Split your dataset into a dataset for model training and a dataset for model evaluation.
phase: 3
step: 3
---

The goal of a machine learning model is to identify patterns within training data. These patterns are used to make predictions using new data.

Use the `TrainTestSplit` method to split the data into train and test sets. The result will be a `TrainTestData` object which contains two `IDataView` members, one for the train set and the other for the test set. The data split percentage is determined by the `testFraction` parameter. The snippet below is holding out 20 percent of the original data for the test set.

Add this to `Program.cs` in the TrainConsole project after the other code we've written so far:

```csharp
// Split the data into a train and test set
var trainTestSplit = mlContext.Data.TrainTestSplit(trainingData, testFraction: 0.2);
```