# Tulip CLI

The Tulip CLI is a tool used to help setup and manage
project Tulip. It's primary purpose is to set up and
update the development environment.

## Usage

To use the Tulip CLI, from the repository root run:

    dotnet run --project Tulip.CLI <arguments>

### Commands

Only a single command exists at the moment:

#### db-setup

    dotnet run --project Tulip.CLI db-setup <path to output SQLite3 file>

For example:

    dotnet run --project Tulip.CLI db-setup Tulip/Tulip.db

## Tests

To run the tests, navigate to the `Tulip.CLI.Tests` folder
and run `dotnet test`.