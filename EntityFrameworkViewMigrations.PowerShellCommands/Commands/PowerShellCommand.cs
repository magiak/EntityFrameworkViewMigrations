using System;
using System.Data.Entity.Migrations;
using EntityFrameworkViewMigrations.PowerShellCommands.Configuration;
using EnvDTE;
using EnvDTE80;

namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    public abstract class PowerShellCommand : IPowerShellCommand
    {
        private readonly DTE2 dte2;

        protected PowerShellCommand(object dte)
        {
                var dteRef = dte as DTE2;
                if (dteRef != null)
                {
                    this.dte2 = dteRef;
                    Console.WriteLine("YEAH CASTED");
                }
                else
                {
                    Console.WriteLine("NOPE NOT CASTED");
                    this.dte2 = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.14.0");
                }
        }
        
        public abstract void Execute();

        protected TextDocument GetActiveDocument()
        {
            return (TextDocument)this.dte2.ActiveDocument.Object(string.Empty);
        }

        protected void ChangeBaseType(TextDocument textDocument, string newBaseTypeName)
        {
            // Change from DbMigration to something
            textDocument.Selection.SelectAll();
            textDocument.Selection.ReplacePattern(nameof(DbMigration), newBaseTypeName, (int)vsFindOptions.vsFindOptionsFromStart);
        }

        protected void AddMissingUsing(TextDocument textDocument, string missingNamespace)
        {
            textDocument.Selection.EndOfDocument();
            textDocument.Selection.FindText("using", (int)vsFindOptions.vsFindOptionsBackwards);
            textDocument.Selection.LineDown();

            textDocument.Selection.Insert($"using {missingNamespace};");

            textDocument.Selection.NewLine();
        }

        protected void RemoveCurrentLine(TextDocument textDocument, int numberOfLines = 1)
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                textDocument.Selection.SelectLine();
                textDocument.Selection.Insert(string.Empty);
            }
        }
    }
}
