#WebApi-AngularJS

Simple Web Api & AngularJS application.

## Project structure

- DemoProject.API - Web Api
- DemoProject.API.Tests - Tests for Web Api methods
- DemoProject.Common - Common functionality
- DemoProject.Model - Database model and context
- DemoProject.Web - UI



## Server-side

- ASP.NET Web API
- Entity Framework (Code First)
- Castle Windsor

__DemoProject.API__ is configured to use Local IIS server. Project URL is set to http://localhost/api



## Client-Side

- AngularJs
- Bootstrap

__DemoProject.Web__ is configured to use Loca IIS server. Project URL is set to http://localhost/web

_ApiUrl_ configuration is located in DemoProject.Web/scripts/app/config.js 



## How To

### Configuration and Compilation
- Change Database connection string (DemoProjectContext) in DemoProject.API\web.config
- Change Storage location (StoragePath) in DemoProject.API\web.config
- Compile the solution
- Ensure that Virtual Directories were created in IIS ("Default Web Site\api" and "Default Web Site\web")


### Testing. UI

Use http://localhost/web

### Testing. Fiddler

Use Composer to create requests to endpoints:

__Metadata__

 - http://localhost/api/metadata (GET)
 - http://localhost/api/metadata/id (GET)
 - http://localhost/api/metadata/id (DELETE)

__Uploads__

 - http://localhost/api/upload (POST, multi-part content is expected)

__Downloads__

 - http://localhost/api/download/{id} (GET)

### Testing. CURL

GET files metadata

>curl.exe http://localhost/api/metadata

POST a file (upload a file)

>curl.exe -X POST -F file=@TestFile.txt http://localhost/api/upload


## TODO

- Uploads validation
- Progress bars to UI
- Authorization