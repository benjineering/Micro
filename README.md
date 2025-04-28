# Micro

## A small backend framework in .NET designed to be called by server side JS

### Design

.NET AOT code runs in lambdas with graviton (ARM).

- user writes a method 
	- decorated with a `[RequestHandler]` attribute
	- return type is a union type with optional error
	- value of return object can be an anonymous object (if we can check what it is using Rosyln)
- source generator creates:
	- strongly typed return types
	- lambda entrypoints
	- protobuf models
	- js functions to call the endpoints
	- zod (or other) schema via standardschema.dev
	- automatic versioning somehow
