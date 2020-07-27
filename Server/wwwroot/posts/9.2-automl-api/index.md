---
title: AutoML.NET API
slug:  9.2-automl-api
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: AutoML
description: Use the AutoML.NET API to create a car price prediction model.
phase: 9
step: 3
---

The AutoML.NET API implements local AutoML for ML.NET.

To start using the AutoML API, in Visual Studio create a new .NET Core console application. This lab will use the "true_car_listings.csv" dataset.

Install the "Microsoft.ML" and "Microsoft.ML.AutoML" NuGet packages.

![AutoML.NET NuGet Package](./media/automl-nuget.png)

Drag and drop the "true_car_listings.csv" dataset from the workshop's data folder into the solution. Right click the dataset in the solution and go to Properties. Set the "Copy to Output" setting to "Copy if newer".

![Copy if Newer Property](./media/copy-if-newer.png)

In the "Program.cs" file, replace the Console.WriteLine line with the below line to create a new ML Context.

```csharp
var context = new MLContext();
```

Next, load the data using the "context.Data.LoadFromTextFile" with the below code. Note that we set the header to "true" and set the separator character to be a comma.

```csharp
var data = context.Data.LoadFromTextFile<CarData>("./true_car_listings.csv", hasHeader: true, separatorChar: ',');
```

This will cause an error since the "CarData" class hasn't been created yet. Right click on the project and click on Add -> Class and rename the class to CarData.cs.

Use the below code to create the "CarData" class.

```csharp
public class CarData
{
    [LoadColumn(0)] 
    public float Price { get; set; }
    [LoadColumn(1)]
    public float Year { get; set; }
    [LoadColumn(2)]
    public float Mileage { get; set; }
    [LoadColumn(3)]
    public string City { get; set; }
    [LoadColumn(4)]
    public string State { get; set; }
    [LoadColumn(5)] 
    public string Vin { get; set; }
    [LoadColumn(6)]
    public string Make { get; set; }
    [LoadColumn(7)]
    public string Model { get; set; }
}
```

Back in the "Program.cs" file, we can drop columns that we know aren't useful for creating our model. Use the below code to drop the Vin, State, and City columns.

```csharp
var dropColumnsTransform = context.Transforms.DropColumns("Vin", "State", "City");
```

Next, use the "Fit" and "Transform" methods on the transform with the loaded data to drop the columns.

```csharp
var newData = dropColumnsTransform.Fit(data).Transform(data);
```

AutoML experiments require settings. Use the below code to create an instance of RegressionExperimentSettings.

```csharp
var settings = new RegressionExperimentSettings
{
    MaxExperimentTimeInSeconds = 60
};

```

We're setting the maximum time in seconds to execute the experiment. The more time it has to go through different algorithms and hyperparameters the better your results can be. However, we're only giving it a minute so we can get quick results.

There are other settings we can set here, such as what metric to use to determine how well a model is or what trainers we want AutoML to exclusively use.

With the settings defined, we can create an AutoML experiment with the below code.

```csharp
var experiment = context.Auto().CreateRegressionExperiment(settings);
```

For especially long experiment times, it helps to see a progress of what AutoML has already gone through. We can add a progress handler to print out anything we need including the current trainer it went through and what its metrics were. Use the below code to create a progress handler.

```csharp
var progress = new Progress<RunDetail<RegressionMetrics>>(p =>
{
    if (p.ValidationMetrics != null)
    {
        Console.WriteLine($"Current Run - {p.TrainerName}. R^2 -                                                                 {p.ValidationMetrics.RSquared}. MAE - {p.ValidationMetrics.MeanAbsoluteError}");
    }
});
```

With the experiment created, use the code below to execute the experiment. This will take only a minute to run since we set the experiment time. This also uses the progress handler we created just above.

```csharp
var run = experiment.Execute(newData, labelColumnName: "Price", progressHandler: progress);
```

With the run we can get the best run's model.

```csharp
var bestModel = run.BestRun.Model;
```

Now that we have a model, we can use it to perform a test prediction. Create a prediction engine with the below code and use the "bestModel" as the parameter.

```csharp
var predictionEngine = context.Model.CreatePredictionEngine<CarData, CarPrediction>(bestModel);
```

The "CreatePredictionEngine" takes in two generic methods. First is the "CarData" as the input class, and it also takes an output class that we called "CarPrediction". Create a new class similar to what we did before and call it "CarPrediction". Then, use the below code to add the properties.

```csharp
[ColumnName("Score")]
public float PredictedPrice { get; set; }

```

Back in the "Program.cs" file, use the below code to create a new "CarData" instance with some test data.

```csharp
var carData = new CarData
{
    Model = "FusionS",
    Make = "Ford",
    Mileage = 61515f,
    Year = 2012f
};

```

With the test data, we can call the "Predict" method on the prediction engine to generate a prediction from our model.

```csharp
var prediction = predictionEngine.Predict(carData);
```

Then, use the below code to write out the prediction to the console and format it into currency.

```csharp
Console.WriteLine($"Prediction - {prediction.PredictedPrice:C}");
```

Run the project and while this is running you should see the progress output.

![AutoML.NET API Output](./media/automl-api-output.png)

And once it finishes, we can see the predicted price for the example data we gave it.

![AutoML.NET API Prediction](./media/automl-api-prediction.png)


Congratulations! You've built an ML.NET model using the AutoML.NET API.