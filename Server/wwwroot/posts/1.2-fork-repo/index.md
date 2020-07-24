---
title: Fork the repo
slug: 1.2-fork-repo
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Get started
description: The first step in setting up a downr workshop is to fork and clone the repo
phase: 1
step: 2
---

We've set up an initial project for you to start from: [ML.NET Workshop Repo](https://github.com/luisquintanilla/mlnet-workshop)

To get started, please complete the following steps:
- Fork the repo
- Clone your forked repo locally, e.g. by running the following command (replace luisquintanilla with your GitHub username):
```powershell
git clone https://github.com/luisquintanilla/mlnet-workshop
```

Once you've done this, change directories to the *mlnet-workshop/src* folder where you'll find the *MLNETWorkshop.sln* file. Open the solution. You should see three projects, similar to what's shown here:

![Project structure](./media/project-structure.png)

## Build the application

At this point you should be able to build the solution for the first time, which you can do from Visual Studio or the command line with `dotnet build` from the root of the `src` folder.