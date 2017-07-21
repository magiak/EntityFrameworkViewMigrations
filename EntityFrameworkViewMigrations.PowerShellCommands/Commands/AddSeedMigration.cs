using EntityFrameworkViewMigrations.PowerShellCommands.Migrations.Base;

namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    using Migrations;

    public class AddSeedMigration : PowerShellCommand
    {

        public AddSeedMigration(object dte) : base(dte)
        {
        }

        public override void Execute()
        {
            var textDocument = this.GetActiveDocument();

            this.ChangeBaseType(textDocument, nameof(SeedMigration));
            this.AddMissingUsing(textDocument, typeof(SeedMigration).Namespace);
            this.RemoveUpAndDownFunctions(textDocument);
        }
    }
}
