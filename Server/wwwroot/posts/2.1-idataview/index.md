---
title: IDataView
slug: 2.1-idataview
author: brachtma
lastModified: 2020-07-14 02:40:10
pubDate: 2020-07-14 02:40:10
categories: Data
description: Learn about the IDataView and different data loading sources in ML.NET.
phase: 2
step: 2
---

Data in ML.NET is represented as an `IDataView`. An IDataView is a flexible and efficient way of describing tabular data (columns and rows). The `IDataView` component provides a very efficient, compositional processing of tabular data especially made for machine learning and advanced analytics applications. It is designed to efficiently handle high dimensional data and large data sets. It is also suitable for single node processing of data partitions belonging to larger distributed data sets. Some key distinctions of the IDataView include:

- *Immutability* - Cursoring through data does not modify input data in any way. The root data is immutable, and the operations performed to materialize derived data are repeatable. Immutability and repeatability enable transparent caching. Immutability also ensures that execution of a composed data pipeline graph is safe for parallelism. The IDataView system's immutability guarantees enable flexible scheduling without the need to clone data.
- *Lazy* - IDataView enables and encourages components to be lazy in both column and row directions. When only a subset of columns or a subset of rows is requested, computation for other columns and rows can be, and generally is, avoided.
- *High dimensionality* - The type system for columns includes homogeneous vector types, so a set of related primitive values can be grouped into a single vector-valued column.

Something else that the `IDataView` component provides is a `DataViewSchema`. As the name suggest, this is the schema of an `IDataView` which defines the types, names, and other annotations for the set of columns that make up an `IDataView`. Before loading data, you must define the schema of that data. You can use classes or Plain-Old-CLR-Objects (POCO) to define a `DataViewSchema`.

ML.NET provides support for loading data into an `IDataView` from various sources:

- *Files* - Load data from sources like text, binary, and image files. You can load data from a single file or multiple files. The supported formats include
- *Databases* - Load data from relational database sources. The following databases are a few of the databases supported:
  - Azure SQL Database
  - Oracle
  - IBM DB2
  - PostgreSQL
  - SQLite
  - SQL Server
- *Other* - Load data that can be represented by an `IEnumerable`. This can include JSON, XML, and many other sources.

See [Load data from files and other sources](https://docs.microsoft.com/dotnet/machine-learning/how-to-guides/load-data-ml-net) for more information on loading data.