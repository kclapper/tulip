# Tulip
The SAP gamification web app.

# Getting Started
## Requirements
- .NET SDK version 8.0

## Installation
1. Clone the repository 
2. `cd` into the repository root directory
3. Install project dependencies

        dotnet restore

4. Setup the development database using the setup tool 

       dotnet tulip db-setup Tulip/Tulip.db

## Development
1. `cd` to the repository root directory
2. Start the ASP.NET application
       
       dotnet watch run --environment Development --project Tulip

> CAUTION: running the `watch` command without specifying the `Development` environment
> will cause the application to connect to the production database.

## Shutdown
1. Use Cntl-C to stop the development server 
   (this command may be different depending on
   operating system/terminal).

# Deployment
When this application is deployed to production, it should be started 
with the `Production` environment. This can be achieved with the
command `dotnet run --environment Production --project Tulip`.

Some systems may do this automatically. The default environment is
`Production`. 

> NOTE: Any time the .Net data models change, the production database will need
> to be updated to reflect the new schema. Use .NET EF Core Migrations for this.
