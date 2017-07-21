namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    using Migrations;

    public class AddModelChangeOnlyDbMigration : PowerShellCommand
    {
        private const int NumberOfUpAndDownSourceLines = 7;

        public AddModelChangeOnlyDbMigration(object dte) : base(dte)
        {
        }

        public override void Execute()
        {
            var textDocument = this.GetActiveDocument();

            this.ChangeBaseType(textDocument, nameof(ModelChangeOnlyDbMigration));
            this.AddMissingUsing(textDocument, typeof(ModelChangeOnlyDbMigration).Namespace);
            this.RemoveUpAndDownFunctions(textDocument);
        }
    }
}
