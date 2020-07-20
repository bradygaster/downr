---
title: Add model training to the GitHub workflow
slug: 7.3-add-training-gh-workflow
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: MLOps
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 7
step: 3
---

To automatically train our model, we will need to use the `dotnet run` command to run our console application.
To do so, go ahead and add the following to your GitHub Action's workflow file. Commit and push the changes to your master branch
```
    - name: Train
      working-directory: 'src/TrainConsole'
      run: dotnet run --project TrainConsole.csproj
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
    - name: Train
      working-directory: 'src/TrainConsole'
      run: dotnet run --project TrainConsole.csproj 
```

The workflow should take about 10-15 minutes to complete, and if all is set up correctly should yield a green build.

![7-4-build](./media/7-4-build.PNG)

If you click on the workflow that has been run, we can see the progress of each step

![7-4-build-details](./media/7-4-build-details.PNG)