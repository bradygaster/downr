---
title: Consume the model
slug: 7.6-consume-model
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: MLOps
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 7
step: 6
---

If we imagine for a second that our CI/CD workflow also publishes our trained model to let's say a test container in an Azure Storage Account. We would then be able to consume that model when doing contract or exploratory testing in a test environment prior to a production deployment. For your convenience, we have published a model to an Azure Storage Account that you can consume as part of the `Web` application in this workshop.

To be able to consume your model from a URI, locate the `Startup.cs` class within the `Web` project and replace the following line:

```
   services.AddPredictionEnginePool<ModelInput, ModelOutput>().FromFile(@"C:\Dev\MLModel.zip");
```

with

```
  services.AddPredictionEnginePool<ModelInput, ModelOutput>().FromUri(@"https://ndcmelbourne.blob.core.windows.net/model/MLModel.zip");
```

To ensure that we are able to use the model now stored in the Azure Storage Account, set the startup project to Web and run the application. Fill in the form fields and select Predict Price.

![Consume the model in web app](./media/7-6-consume-model.png)

Congratulations! You have now mastered the art of MLOps.
