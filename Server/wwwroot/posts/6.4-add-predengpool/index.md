---
title: Add the PredictionEnginePool to Index PageModel
slug: 6.4-add-predengpool
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Consume model
description: Add the PredictionEnginePool to your project.
phase: 6
step: 5
---

Open the *Pages/Index.cshtml.cs* file and add the following `using` statements at the top to reference the *Shared* project and `Microsoft.Extensions.ML` NuGet package:

```csharp
using Microsoft.Extensions.ML;
using Shared;
```

Inside the class, define a private readonly variable for the `PredictionEnginePool` service.

```csharp
private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;
```

Then, inject the `PredictionEnginePool` service into the `Index` constructor.

```csharp
public IndexModel(ILogger<IndexModel> logger, ICarModelService carFileModelService, PredictionEnginePool<ModelInput,ModelOutput> predictionEnginePool)
{
    _logger = logger;
    _carModelService = carFileModelService.GetDetails();
    CarMakeSL = new SelectList(_carModelService, "Id", "Model", default, "Make");
    _predictionEnginePool = predictionEnginePool;
}
```

Finally, replace the implementation of the `OnPost` method with the following.

```csharp
public void OnPost()
{
    var selectedMakeModel = _carModelService.Where(x => CarModelDetailId == x.Id).FirstOrDefault();

    CarInfo.Make = selectedMakeModel.Make;
    CarInfo.Model = selectedMakeModel.Model;

    ModelInput input = new ModelInput
    {
        Year = (float)CarInfo.Year,
        Mileage = (float)CarInfo.Mileage,
        Make = CarInfo.Make,
        Model = CarInfo.Model
    };

    ModelOutput prediction = _predictionEnginePool.Predict(input);
    CarInfo.Price = prediction.Score;
    ShowPrice = true;
}
```

In the snippet above, the information from the `CarInfo` model is taken and a new instance of `ModelInput` is created. Then, the `Predict` function is used to predict the price of a vehicle, given the inputs from the `input` variable. Once a prediction is made, the `Price` property of the `CarInfo` is set to the predicted value and the price is displayed on screen.