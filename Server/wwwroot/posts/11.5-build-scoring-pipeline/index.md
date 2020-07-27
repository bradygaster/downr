---
title: Build and save the ML.NET ONNX scoring pipeline
slug: 11.5-build-scoring-pipeline
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: ONNX
description: Build and save an ML.NET scoring pipeline that uses an ONNX model.
phase: 11
step: 6
---

Now that you have the pipeline, you can create an ML.NET model that contains all of the preprocessing and scoring transforms. 

Since no training is actually taking place, you can use an empty `IDataView` when building the pipeline.

Below your pipeline definition, create an empty IDataView and call `Fit` to create the model.

```csharp
// Build the pipeline
var emptyDV = mlContext.Data.LoadFromEnumerable(new ONNXInput[] { });
var model = pipeline.Fit(emptyDV);
```

Now that you have a pipeline, save it for later use. The model is serialized and stored as a `.zip` file. In this case, the model will be saved to a file called *ONNXModel.zip*.

Inside the top of the class definition, set the path where you want to save your pipeline to.

```csharp
private static string PIPELINE_FILEPATH = @"C:\Dev\ONNXModel.zip";
```

Then, at the bottom of the `Main` method, add the following code:

```csharp
// Save the pipeline
Console.WriteLine("Saving pipeline...");
mlContext.Model.Save(model, emptyDV.Schema, PIPELINE_FILEPATH);
```

![Save ML.NET ONNX Scoring Pipeline](./media/build-scoring-pipeline.png)

Set the startup project to `ONNXConsole` and run the application.

Congratulations! You have now built and saved a prediction pipeline that uses an ONNX model.