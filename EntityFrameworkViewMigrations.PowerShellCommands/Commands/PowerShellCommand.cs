using EnvDTE;
using EnvDTE80;

namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    public abstract class PowerShellCommand : IPowerShellCommand
    {
        public DTE2 Dte2 { get; set; }

        public PowerShellCommand()
        {
            this.Dte2 = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.14.0");
        }
        
        public abstract void Execute();

        public TextDocument GetActiveDocument()
        {
            return (TextDocument)this.Dte2.ActiveDocument.Object(string.Empty);
        }
    }
}
