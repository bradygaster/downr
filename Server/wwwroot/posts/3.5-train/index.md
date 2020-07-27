---
title: Train your model
slug: 3.5-train
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Training
description: Call the Fit method to execute the data transforms and algorithm to train your model.
phase: 3
step: 6
---

We train the model by calling the `Fit` method on the pipeline we've set up, passing in the data as an `IDataView` instance.

Add the following code to *Program.cs* after setting up the `trainingPipeline`:

```csharp
// Train the model
Console.WriteLine("Training model...");
var model = trainingPipeline.Fit(trainTestSplit.TrainSet);
```

![Train the model](./media/train-model.png)

Once we've set up our model, the next step is to test it and see how it performs.