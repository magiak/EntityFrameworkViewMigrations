using System.Data.Entity.Migrations;
using EntityFrameworkViewMigrations.PowerShellCommands.Configuration;
using EnvDTE;

namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
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
        
        private void RemoveUpAndDownFunctions(TextDocument textDocument)
        {
            // Find line with UP
            textDocument.Selection.StartOfDocument();
            textDocument.Selection.FindText("Up()");

            this.RemoveCurrentLine(textDocument, NumberOfUpAndDownSourceLines);
        }
    }
}
