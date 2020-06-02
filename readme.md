# downr

downr is a ridiculously simple content tool written in .NET Core for folks who like to use Visual Studio Code and Git to write Markdown content.

## downr site modes

Content authors can use downr to write content in two modes:

1. Blog - bloggers can author blog content and it will appear chronologically, from newest to oldest.
1. Workshop - content creators can use downr to create step-by-step, sequenced content, useful for classes and workshops.

## downr content

You write Markdown and downr renders it using [Markdig](https://github.com/lunet-io/markdig), the fantastically awesome Markdown engine. The basic rules you follow are:

1. Each post is an *index.md* file in a sub-folder named for the post's slug.
2. Media for each post is in a `media` folder sitting in the same folder as the *index.md* file.

Consider the introduction post to downr. This post's slug is `introducing-downr-3`. This post's folder structure on disk looks like this. You place the Markdown files into the `Server\wwwroot\posts` folder.

    |-- introducing-downr-3
        |-- index.md
        |-- media
            |-- img1.png
            |-- img2.png

3. Each *index.md* file starts with YAML metadata that looks like this:

```yml
---
title: downr 3.0
slug: introducing-downr-3
author: bradygaster
lastModified: 2020-05-21 00:00:00
pubDate: 2020-05-21 00:00:00
categories: downr
description: downr, the simple-blogging-engine written for people who like using VS Code and Markdown to blog, has been updated atop ASP.NET Core 3.1 and Blazor WebAssembly
---
```

4. If the downr site is running in `Workshop` mode, each *index.md* file will **also** need the `phase` and `step` metadata elements to define each post's position.

```yml
---
title: installing downr
slug: installing-downr
author: bradygaster
lastModified: 2020-05-21 00:00:00
pubDate: 2020-05-21 00:00:00
categories: downr
description: In this step, you'll get downr running locally.
phase: 1
step: 1
---
```

## downr configuration

downr is deliberately simple, deliberately configurable, and deliberately opinionated in how it expects to be used. If you're writing a chronologically, like a blog, the *appsettings.json* file will look something like this:

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 4,
    "author": "author name",
    "indexPageText": "my blog",
    "imagePathFormat": "/posts/{0}/media/",
    "autoRefreshInterval": 0,
    "googleTrackingCode" : "",
    "siteMode" : "Blog"
  },
  "downr.AzureStorage" : {
    "ConnectionString" : "",
    "Container" : "posts"
  }
}
```

If your site is educational in nature and you want to order the content, like in a step-by-step workshop, your `appsettings.json` will have the `siteMode` set to `Workshop` instead of `Blog` (the default).

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 30,
    "author": "author name",
    "indexPageText": "my blog",
    "imagePathFormat": "/posts/{0}/media/",
    "autoRefreshInterval": 0,
    "googleTrackingCode" : "",
    "siteMode" : "Workshop"
  },
  "downr.AzureStorage" : {
    "ConnectionString" : "",
    "Container" : "posts"
  }
}
```

Note that in `Workshop` mode, the `pageSize` property is set to *obscenely* high on purpose, as you'd want the index page of any workshop to show everything you're going to do.

## auto-refresh

If you know you'll be frequently updating your site, or you have an obsessive need to make *sure* your content is always up-to-date no-matter-what, you can set the site to automatically reload data on intervals by setting the `autoRefreshInterval` property to **anything higher than 0.**

```json
"autoRefreshInterval": 5
```

This will cause the site to automatically reload the content files every 5 **minutes**. The default is 0 (thus, auto-refresh is disabled by default).

# storage modes

You can wire downr up using typical ASP.NET Core wire-up in `Startup.cs`. There aren't many options for using downr - you can basically opt for loading post files directly from disk, or from Azure Storage.

First, use the `AddDownr()` extension method to dial in the downr dependencies. Then, pick the storage mechanism. The code comes wired up to pull files from disk:

```csharp
public void ConfigureServices(IServiceCollection services)
{
  // ... other stuff

  services.AddDownr(Configuration) // wires up downr services
            .WithWebServerFileSystemStorage(); // with web server storasge
            //.WithAzureStorage(); // using azure storage blobs
  
  // ... other stuff
}
```

Then you use the services later, essentially starting the indexer and loading up the content and configuration.

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  // ... other stuff

  app.UseDownr() // use the services we dialed in
    .UseWebServerFileSystemStorage(); // use local web server storage
    //.UseAzureStorage(); // use azure blob storage

  // ... other stuff
}
```

If you decide to use Azure Storage, you'll need to add the requisite configuration to `appsettings.json` (or use an environment variable).

```json
"downr.AzureStorage" : {
  "ConnectionString" : "",
  "Container" : "posts"
}
```

The blob container in which you store your data will need to be public and anonymous. If you use Azure Storage to store your Markdown and image content, that enables you to go one step further and put Azure CDN on top of your storage account, so your site is as fast as possible.