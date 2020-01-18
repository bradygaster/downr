---
title: Introducing downr
slug: introducing-downr
author: bradygaster
lastModified: 2019-12-12 03:00:00
pubDate: 2019-12-12 03:00:00
categories: downr
description: downr is a very simple blogging engine written for people who like using Visual Studio Code to write content in Markdown format
---

downr is a **very** simple blogging engine written for people who like using [Markdown](https://en.wikipedia.org/wiki/Markdown) to write content and Git to maintain their source code and/or content files. The goals of downr are as follows:

* Enable bloggers with Markdown syntax-driven blogging
* Create an enjoyable experience for developer bloggers using [Visual Studio Code](http://code.visualstudio.com) or the [Visual Studio Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor) extension to author their content and maintain their site
* Support CI/CD

downr is written using [.NET Core](https://www.microsoft.com/net/core) and is open-source and available on GitHub at [http://github.com/bradygaster/downr](http://github.com/bradygaster/downr). 

## Getting downr Running Locally

Getting downr running on a development workstation is easy. The project is open source and available on GitHub. 

![downr repo on GitHub](media/github.png)

Here are the steps you'll take to get downr running locally:

    git clone https://github.com/bradygaster/downr.git
    cd downr
    dotnet run

## Blogging with downr

Blogging with downr is deliberately very simple - you just write Markdown. downr operates on two conventions - how you organize your markdown and content files and some simple YAML metadata in each post's Markdown file. 

### Example post

Consider this post, the introduction to downr. This post's slug is `introducing-downr`. Hence, the folder structure of the site, this post and all the content files associated with it inclusive, looks like the folder structure below. 

    |-- wwwroot
        |-- posts
            |-- introducing-downr
                |-- index.md
                |-- media
                    |-- img1.png
                    |-- img2.png

This ensures all your posts follow a nice, neat conventional folder structure, which results in something like the folders below once you have a few posts. 

    |-- wwwroot
        |-- posts
            |-- a-post-with-no-pictures
                |-- index.md
            |-- introducing-downr
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

If you want to customize the style or HTML layout, you have 4 files to edit:

* appsettings.json
* _Layout.cshtml
* Index.cshtml
* site.css

Site-wide data can be edited using the `downr` section of `appsettings.json`. Customize this to change the title and banner of your site. 

    "Downr": {
        "title" : "downr",
        "rootUrl" : "http://downr.azurewebsites.net",
        "homepageStyle" : "LatestPost",
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