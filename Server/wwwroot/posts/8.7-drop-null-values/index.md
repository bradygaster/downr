---
title: Drop Null Values
slug: 8.7-drop-null-values
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: DataFrame and Jupyter
description: Get rid of null and empty values in your data.
phase: 8
step: 8
---

Often times datasets will come in with null or missing values. There are several ways to handle null values, such as setting the values to a default value or imputing the mean of the other values. You can also drop any null columns or rows so they aren't used in the dataset.

In a new cell, use the below code to drop any null values and assign the new dataset to a variable.

```csharp
var newData = data.DropNulls();
```

In a new cell, run the `newData` variable to view the top 10 rows from it.

![Dropped Null Values](./media/null-values.png)