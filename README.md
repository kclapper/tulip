# Tulip
The SAP gamification web app.

# Getting Started
## Requirements
- .NET SDK version 8.0
- Docker

## Installation
1. Install Docker
    - If you're using a Mac computer with an M series
      processor, you will need to enable Rosetta 2 mode in the Docker settings.
2. Clone the repository 
3. `cd` into the repository root directory
4. Setup the database container by running the setup
   script 

       ./database.sh setup

5. Install project dependencies

        dotnet restore

6. Run database migrations 

        dotnet dotnet-ef --project Tulip database update -- --environment Development

> CAUTION: running this command without specifying the `Development` environment
> may cause the application to connect to the production database.
   
## Development
1. `cd` to the repository root directory
2. Start the local development database (if not already running)

        ./database.sh start

2. Start the ASP.NET application
       
       dotnet watch run --environment Development --project Tulip

> CAUTION: running the `watch` command without specifying the `Development` environment
> will cause the application to connect to the production database.

> NOTE: Any time the .Net data models change, the local database will need
> to be updated to reflect the new schema. To do this, run:
> `dotnet dotnet-ef --project Tulip database update -- --environment Development`

## Shutdown
1. Use Cntl-C to stop the development server 
   (this command may be different depending on
   operating system/terminal).
2. Stop the local database

        ./database.sh stop

# Deployment
When this application is deployed to production, it should be started 
with the `Production` environment. This can be achieved with the
command `dotnet run --environment Production --project Tulip`.

Some systems may do this automatically. The default environment is
`Production`. 