---
title: Announcing the Deprecation of the Azure Tools for Visual Studio Code
slug: azure-tools-for-vs-code-deprecation
author: bradygaster
lastModified: 2018-04-17 12:00:00
pubDate: 2018-04-17 12:00:00
categories: Visual Studio Code,Azure
description: All good things must come to an end
---

A few months ago I let the community know that I'd be deprecating some features from the [Azure Tools for Visual Studio Code](https://aka.ms/vscodeazuretools). Some **amazing** new extensions have appeared in the [Azure category](https://marketplace.visualstudio.com/search?target=VSCode&category=Azure&sortBy=Downloads) of the [Visual Studio Marketplace](https://marketplace.visualstudio.com/) that make it easy and fun to party with Azure. With the great work that's getting done by folks in the Visual Studio Code Azure Tools team, I think it's safe to say that full deprecation is in order. 

## Why? What's the deal? 

To make it as short and sweet as possible, I don't want to confuse anyone who is looking in the marketplace for the **real Azure tools**. This extension has been in the marketplace for a long time, and as a result of frequent updates at our beginning, we've had a great running and our download count has gotten pretty great. We **wholly appreciate the great feedback, contributions, and issues we've received** on the extension. 

I started this extension with a few goals:

1. Learn more about the Node.js SDK.
1. Use the Node.js SDK with the VS Code extensibility API.
1. Have a tool I could use to do things I have to do frequently when I'm building stuff in Azure.

The extension definitely gave an opportunity for those learnings, but it also introduced a lot of folks to developing with Azure within VS Code. Since those early beginnings there's been a **huge set of investments** in enabling Azure developer experiences within VS Code, and I want to make sure customers don't stumble onto our humble beginnings and miss out on the **true** awesomeness of the official Azure extensions. 

## When? 

I'll remove the extension from the marketplace on **April 27th, 2018**. 

## What's Next Here?

Simply - nothing. The original extension's [repository](https://github.com/bradygmsft/azure-tools-vscode) will remain intact so developers can learn from the code or clone-and-package it on your own using the [VSCE](https://code.visualstudio.com/docs/extensions/publish-extension) tools for packaging and publishing your own extensions. 

Again - thanks for your use of the extension, contributions, and ideas. Please take my advice, try out the new and official extensions for App Service, Cosmos DB, mssql, Azure Functions, IoT - all of it! Happy coding!