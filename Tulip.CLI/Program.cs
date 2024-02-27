// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;
using Tulip.CLI;

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
    builder.AddConsole();
}); 

var commandRegistry = new Dictionary<string, ICommand>
{
    { DBSetup.Name, new DBSetup(loggerFactory) }
};

var parser = new ArgumentParser(args);
var cliCommand = parser.Args.Dequeue();

if (!commandRegistry.ContainsKey(cliCommand))
{
    throw new ArgumentException($"Command not found: {cliCommand}");
}

var command = commandRegistry[cliCommand];
command.Configure(parser.Args.ToList(), parser.Kwargs);
command.Execute();