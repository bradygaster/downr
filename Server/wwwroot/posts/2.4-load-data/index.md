---
title: Load data from a file into an IDataView
slug: 02-data
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Data
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 2
step: 2
---

Now open up *Program.cs* file in the `TrainConsole` project (**not** the web project!) and add the following `using` statement:

```csharp
using Shared;
```

Inside of the class definition, add the location of the file, which will depend on where you cloned or downloaded this repo (it's in `data/true_car_listings.csv` in the repository's folder).:

```csharp
class Program
{
    // update this with your file's path where you saved it
    private static string TRAIN_DATA_FILEPATH = @"C:\Dev\mlnet-workshop\data\true_car_listings.csv";

    //...
}
```

Next, in the `Main` method, add this block of code after `mlContext`:

```csharp
// Load training data
Console.WriteLine("Loading data...");
IDataView trainingData = mlContext.Data.LoadFromTextFile<ModelInput>(path: TRAIN_DATA_FILEPATH, hasHeader: true, separatorChar: ',');
```

At this point, you have defined how to load data into an `IDataView` with a `ModelInput` schema. It's important to remember though that an `IDataView` is lazy and no loading takes place at this stage. We'll explore that in the following section. You're now ready to define the set of data transformations and algorithms used to train your machine learning model.