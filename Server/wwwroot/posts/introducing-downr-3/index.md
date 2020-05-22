---
title: downr 3.0
slug: introducing-downr-3
author: bradygaster
lastModified: 2020-05-21 00:00:00
pubDate: 2020-05-21 00:00:00
categories: downr
description: downr, the simple-blogging-engine written for people who like using VS Code and Markdown to blog, has been updated atop ASP.NET Core 3.1 and Blazor WebAssembly
---

downr is a **very** simple blogging engine written for people who like using [Markdown](https://en.wikipedia.org/wiki/Markdown) to write content and Git to maintain their source code and/or content files. The goals of downr are to create a blogging engine that:

* Is built using .NET 3.1 so it can run anywhere
* Feels natural for developer bloggers who use [Visual Studio Code](http://code.visualstudio.com) to author their content in Markdown
* Supports being CI/CD'd anywhere
* Feels natural to .NET developers who frequently use the `dotnet` CLI
* Is open source and available on [GitHub](http://github.com/bradygaster/downr)
* Is build using Blazor WebAssembly (because why not?)

## Getting downr Running Locally

Getting downr running on a development workstation is easy. Open your favorite terminal window and run this:

    git clone https://github.com/bradygaster/downr.git
    cd downr/server
    dotnet run

## Blogging with downr

Blogging with downr is deliberately very simple - you just write Markdown. downr operates on two conventions - how you organize your markdown and content files and some simple YAML metadata in each post's Markdown file. 

### Example post structure

Consider this post, the introduction to downr. This post's slug is `introducing-downr-3`. This post's folder structure on disk looks like this. You place the Markdown files into the `Server\wwwroot\posts` folder.

    |-- introducing-downr-3
        |-- index.md
        |-- media
            |-- img1.png
            |-- img2.png

This ensures all your posts follow a nice, neat conventional folder structure. Once you have a gaggle of posts published to your site, it'll be all tidy like this:

    |-- wwwroot
        |-- posts
            |-- a-post-with-no-pictures
                |-- index.md
            |-- introducing-downr-3
                |-- index.md
                |-- media
                    |-- img1.png
                    |-- img2.png
            |-- some-other-post
                |-- index.md
                |-- media
                    |-- pic1.png
                    |-- pic2.png
                    |-- pic3.png

## Post Metadata

The top section of each Markdown file must contain a YAML header with some simple metadata elements. **All** of these elements are **required**. The YAML below demonstrates this convention.

    ---
    title: Introducing downr
    slug: introducing-downr
    author: bradygaster
    lastModified: 2017-01-06 12:00:00
    pubDate: 2017-01-06 12:00:00
    categories: downr
    description: Each post should have a description for the index page and for the RSS feed
    ---

## Customizing your site

If you want to customize the style or HTML layout, you have 4 files to edit in the `Client` folder:

* appsettings.json
* Pages/Index.razor
* Pages/Posts.razor
* wwwroot/index.html

Site-wide data can be edited using the `downr` section of `appsettings.json`. Customize this to change the title and banner of your site.

    "Downr": {
        "title" : "downr",
        "rootUrl" : "http://downr.azurewebsites.net",
        "pageSize": 2,
        "author": "the downr team",
        "indexPageText": "downr is a dirt-simple blogging tool written for people who like to author their content in markdown"
    }

Obviously, you can customize your site all you want, but if you're simply into blogging with Markdown you never need to look at the source code.  

## Dependencies

downr was created using these excellent open-source libraries:

* [Markdig](https://github.com/lunet-io/markdig) is used for Markdown-to-HTML conversion
* [YamlDotNet](http://aaubry.net/pages/yamldotnet.html) is used to parse the YAML headers in post Markdown files
* The [HTML Agility Pack for .NET Core](https://github.com/zulfahmi93/HtmlAgilityPack.NetCore) is used for the image path fix-ups

## Contributing

Contributions to downr are welcome and encouraged. Fork the [downr GitHub repository](http://github.com/bradygaster/downr) and submit pull requests to your heart's content.

### Naming Disclaimer
*Note: Product naming is difficult. I noticed that there were a few other projects out there [named](https://github.com/duhruh/Downr) [similarly](https://downr.codeplex.com/) to this one. The dotnet markdown blogging tool known as downr has no implied or accidental relationship to any of these other awesome projects.*