namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    using Base;
    using Migrations.Base;

    public class AddSeedDbMigration : PowerShellCommand
    {

        public AddSeedDbMigration(object dte, object project) : base(dte, project)
        {
        }

        public override void Execute()
        {
            var textDocument = this.GetActiveDocument();

            this.ChangeBaseType(textDocument, nameof(SeedDbMigration));
            this.AddMissingUsing(textDocument, typeof(SeedDbMigration).Namespace);
            this.RemoveUpAndDownFunctions(textDocument);
        }
    }
}
