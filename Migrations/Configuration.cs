using System.Data.Entity.Migrations;
using Dictionary.Models;
using Dictionary.Seeders;

namespace Dictionary.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Dictionary.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            Seeder.SeedTestData(context);
        }
    }
}
