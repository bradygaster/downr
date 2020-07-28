---
title: Data and model tests
slug: 7.4-data-model-tests
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: MLOps
description: Create some automated tests.
phase: 7
step: 5
---

If you've made it this far, you've successfully set up a workflow that automatically trains your model on new commits.

However, as with any well-architected software application, automated tests are also required to ensure that the application works as expected. Thus, you can add tests to your model training workflow. 

There are two types of tests that you will learn about in this workshop:

1. **Data validation tests** to ensure the integrity of your training data 
2. **Model tests** to validate the quality of your trained model

### Data validation tests
To train a model, you use a dataset that consists of several features, such as price, the year the car was made, milage and so forth.

To ensure the quality of our model, it's important to validate that the data is sound. You may, for example, want to verify that the dataset does not contain any negative numbers or other invalid data points. 

There is a `DataValidationsTests.cs` test class in the `DataTests` project (located under the `Tests` folder in the solutions explorer). This test class contains several tests that will ultimately verify your dataset.

The first thing you need to do is set the correct path to your data (which will be located on the Azure FileShare as mentioned in previous steps).

Replace the `TRAIN_DATA_FILEPATH` variable located in `DataValidationTests` with:

```
  private static string TRAIN_DATA_FILEPATH = @"/media/data/true_car_listings.csv";
```

The next thing you need to do is implement the `Initialize` method. This method will be used to load the dataset and convert all rows into an enumerable of `ModelInput`.

Change the `Initialize` method to the following code (note the `Rows` private member variable that will be used in the tests):

```
        private static IEnumerable<ModelInput> Rows;
        
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromTextFile<ModelInput>(TRAIN_DATA_FILEPATH, hasHeader: true, separatorChar: ',');

            Rows = mlContext.Data.CreateEnumerable<ModelInput>(data, false);
        }
```

With the `Initialize` method set up, you can start implementing your tests. 

To verify a valid year range and that there are no negative prices in your dataset, you can implement `VerifyValidPrice()` and `VerifyValidYear()` as follows:

```
        [TestMethod]
        public void VerifyValidPrice()
        {
            var hasNegativePrice = Rows.Any(x => x.Price < 0);

            hasNegativePrice.Should().BeFalse();
        }

        [TestMethod]
        public void VerifyValidYear()
        {
            var hasValidYears = Rows.All(x => x.Year > 1950 && x.Year < DateTime.Now.Year + 1);

            hasValidYears.Should().BeTrue();
        }
```

Based on these tests, try to implement `VerifyValidMilage()` and `VerifyMinimumNumberOfRows()` yourself (assume that you need at least 10,000 rows).

The final two tests should look something like this:

```
        [TestMethod]
        public void VerifyValidMilage()
        {
            var hasInvalidMilage = Rows.Any(x => x.Mileage < 0);

            hasInvalidMilage.Should().BeFalse();
        }
        
        [TestMethod]
        public void VerifyMinimumNumberOfRows()
        {
            var rowCount = Rows.Count();

            rowCount.Should().BeGreaterThan(10000);
        }        
```

Commit your changes and push them to your fork using a tool of your choice, such as GitHub Desktop, Visual Studio, or the Git CLI.

The next step is to add your data tests to your CI workflow to make sure they are run before the model training.

Do do so, open up the `dotnet-core.yml` file under `.github/workflows` and add the following just prior to `Train` step:

```
    - name: Data Tests
      working-directory: 'test/DataTests'     
      run: dotnet test DataTests.csproj
```

Your complete `dotnet-core.yml` file should now look like: 

```
name: .NET Core

on:
  push:
    branches: [ master ]
  pull_request:
    branches: [ master ]
  
jobs:
  build:

    runs-on: ubuntu-latest

    steps:        
    - uses: actions/checkout@v2
    - name: Setup .NET Core
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 3.1.101   
    - name: 'Create mount points'
      run: 'sudo mkdir /media/data'
    - name: 'Map disk drive to Azure Files share folder'
      run: 'sudo mount -t cifs //ndcmelbourne.file.core.windows.net/data /media/data -o vers=3.0,username=ndcmelbourne,password=${{ secrets.STORAGEKEY }},dir_mode=0777,file_mode=0777'
    - name: Install dependencies
      run: dotnet restore src/MLNETWorkshop.sln
    - name: Build
      run: dotnet build src/MLNETWorkshop.sln --configuration Release --no-restore
    - name: Data Tests
      working-directory: 'test/DataTests'     
      run: dotnet test DataTests.csproj        
    - name: Train
      working-directory: 'src/TrainConsole'
      run: dotnet run --project TrainConsole.csproj    
```

Commit your changes and push them to GitHub. This should kick of the workflow under the `Actions` tab, and you should see a successful build within 10-15 min.

If you click on the succcessful workflow, you can inspect to see each build step and expand to see that the four data validation tests ran successfully.

![7-4-data-tests](./media/7-4-data-tests.png)

### Model tests
The model tests will run after you've trained your model to do some basic health checks.

In more advanced scenarios, you may also want to compare the trained model to an existing model in production at this stage so that you can quickly determine if the model is worth investing additional time in.

In this project, there is the `ModelTests.cs` test class in the `ModelTests` project (located under the `Tests` folder in the solution).

In this instance, you should run three tests on your model to ensure that it's able to correctly predict the price of a low-, mid- and high-range car within a given interval.

To do so, replace the content of the `ModelTests` class with the following:

```
        [TestMethod]
        public void Given_LowRangeCar_ShouldEstimatePriceWithinRange()
        {
            //Arrange
            var predictionEngine = GetPredictionEngine();

            var input = new ModelInput
            {
                Year = 2006,
                Mileage = 182248,
                Make = "Chevrolet",
                Model = "TrailBlazer4dr"
            };

            //Act
            var pricePrediction = predictionEngine.Predict(input).Score;

            //Assert
            pricePrediction.Should().BeInRange(2000, 6000);
        }

        [TestMethod]
        public void Given_MidRangeCar_ShouldEstimatePriceWithinRange()
        {
            //Arrange
            var predictionEngine = GetPredictionEngine();

            var input = new ModelInput
            {
                Year = 2013,
                Mileage = 38343,
                Make = "Acura",
                Model = "TSX5-Speed"
            };

            //Act
            var pricePrediction = predictionEngine.Predict(input).Score;

            //Assert
            pricePrediction.Should().BeInRange(13000, 18000);
        }

        [TestMethod]
        public void Given_HighRangeCar_ShouldEstimatePriceWithinRange()
        {
            //Arrange
            var predictionEngine = GetPredictionEngine();

            var input = new ModelInput
            {
                Year = 2016,
                Mileage = 20422,
                Make = "Lexus",
                Model = "GX"
            };

            //Act
            var pricePrediction = predictionEngine.Predict(input).Score;

            //Assert
            pricePrediction.Should().BeInRange(47000, 54000);
        }

        private PredictionEngine<ModelInput, ModelOutput> GetPredictionEngine()
        {
            var modelPath = MLConfiguration.GetModelPath();

            var mlContext = new MLContext();

            var model = mlContext.Model.Load(modelPath, out var schema);

            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model, schema);
        }
```

You can see that you're using the `MLContext` from ML.NET to load the model from the Azure FileShare, which is where it's saved as part of training. A `PredictionEngine` is created based on the `ModelInput` and `ModelOutput` schema created earlier. Using this `PredictionEngine`, you are then able to make a prediction based on several different inputs and compare the result with the expected result, in this case within a given range.

Commit the changes to your fork and push the changes to GitHub.

To ensure that these model tests are run as part of your workflow, add the following to your `dotnet-core.yml` file just after the `Train` step:

```
    - name: Model Tests
      working-directory: 'test/ModelTests'      
      run: dotnet test ModelTests.csproj   
```

Your complete `dotnet-core.yml` file should now look like: 

```
name: .NET Core

on:
  push:
    branches: [ master ]
  pull_request:
    branches: [ master ]
  
jobs:
  build:

    runs-on: ubuntu-latest

    steps:        
    - uses: actions/checkout@v2
    - name: Setup .NET Core
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 3.1.101   
    - name: 'Create mount points'
      run: 'sudo mkdir /media/data'
    - name: 'Map disk drive to Azure Files share folder'
      run: 'sudo mount -t cifs //ndcmelbourne.file.core.windows.net/data /media/data -o vers=3.0,username=ndcmelbourne,password=${{ secrets.STORAGEKEY }},dir_mode=0777,file_mode=0777'
    - name: Install dependencies
      run: dotnet restore src/MLNETWorkshop.sln
    - name: Build
      run: dotnet build src/MLNETWorkshop.sln --configuration Release --no-restore
    - name: Data Tests
      working-directory: 'test/DataTests'     
      run: dotnet test DataTests.csproj        
    - name: Train
      working-directory: 'src/TrainConsole'
      run: dotnet run --project TrainConsole.csproj  
    - name: Model Tests
      working-directory: 'test/ModelTests'      
      run: dotnet test ModelTests.csproj       
```

Again, push and commit these changes to your repo. A successful build should complete within 10-15 min, which you can observe under the `Actions` tab.

If you click on the succcessful workflow, you can inspect to see each build step and expand to see that your three model tests ran successfully.

![7-4-model-tests](./media/7-4-model-tests.png)