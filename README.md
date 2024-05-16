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

       dotnet run --project Tulip.CLI db-setup Tulip/Tulip.db

5. (Optional) Log in to the admin panel and enable an AI Chat
system. You can either enter a ChatGPT API key, or generate and
upload a LLaMa 2 model (see [ai model generation documentation](Tulip/Hubs/Generating%20an%20AI%20model.md)).

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
1. Generate new database migrations (if the database was changed).

       dotnet ef migrations add [migration name] --project Tulip -- --environment Production

2. Generate a migration script (if the database was changed). This will create a file called `db-migration.sql`.

       dotnet ef migrations script --idempotent --project Tulip -o db-migration.sql -- --environment Production

3. Create the new application executable. This will create a `publish` folder containing the executable located at `tulip/Tulip/bin/Release/net8.0/publish/`

       dotnet publish 

4. Stop the production server.
5. Remove the old application code.
6. Upload the new application code to the server (the files inside the `publish` folder).
7. Run the migration SQL script on the database (if the database was changed).
8. Run the new application executable.
9. Verify the application is running properly.

## Useful deployment links

- [Host and deploy ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-8.0)
- [Migrations Overview](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
- [Applying Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli)
