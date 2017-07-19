using EntityFrameworkViewMigrations.PowerShellCommands.Configuration;
using EnvDTE;
using System;
using System.Data.Entity.Migrations;

namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    public class AddViewMigration : PowerShellCommand
    {
        public string SqlViewName { get; set; }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(this.SqlViewName))
            {
                throw new ArgumentNullException(nameof(SqlViewName));
            }

            var textDocument = this.GetActiveDocument();

            this.ChangeBaseType(textDocument);
            this.AddMissingUsing(textDocument);
            this.InsertDatabaseSqlFileCommands(textDocument);

            Console.WriteLine("DONE YEAH");
        }

        private void ChangeBaseType(TextDocument textDocument)
        {
            // Change from DbMigration to something
            textDocument.Selection.StartOfDocument();
            var result = textDocument.Selection.ReplacePattern(nameof(DbMigration), nameof(BaseDbMigration));

            Console.WriteLine("Replaced: " + result);
        }

        private void AddMissingUsing(TextDocument textDocument)
        {
            textDocument.Selection.EndOfDocument();
            textDocument.Selection.FindText("using", (int)vsFindOptions.vsFindOptionsBackwards);
            textDocument.Selection.LineDown();

            var @namespace = typeof(BaseDbMigration).Namespace;
            textDocument.Selection.Insert($"using {@namespace};");

            textDocument.Selection.NewLine();
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

            var databaseSqlFileCommand = $"this.{nameof(BaseDbMigration.DatabaseSqlFile)}({this.SqlViewName}{upOrDownString})";
            textDocument.Selection.Insert(databaseSqlFileCommand);
        }
    }
}
