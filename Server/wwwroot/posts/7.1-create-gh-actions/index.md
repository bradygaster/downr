---
title: Create GitHub Actions workflow
slug: 7.1-create-gh-actions
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: MLOps
description: The first step is to make sure you have all the requirements and to clone the workshop source code.
phase: 7
step: 1
---

In this section you will learn how to automate the model lifecycle from training to model deployment. 
We will in addition look at some additional considerations such as data and model tests.


## Phase 7.1: Create our first GitHub Actions workflow
The first thing we want to do is to create a simple GitHub Actions workflow which will be triggered to train a new model when a commit is pushed to our repository.

To do so, navigate to your forked repo and click on the `Actions` tab
On the page that appears, go ahead and select to set up a new .NET Core workflow
![action](./media/action-dotnet-core-workflow.png)

GitHub will provide you with a template workflow that is intended to restore, build, and test a .NET Core app. 

Replace the workflow file with the following content and commit it to your master branch.

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
    - name: Install dependencies
      run: dotnet restore src/MLNETWorkshop.sln
    - name: Build
      run: dotnet build src/MLNETWorkshop.sln --configuration Release --no-restore
``` 

If all goes well, a successful build should complete in less than a minute.

![7-1-build](./media/7-1-build.png)