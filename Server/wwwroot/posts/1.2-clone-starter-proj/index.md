---
title: Clone the starter project
slug: 1.2-clone-starter-proj
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Get started
description: The first step in setting up a downr workshop is to clone the source code
phase: 1
step: 2
---

## Clone or download the starter application

We've set up an initial project for you to start from. You just need to clone this repo to your machine and then open it up in Visual Studio or your editor of choice. A simple way to clone the application is to run this command from a command line terminal:

```powershell
git clone https://github.com/luisquintanilla/mlnet-workshop
```

Alternatively, you can download a [zipped version of the project](https://github.com/luisquintanilla/mlnet-workshop/archive/master.zip) and unzip that.

Once you've done this, change directories to the *mlnet-workshop/src* folder where you'll find the *MLNETWorkshop.sln* file. Open the solution. You should see three projects, similar to what's shown here:

![](./media/project-structure.png)

## Build the application

At this point you should be able to build the solution for the first time, which you can do from Visual Studio or the command line with `dotnet build` from the root of the `src` folder.