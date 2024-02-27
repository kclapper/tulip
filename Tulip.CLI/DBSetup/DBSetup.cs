using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tulip.Data;
using Tulip.Models;

namespace Tulip.CLI
{
    class DBSetup: ICommand
    {
        public static string Name { get; } = "db-setup";
        public static string Help { get; } = "";

        private DbContext productionDb;
        private DbContext developmentDb;

        public DBSetup()
        {
            productionDb = getProdDbContext();
            developmentDb = getDevDbContext();
        }

        private static DbContext getProdDbContext()
        {
            string productionAppSettingsFilePath = "./Tulip/appsettings.Production.json";
            string jsonString = File.ReadAllText(productionAppSettingsFilePath);
            string productionConnectionString = JsonNode.Parse(jsonString)!["ConnectionStrings"]!["DefaultConnection"]!.GetValue<string>();
            productionConnectionString += ";TrustServerCertificate=true";

            DbContextOptionsBuilder sqlServerOptions = new DbContextOptionsBuilder();
            sqlServerOptions.UseSqlServer(productionConnectionString);

            return new ApplicationDbContext(sqlServerOptions.Options);
        }

        private static DbContext getDevDbContext()
        {
            string developmentAppSettingsFilePath = "./Tulip/appsettings.Development.json";
            string jsonString = File.ReadAllText(developmentAppSettingsFilePath);
            string developmentConnectionString = JsonNode.Parse(jsonString)!["ConnectionStrings"]!["DefaultConnection"]!.GetValue<string>();

            DbContextOptionsBuilder sqliteOptions = new DbContextOptionsBuilder();
            sqliteOptions.UseSqlite(developmentConnectionString);

            return new ApplicationDbContext(sqliteOptions.Options);
        }

        public ICommand Configure(List<string> args)
        {
            return this;
        }

        public void Execute()
        {
            var productionEntities = productionDb.Model.GetEntityTypes();    
            foreach (var entity in productionEntities)
            {
                var type = entity.ClrType;
                var dbSet = (IEnumerable<object>)productionDb.GetType()
                                    .GetMethod("Set", [])?
                                    .MakeGenericMethod(type)
                                    .Invoke(productionDb, [])!;

                if (dbSet == null)
                {
                    continue;
                }

                foreach (var entry in dbSet)
                {
                    Console.WriteLine(entry);
                    developmentDb.Add(entry);
                }
            }

            developmentDb.SaveChanges();
        } 
    }
}