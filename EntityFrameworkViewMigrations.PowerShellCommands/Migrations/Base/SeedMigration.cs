namespace EntityFrameworkViewMigrations.PowerShellCommands.Migrations.Base
{
    /// <summary>
    /// Use ModelChangeOnlyDbMigration class to mark a dummy
    /// migration that only changes the model but not the database.
    /// When using ModelChangeOnlyDbMigration, write a comment
    /// above the derived class and state why you use an empty migration.
    /// </summary>
    public class SeedMigration : BaseDbMigration
    {
        public override void Up()
        {
            this.Seed();
        }
    }
}
