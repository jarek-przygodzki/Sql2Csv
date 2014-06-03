Sql2Csv
=======

A simpe script to convert SQL Server query output to CSV  format

Install
-------
Clone repository
```
git clone https://github.com/jarek-przygodzki/Sql2Csv.git
```
And then just call
```
scriptcs -install
```
As a result, all packages specified in the packages.config, including transitive dependencies, will be downloaded and installed in the packages folder.

Usage
=====
`scriptcs Sql2Csv.csx -- -q  <Query> -c <ConnectionString> -o <OutputFilePath>`

Examples
--------
