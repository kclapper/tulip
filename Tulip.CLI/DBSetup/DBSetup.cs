using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Tulip.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tulip.CLI
{
    class DBSetup: ICommand
    {
        public static string Name { get; } = "db-setup";
        public static string Help { get; } = "";

        private DbContext productionDb;

        private string developmentDbPathString;
        private DbContext developmentDb;

        private ILogger logger;

        public DBSetup(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("DBSetup");
        }

        private DbContext getProdDbContext()
        {
            string productionAppSettingsFilePath = "./Tulip/appsettings.Production.json";
            string jsonString = File.ReadAllText(productionAppSettingsFilePath);
            string productionConnectionString = JsonNode.Parse(jsonString)!["ConnectionStrings"]!["DefaultConnection"]!.GetValue<string>();
            productionConnectionString += ";TrustServerCertificate=true";

            DbContextOptionsBuilder sqlServerOptions = new DbContextOptionsBuilder();
            sqlServerOptions.UseSqlServer(productionConnectionString);

            return new ApplicationDbContext(sqlServerOptions.Options);
        }

        private DbContext getDevDbContext()
        {
            string developmentConnectionString = $"Data Source={developmentDbPathString}";

            DbContextOptionsBuilder sqliteOptions = new DbContextOptionsBuilder();
            sqliteOptions.UseSqlite(developmentConnectionString);

            return new ApplicationDbContext(sqliteOptions.Options);
        }

        public ICommand Configure(List<string> args, Dictionary<string, string> kwargs)
        {
            if (args.Count == 0)
            {
                throw new ArgumentException(
                    "No output path provided!\n"
                    + "Please specify where to put the development database\n"
                    + "Ex: dotnet tulip db-setup Tulip/Tulip.db\n"
                );
            }

            if (args.Count != 1)
            {
                throw new ArgumentException(
                    "Too many arguments provided!"
                );
            }

            developmentDbPathString = args.First();

            productionDb = getProdDbContext();
            developmentDb = getDevDbContext();

            return this;
        }

        private void prepareDevDb() 
        {
            developmentDb.Database.EnsureDeleted();
            developmentDb.Database.EnsureCreated();
        }

        private IEnumerable<object> getDbEntitiesOfType(IEntityType entity)
        {
            var type = entity.ClrType;
            return (IEnumerable<object>)productionDb.GetType()
                                .GetMethod("Set", [])?
                                .MakeGenericMethod(type)
                                .Invoke(productionDb, [])!;
        }

        public void Execute()
        {
            prepareDevDb();

            int successfulTransactions = 0;

            var productionEntities = productionDb.Model.GetEntityTypes();    
            foreach (var entity in productionEntities)
            {
                var dbSet = getDbEntitiesOfType(entity);

                /* If no records are found, the dbSet will be null and throw a null pointer exception. 
                 * If the produciton database schema doesn't have the entity type, it will throw an
                 * Invalid object error. In both cases we want to ignore the records. 
                */  
                try 
                {
                    dbSet.Count();
                }
                catch (Exception e)
                {
                    logger.LogWarning(
                        $"Could not save entity type: {entity}\n"
                        + $"The database schema may not support this\n"
                        + $"type or it may have no entries.\n"
                        + $"{e.GetType()}\n"
                        + $"{e.Message}\n"
                    );
                    continue;
                }

                foreach (var entry in dbSet)
                {
                    try {
                        developmentDb.Add(entry);
                        developmentDb.SaveChanges();
                        successfulTransactions++;
                    }
                    catch (Exception e) {
                        developmentDb = getDevDbContext(); // Reset the DB context

                        var innerExceptionMessage = e.InnerException == null ? "" : e.InnerException.Message;

                        logger.LogWarning(
                            $"Could not save entry: {entry} \n"
                            + $"{e.Message}\n"
                            + $"{innerExceptionMessage}\n"
                        );
                    }
                }
            }

            logger.LogInformation(
                "Development database setup complete\n"
                + $"Successful transactions: {successfulTransactions}\n"
                + """
                  Note: Some transactions may have failed due to duplicate data or incompatibility between
                  the development database and production database. This is to be expected.  
                  """
            );
        } 
    }
}