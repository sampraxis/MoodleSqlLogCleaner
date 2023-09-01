using LogCleaner.Commands;
using LogCleaner.Configurations;
using LogCleaner.Services;


var command = new Command(args);
var config = new Configuration();
var service = new LogCleanerService();

var cli = new CommandHandler(command, config, service);

await cli.Execute();
