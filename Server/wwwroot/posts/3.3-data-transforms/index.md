---
title: Add data transformations to the pipeline
slug: 3.3-data-transforms
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Training
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 3
step: 3
---

Most data can't be used as-is - we need to transform it before we can work with it. This is done by performing a series of transforms in succession as a pipeline.

Go back into *Program.cs* in the TrainConsole project and add the following code after declaring `trainingData`:

```csharp
// Create data transformation pipeline
var dataProcessPipeline =
    mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "MakeEncoded", inputColumnName: "Make")
        .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "ModelEncoded", inputColumnName: "Model"))
        .Append(mlContext.Transforms.Concatenate("Features", "Year", "Mileage", "MakeEncoded", "ModelEncoded"))
        .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
        .AppendCacheCheckpoint(mlContext);
```

The above code first encodes the Make and Model columns using `OneHotEncoding`. It then concatenates the encoded Year, Make, and Model, as well as Mileage, into a Features column. Finally, it normalizes the Features values using a `MinMax` transform that results in a linear range from 0 to 1, with the min value at 0 and the max at 1.

Finally, since ML.NET doesn't perform any caching automatically, the resulting values are cached in preparation for running the training. Caching can help improve training time since data doesn't have to continuously be loaded from disk. Keep in mind though, only cache when the dataset can fit into memory.