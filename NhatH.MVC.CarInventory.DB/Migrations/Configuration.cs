namespace NhatH.MVC.CarInventory.DB.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<NhatH.MVC.CarInventory.DB.CarInventoryDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            //Check if any migration pending to run, this can happen if database doesn't exists or if any change in the shcema
            var migrator = new DbMigrator(this);
            var _pendingMigrations = migrator.GetPendingMigrations().Any();
            //If ther are penidng migrations run migrate.Update() to create/Update the database then run the Seed() method to populate the needed data
            if (_pendingMigrations)
            {
                migrator.Update();
                Seed(new CarInventoryDBContext());
            }
        }

        protected override void Seed(NhatH.MVC.CarInventory.DB.CarInventoryDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            // Apply changes to database
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
