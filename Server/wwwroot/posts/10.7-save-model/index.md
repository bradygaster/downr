---
title: Save the model
slug: 10.7-save-model
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Deep learning training
description: Save your image classification model for later use in other applications (desktop, mobile, web).
phase: 10
step: 8
---

Once you're satisfied with your model, save it to your PC.

Inside of the *Program* class definition of the `ImageTrainConsole` project, add the path where you want to save your model to. The model is serialized and stored as a `.zip` file. In this case, the model will be saved to a file called *ImageModel.zip*.

```csharp
private static string MODEL_FILEPATH = @"C:\Dev\ImageModel.zip";
```

Then, inside the `Main` method, add the following code at the bottom:

```csharp
//Save the model
mlContext.Model.Save(model, trainImagesDV.Schema, MODEL_FILEPATH);
Console.WriteLine("Saved image classification model");
```

![Save image classification model](./media/dl-save-model.png)

Set the startup project to `ImageTrainConsole` and run the application.

Congratulations! You have now trained a custom image classification model with ML.NET.

If you missed any parts, you can download the [completed app](https://github.com/luisquintanilla/mlnet-workshop-guide/archive/10.0.zip).

## Additional resources

- [Build an image classification model in under 10 minutes with Model Builder and Azure](https://www.youtube.com/watch?v=G_ZJZdKLNMc&)
- [How to train an image classification model with Model Builder](https://devblogs.microsoft.com/dotnet/train-image-classification-model-azure-mlnet-model-builder/)
- [Train a custom deep learning model to identify damaged infrastructure](https://docs.microsoft.com/dotnet/machine-learning/tutorials/image-classification-api-transfer-learning)
- [Detect objects using ONNX in ML.NET](https://docs.microsoft.com/dotnet/machine-learning/tutorials/object-detection-onnx)
- [Classify sentiment using TensorFlow](https://docs.microsoft.com/dotnet/machine-learning/tutorials/text-classification-tf)