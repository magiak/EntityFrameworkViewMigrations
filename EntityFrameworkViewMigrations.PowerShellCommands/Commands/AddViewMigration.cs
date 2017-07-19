using EntityFrameworkViewMigrations.PowerShellCommands.Configuration;
using EnvDTE;
using System;
using System.Data.Entity.Migrations;

namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    public class AddViewMigration : PowerShellCommand
    {
        public AddViewMigration(object dte) : base(dte)
        {
        }

        public string SqlViewName { get; set; }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(this.SqlViewName))
            {
                throw new ArgumentNullException(nameof(SqlViewName));
            }

            var textDocument = this.GetActiveDocument();

            this.ChangeBaseType(textDocument, nameof(BaseDbMigration));
            this.AddMissingUsing(textDocument, typeof(BaseDbMigration).Namespace);
            this.InsertDatabaseSqlFileCommands(textDocument);
        }

        private void InsertDatabaseSqlFileCommands(TextDocument textDocument)
        {
            // Insert into Up()
            this.InsertDatabaseSqlFile(textDocument, UpOrDownEnum.Up);

            // Insert into Down()
            this.InsertDatabaseSqlFile(textDocument, UpOrDownEnum.Down);
        }

        private void InsertDatabaseSqlFile(TextDocument textDocument, UpOrDownEnum upOrDown)
        {
            string upOrDownString = upOrDown.ToString();

            textDocument.Selection.StartOfDocument();
            textDocument.Selection.FindText($"{upOrDownString}()");

            textDocument.Selection.LineDown();
            textDocument.Selection.NewLine();

            var databaseSqlFileCommand = $"this.{nameof(BaseDbMigration.DatabaseSqlFile)}(\"{this.SqlViewName}{upOrDownString}\");";
            textDocument.Selection.Insert(databaseSqlFileCommand);
        }
    }
}
