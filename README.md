# SAPSync
Application for generic data extraction, elaboration and report production

The Project started as a bulk data extractor from the SAP ERP (hence the name) and later evolved into a generic tool
for data manipulation.

A tasklist can be configured using an extensible internal framework, currently implemented operations are:

-MySQL Read/Write  via EfCore
-Read from SAP DB via RFC modules and .NET connector
-Excel Read/Write via OpenXml
-In-Memory manipulation

The framework can potentially be extended to include any .NET compatible data source and destination.

A lightweight agent can be run in a background thread to periodically execute the tasklist on a configurable schedule.

The tasklist is hardcoded in the present version, and xml-based configuration feature will hopefully be implemented shortly.
