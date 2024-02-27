# Tulip CLI

The Tulip CLI is a tool used to help setup and manage
project Tulip. It's primary purpose is to set up and
update the development environment.

## Usage

## Development

To run the project in development mode, from the repository root run:

    dotnet run --project Tulip.CLI <arguments>

To integrate the tool after development, from the repository root run:

    dotnet pack
    dotnet tool uninstall Tulip.CLI
    dotnet tool install --add-source ./Tulip.CLI/nupkg Tulip.CLI

## Tests

To run the tests, navigate to the `Tulip.CLI.Tests` folder
and run `dotnet test`.