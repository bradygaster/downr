---
title: Creating new posts
slug: creating-new-posts
author: bradygaster
lastModified: 2020-05-22 00:00:00
pubDate: 2020-05-22 00:00:00
categories: downr
description: Using the downr blogging tools and Visual Studio Code to author content
---

As mentioned in the [introduction](/introducing-downr), one of the goals of downr is that it feels natural for developer bloggers who use [Visual Studio Code](http://code.visualstudio.com) and the [.NET](http://dot.net) CLI. As such, we've created a [.NET tool](https://www.nuget.org/packages/downr-tools/) that contains a blog post template. 

## Install the downr tools

To make your downr blogging experience feel as natural as developing with .NET Core, run the following command to install the downr CLI tools.

    dotnet new --install downr-tools::1.0.0

Installing the tools will place a simple template into your `dotnet new` template list named `post`. 

## Create a new post

With the tools installed, creating a new post is simple. Open a terminal window in the `wwwroot\posts` folder of your site and execute this `dotnet new` command:

    dotnet new post --output new-post

This will create a the following folder structure underneath your `posts` folder: 

    |-- posts
        |-- new-post
            |-- index.md
            |-- media
                |-- placeholder.png

The `index.md` file will contain the defaults if you don't provide any parameters. From executing the command above, you'd get an `index.md` with this content:

    ---
    title: My new downr blog post
    slug: my-new-downr-blog-post
    author: author-name
    lastModified: 2020-01-19 12:15:38
    pubDate: 2020-01-19 12:15:38
    categories: blog,post,article
    description: This is a description of the post.
    ---

    Your content here in [Markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) format.

    ![Sample image](media/placeholder.png)

### Post template CLI parameters

If you'd prefer to use the template's CLI parameters to set your metadata up front, you can find out descriptions of the parameters via the `dotnet new post --help` command, which results with the following output:

    Options:                                                                                                      
        -t|--title        The title of your blog post                                                               
                            string - Optional                                                                         
                            Default: My new downr blog post                                                           

        -s|--slug         A URL-friendly slug that will make your post easy to find                                 
                            string - Optional                                                                         
                            Default: my-new-downr-blog-post                                                           

        -au|--author      The name you wish to be recognized by on the blog                                         
                            string - Optional                                                                         
                            Default: author-name                                                                      

        -c|--categories   A comma-separated list of categories in which your blog will appear                       
                            string - Optional                                                                         
                            Default: blog,post,article                                                                

        -d|--description  A description of your site. This will be used in the site RSS feed to introduce your post.
                            string - Optional                                                                         
                            Default: This is a description of the post.

So, you could get started with a new post, complete with the metadata to describe the post *rather* than the boring defaults, using something like: 

    dotnet new post -t "My exciting new post" -au happy-blogger -s my-exciting-new-post -o my-exciting-new-post -d "This post is going to be so awesome and you will be a better person for reading it."

If you're a detail-oriented CLI user who isn't afraid of that verbose a command, the results are like blogging zen. A fully-ready document awaiting your creative output. 

    ---
    title: My exciting new post
    slug: my-exciting-new-post
    author: happy-blogger
    lastModified: 2020-01-19 12:26:33
    pubDate: 2020-01-19 12:26:33
    categories: blog,post,article
    description: This post is going to be so awesome and you will be a better person for reading it.
    ---

Now, my metadata needs are satisfied, and I can start writing.
