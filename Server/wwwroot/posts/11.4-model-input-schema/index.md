---
title: Define the model input schema
slug: 11.4-model-input-schema
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: ONNX
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 11
step: 4
---

In this section you install the required NuGet packages and define the model input schema.

First, we need to add a few NuGet packages to the `ONNXConsole` project.

If you're using Visual Studio, right click on the project name and select **Manage NuGet Dependencies**. Then click the "Browse" tab and search for `Microsoft.ML.OnnxTransformer`. Make sure to install version **1.5.1**.

![Install Microsoft.ML.OnnxTransformer NuGet package](./media/onnxtransformer.png)

Repeat these steps for the `Microsoft.ML.OnnxRuntime` version `1.4.0` and `Microsoft.ML.ImageAnalytics` version `1.5.1`.

Alternately if you prefer working from the command line, you can run this command from the *src/ONNXConsole* folder:

```dotnetcli
dotnet add package Microsoft.ML.OnnxTransformer -v 1.5.1
dotnet add package Microsoft.ML.OnnxRuntime -v 1.4.0
dotnet add package Microsoft.ML.ImageAnalytics -v 1.5.1
```

### Define model input schema

In the `Shared` project, add a new class called `ONNXInput` to the root directory.

Then, define the class as follows:

```csharp
public class ONNXInput
{
    public string ImagePath { get; set; }
}
```

![Define ONNX input schema](./media/onnx-model-input.png)