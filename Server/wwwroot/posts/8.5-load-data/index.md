---
title: Load the CSV data
slug: 8.5-load-data
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: DataFrame and Jupyter
description: Use the DataFrame API to load data from a CSV file.
phase: 8
step: 6
---

The DataFrame API offers a convenient method to load in the CSV data. In a new cell, use the below code to load in the data into a variable.

```csharp
var data = DataFrame.LoadCsv("../../data/true_car_listings.csv", separator: ',');
```

The `separator` parameter tells the method what delimiter the file has. In this case we have a comma separating each value in the data file.

Next, you can run a new cell with the `data` variable and it will print out the top 10 rows of the data. Notice how well it is formatted since we added the formatter from phase 9.3.

![Loaded Data](./media/loaded-data.png)