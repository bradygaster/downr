---
title: Register a Formatter
slug: 8.4-register-formatter
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: DataFrame and Jupyter
description: Register a formatter so that your Notebook can format the data from DataFrame.
phase: 8
step: 5
---

Jupyter doesn't know how to format the data from the DataFrame API, so we need to register a formatter for it. In a new cell, use the below code that creates a formatter.

```csharp
using Microsoft.AspNetCore.Html;
Formatter<DataFrame>.Register((df, writer) =>
{
    var headers = new List<IHtmlContent>();
    headers.Add(th(i("index")));
    headers.AddRange(df.Columns.Select(c => (IHtmlContent) th(c.Name)));
    var rows = new List<List<IHtmlContent>>();
    var take = 10;
    for (var i = 0; i < Math.Min(take, df.Rows.Count); i++)
    {
        var cells = new List<IHtmlContent>();
        cells.Add(td(i));
        foreach (var obj in df.Rows[i])
        {
            cells.Add(td(obj));
        }
        rows.Add(cells);
    }

    var t = table(
        thead(
            headers),
        tbody(
            rows.Select(
                r => tr(r))));

    writer.Write(t);
}, "text/html");
```

Without this formatter the data won't be formatted correctly and we would have a hard time reading it. It would look like the below.

![Unformatted Data](./media/dataframe-unformatted.png)