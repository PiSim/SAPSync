# DMTAgent 
v 0.5.0

Application for generic data extraction, elaboration and report production

The Project started as a bulk data extractor from the SAP ERP and later evolved into a generic tool
for data manipulation.

A tasklist can be configured using an extensible internal framework, currently implemented operations are:

- MySQL Read/Write  via EfCore
- Read from SAP DB via RFC modules and .NET connector
- Excel Read/Write via OpenXml
- In-Memory manipulation

The framework can be extended to include any .NET compatible data source and destination and potentially it can be used to track and perform any kind of task.

A lightweight agent can be run in a background thread to periodically execute the tasklist on a configurable schedule.

