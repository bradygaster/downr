---
title: Set up data source
slug: 7.2-data-source
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: MLOps
description: Set up the data source in Azure FileShare.
phase: 7
step: 3
---

Now you're able to compile your training project as part of your CI pipeline to ensure the integrity of the system. The next step is to automatically kick off the training of your machine learning model.

Before you can do that, you need to address an issue, which is the location of the training data. So far in this workshop, you have had your training data available on disk as part of the GitHub repository. However, in many cases the training data is 1-100GB, which makes it non-feasible to store it in GitHub.

One way to solve this problem is to upload your data to an Azure FileShare and mount the fileshare to the Ubuntu build agent as part of each build so that the training application can access it. An Azure FileShare can handle concurrent loads, meaning that multiple build agents can read the data simultaneously. 

In order to achieve this you need to do two things:
1) Change the path to your data
2) Mount the fileshare as part of your Github workflow

### Change the path to your data
Navigate to the `Program.cs` file and change the `TRAIN_DATA_PATH` variable to:
```
  private static string TRAIN_DATA_FILEPATH = @"/media/data/true_car_listings.csv";
```

In addition, you'll also need to change the path to where you stored the model. To do so, change the `MODEL_FILEPATH` variable to:
```
  private static string MODEL_FILEPATH = MLConfiguration.GetModelPath();
```

This will store the model on the fileshare with a unique ID matching the Git commit SHA.
Commit the changes to your master branch and push the changes to your repo.

### Mount the fileshare as part of your GitHub workflow
To mount the fileshare as part of your workflow, open the `dotnet-core.yml` file located under `.github/workflows` in your repo. 

Add the following just before the `Install dependencies` step and commit and push the changes to your master branch.

```
 - name: 'Create mount points'
      run: 'sudo mkdir /media/data'
    - name: 'Map disk drive to Azure Files share folder'
      run: 'sudo mount -t cifs //ndcmelbourne.file.core.windows.net/data /media/data -o vers=3.0,username=ndcmelbourne,password=${{ secrets.STORAGEKEY }},dir_mode=0777,file_mode=0777'
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

```

To mount the fileshare, you will also need to add the access key to the Azure Storage Container as a GitHub secret.

To add a secret, navigate to the `Settings` tab and select `Secrets` in the left menu:

![secrets](./media/secrets.png)

Click on `New Secret` and add a new secret with the name `STORAGEKEY`. The value will be provided to you by the workshop facilitators.