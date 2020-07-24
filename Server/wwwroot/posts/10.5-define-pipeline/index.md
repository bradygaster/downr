---
title: Define the pipeline
slug: 10.5-define-pipeline
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Deep learning training
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 10
step: 5
---

In this step, you will define a training pipeline for a deep learning image classification model using the ML.NET [ImageClassification API](https://docs.microsoft.com/dotnet/api/microsoft.ml.visioncatalog.imageclassification?view=ml-dotnet#Microsoft_ML_VisionCatalog_ImageClassification_Microsoft_ML_MulticlassClassificationCatalog_MulticlassClassificationTrainers_System_String_System_String_System_String_System_String_Microsoft_ML_IDataView_)

Once your data is loaded, it's time to define the set of transforms to train your model.

Start off by creating a data loading pipeline which takes the image path and creates a `Bitmap`. Below the `validationImagesDV` variable, add the following code:

```csharp
var dataLoadPipeline = mlContext.Transforms.LoadRawImageBytes(outputColumnName:"ImageBytes", imageFolder:null, inputColumnName:"ImagePath");
```

To train an image classification model, there are several parameters that you may want to set. To make it easier to manage, create an instance of [`ImageClassificationTrainer.Options`](https://docs.microsoft.com/dotnet/api/microsoft.ml.vision.imageclassificationtrainer.options?view=ml-dotnet).

```csharp
// Set the options for the image classification trainer
var trainerOptions = new ImageClassificationTrainer.Options
{
    FeatureColumnName = "ImageBytes",
    LabelColumnName = "EncodedLabel",
    WorkspacePath = "workspace",
    Arch = ImageClassificationTrainer.Architecture.InceptionV3,
    ReuseTrainSetBottleneckCachedValues=true,
    MetricsCallback = (metrics) => Console.WriteLine(metrics.ToString())
};
```

Some parameters to note:

- *WorkspacePath*: The path where image bottleneck cache values and trained model are saved.
- *Arch*: The model architecture to use. In this case, the architecture used is [Inception v3](https://en.wikipedia.org/wiki/Inceptionv3). ML.NET supports several [image classification model architectures](https://docs.microsoft.com/dotnet/api/microsoft.ml.vision.imageclassificationtrainer.architecture?view=ml-dotnet).
- *ReuseTrainSetBottleneckCachedValues*: When set to `true`, cached bottleneck values are used if already available in the workspace folder.
- *MetricsCallback*: Callback to report statistics on accuracy/cross entropy during training phase.

Once you have defined your options, use them to create your training pipeline. Note that the DamageClass is encoded as a [`KeyDataViewType`](https://docs.microsoft.com/dotnet/microsoft.ml.data.keydataviewtype?view=ml-dotnet) before being passed into the model by using the [`MapValueToKey`](https://docs.microsoft.com/dotnet/api/microsoft.ml.conversionsextensionscatalog.mapvaluetokey?view=ml-dotnet) transform. Once the model classifies the image, the predicted category is decoded back to the original `DamageClass` value using the [`MapKeyToValue`](https://docs.microsoft.com/dotnet/api/microsoft.ml.conversionsextensionscatalog.mapkeytovalue?view=ml-dotnet#Microsoft_ML_ConversionsExtensionsCatalog_MapKeyToValue_Microsoft_ML_TransformsCatalog_ConversionTransforms_System_String_System_String_) transform.

```csharp
// Define training pipeline
var trainingPipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "EncodedLabel", inputColumnName: "DamageClass")
    .Append(mlContext.MulticlassClassification.Trainers.ImageClassification(trainerOptions))
    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));
```

Finally, combine the data loading pipeline and training pipeline.

```csharp
var trainer = dataLoadPipeline.Append(trainingPipeline);
```

![Define image classification training pipeline](https://user-images.githubusercontent.com/46974588/88376609-bfb92c80-cd6b-11ea-8123-0f309e384413.png)