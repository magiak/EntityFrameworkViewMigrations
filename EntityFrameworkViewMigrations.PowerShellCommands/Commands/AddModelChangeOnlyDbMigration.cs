namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    using Base;
    using Migrations.Base;

    public class AddModelChangeOnlyDbMigration : PowerShellCommand
    {
        public AddModelChangeOnlyDbMigration(object dte, object project) : base(dte, project)
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
