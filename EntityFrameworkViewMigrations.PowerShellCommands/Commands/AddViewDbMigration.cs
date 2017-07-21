
namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    using EnvDTE;
    using System;
    using System.Configuration;
    using System.Linq;
    using Base;
    using Configuration;
    using Enums;
    using Migrations.Base;

    public class AddViewDbMigration : PowerShellCommand
    {
        public AddViewDbMigration(object dte, object project) : base(dte, project)
        {
        }

        public string SqlViewName { get; set; }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(this.SqlViewName))
            {
                throw new ArgumentNullException(nameof(this.SqlViewName));
            }

            var textDocument = this.GetActiveDocument();

            this.ChangeBaseType(textDocument, nameof(BaseDbMigration));
            this.AddMissingUsing(textDocument, typeof(BaseDbMigration).Namespace);
            this.InsertDatabaseSqlFileCommands(textDocument);
            this.CreateFilesInMigrationsFolder();
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

        // TODO change to private
        public void CreateFilesInMigrationsFolder()
        {
            System.Diagnostics.Debugger.Launch();

            var configuration = EntityFrameworkViewMigrationsSection.GetSectionFromProject(this.Project);
            var databaseProjectConf = configuration.DatabaseProject;

            var databaseProject = this.Dte2.Solution.Projects.Cast<Project>()
                .FirstOrDefault(x => x.Name == databaseProjectConf.ProjectName);

            if (databaseProject == null)
            {
                throw new ConfigurationErrorsException($"Unable to find a project {databaseProjectConf.ProjectName}");
            }

            var migrationsFolder = databaseProject.ProjectItems.Cast<ProjectItem>()
                .FirstOrDefault(x => x.Name == databaseProjectConf.MigrationsFolderName);

            if (migrationsFolder == null)
            {
                throw new ConfigurationErrorsException($"Unable to find a folder {databaseProjectConf.MigrationsFolderName} inside root of project {databaseProjectConf.ProjectName}");
            }

            string folderName = this.GetMigrationFolderName();
            var folder = databaseProject.ProjectItems.AddFolder(folderName);
            this.CreateFile(folder, $"{this.SqlViewName}Up.sql", "Empty up"); // TODO better way to create file name
            this.CreateFile(folder, $"{this.SqlViewName}Down.sql", "Empty down");
        }

        private string GetMigrationFolderName()
        {
            // Not the best but get it from the current opened file
            return this.Dte2.ActiveDocument.Name;
        }

        private void CreateFile(ProjectItem folder, string fileNameWithExtension, string content)
        {
            string folderPath = folder.FileNames[0];

            System.IO.File.AppendAllText(System.IO.Path.Combine(folderPath, fileNameWithExtension), content);
            var fileFullNames = System.IO.Directory.GetFiles(folderPath);

            foreach (string fileFullName in fileFullNames)
            {
                folder.ProjectItems.AddFromFile(fileFullName);
            }
        }
    }
}
