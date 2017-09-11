namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands.Base
{
    using System;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Configuration;
    using EnvDTE;
    using EnvDTE80;

    public abstract class PowerShellCommand : IPowerShellCommand
    {
        private const int NumberOfUpAndDownSourceLines = 7;

        public DTE2 Dte2 { get; set; }

        public Project Project { get; set; }

        public Project StartUpProject { get; }

        public Project DatabaseProject { get; }

        public EntityFrameworkViewMigrationsSection Configuration { get; }

        public DatabaseProjectConfigurationElement DatabaseConfiguration { get; }

        protected PowerShellCommand(object dte, object project)
        {
            var dteRef = dte as DTE2;
            if (dteRef != null)
            {
                this.Dte2 = dteRef;
            }
            else
            {
                // TODO Maybe better: (DTE)ServiceProvider.GetService(typeof(DTE));
                this.Dte2 = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.14.0");
            }

            this.Project = project as Project;

            this.Configuration = this.GetConfiguration();
            this.DatabaseConfiguration = this.GetDatabaseConfiguration(this.Configuration);

            this.StartUpProject = this.GetStartUpProject();
            this.DatabaseProject = this.GetDatabaseProject();
        }
        
        public abstract void Execute();

        protected TextDocument GetActiveDocument()
        {
            return (TextDocument)this.Dte2.ActiveDocument.Object(string.Empty);
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

        protected void RemoveUpAndDownFunctions(TextDocument textDocument)
        {
            // Find line with UP
            textDocument.Selection.StartOfDocument();
            textDocument.Selection.FindText("Up()");

            this.RemoveCurrentLine(textDocument, NumberOfUpAndDownSourceLines);
        }

        protected void RemoveCurrentLine(TextDocument textDocument, int numberOfLines = 1)
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                textDocument.Selection.SelectLine();
                textDocument.Selection.Insert(string.Empty);
            }
        }

        private EntityFrameworkViewMigrationsSection GetConfiguration()
        {
            return EntityFrameworkViewMigrationsSection.GetSectionFromProject(this.Project);
        }

        private DatabaseProjectConfigurationElement GetDatabaseConfiguration(EntityFrameworkViewMigrationsSection configuration)
        {
            return configuration.DatabaseProject;
        }

        private Project GetDatabaseProject()
        {
            var databaseProject = this.Dte2.Solution.Projects.Cast<Project>()
                .FirstOrDefault(x => x.Name == this.DatabaseConfiguration.ProjectName);

            if (databaseProject == null)
            {
                throw new ConfigurationErrorsException($"Unable to find a project {this.DatabaseConfiguration.ProjectName}");
            }

            return databaseProject;
        }

        private Project GetStartUpProject()
        {
            foreach (string item in (Array)this.Dte2.Solution.SolutionBuild.StartupProjects)
            {
                return this.Dte2.Solution.Item(item);
            }

            throw new ConfigurationErrorsException("There is not startup project selected");
        }
    }
}
