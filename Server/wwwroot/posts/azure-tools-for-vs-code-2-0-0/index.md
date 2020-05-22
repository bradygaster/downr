---
title: Important Updates regarding Azure Tools for Visual Studio Code V2
slug: azure-tools-for-vs-code-2-0-0
author: bradygaster
lastModified: 2018-01-26 12:00:00
pubDate: 2018-01-26 12:00:00
categories: Visual Studio Code,Azure
description: Updates to the V2 you will need to be aware of
---

Some time has passed since the last update of the [Azure Tools for Visual Studio Code](https://aka.ms/vscodeazuretools), and in that time there have been some great advancements in the extensions available to Azure developers who use [Visual Studio Code](http://code.visualstudio.com). Some **amazing** new extensions have appeared in the [Azure category](https://marketplace.visualstudio.com/search?target=VSCode&category=Azure&sortBy=Downloads) of the [Visual Studio Marketplace](https://marketplace.visualstudio.com/) that make it easy and fun to party with Azure. 

## V2 will remove some features 

The short version is that the official extensions - [Azure App Service](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice) and [Azure Storage](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurestorage) - provide much better experiences for their service areas than does our (myself, [Christos Matskas](https://cmatskas.com/), and [Ian Philpot](https://tripdubroot.com/)) extension. So we're removing the redundant functionality from the [Azure Tools for Visual Studio Code](https://aka.ms/vscodeazuretools) in lieu of the official extensions' features. There will be a notice of this change when the extension is updated. 

The official extensions are **so** much easier to use, offer richer experiences and more discoverable features, and the authentication experience is great. Plus, the official extensions share certain components, yet revise independently. Smaller pieces, easier autonomy for the individual feature owners. **Yay extensibility!**

## Future Plans

The great folks in the Azure SDK and Visual Studio Code teams have worked together to create not only a great set of extensions for Azure developers, but they've also given extension authors a common dependency to get started, the [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account) extension. There's [guidance](https://github.com/Microsoft/vscode-azure-ext-sdk) for developers who want to write Azure extensions for Visual Studio Code, too.

With so many improvements in the community of extensions being developed around this great SDK and new directions in how authors contribute Azure features our team has some opportunities ahead for distributing the individual resource management areas into separate, smaller extensions. Obviously, there's a huge benefit to retrofitting our extension with the new common underpinnings, too. We'll keep everyone updated in the README.md updates to the extension, link back to this blog for updates to the extension. 

## Question - to depend or not? 

There are various schools of thought on adding arbitrary dependencies for feature redirection. My personal feelings on this is that I'd be receptive to an extension author who takes a dependency on a later extension that improves/replaces the original feature. But some find this intrusive. 

**What's your opinion?** Leave a comment. Given the potential intrusiveness of taking a dependency on the official extensions, I'll hold off, but given it'd be my own personal preference, I'd implement this next if the community prefers it. 