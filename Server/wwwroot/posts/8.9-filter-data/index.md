---
title: Filter data
slug:  8.9-filter-data
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: DataFrame and Jupyter
description: Filter out the data that you don't need.
phase: 8
step: 10
---


Along with aggregating data to get a better understanding of it, there may be pieces of the data you care more about than others. Being able to filter out the data that you don't need helps in this case.

In a new cell, use the below code to filter out the "Mileage" column to only show cars that have a mileage greater than or equal to 10,000.

```csharp
var mileage = newData[newData.Columns["Mileage"].ElementwiseGreaterThanOrEqual(100000)];
```
In another cell, run the `mileage` variable to get a view of the data.

![Filtered Data](./media/filtered-data.png)