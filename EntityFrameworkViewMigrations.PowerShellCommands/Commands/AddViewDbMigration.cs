namespace EntityFrameworkViewMigrations.PowerShellCommands.Commands
{
    using EnvDTE;
    using System;
    using System.Configuration;
    using System.IO;
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
            var configuration = EntityFrameworkViewMigrationsSection.GetSectionFromProject(this.Project);
            var dbConfiguration = configuration.DatabaseProject;

            var databaseProject = this.Dte2.Solution.Projects.Cast<Project>()
                .FirstOrDefault(x => x.Name == dbConfiguration.ProjectName);

            if (databaseProject == null)
            {
                throw new ConfigurationErrorsException($"Unable to find a project {dbConfiguration.ProjectName}");
            }

            var migrationsFolder = databaseProject.ProjectItems.Cast<ProjectItem>()
                .FirstOrDefault(x => x.Name == dbConfiguration.MigrationsFolderName);

            if (migrationsFolder == null)
            {
                throw new ConfigurationErrorsException($"Unable to find a folder {dbConfiguration.MigrationsFolderName} inside root of project {dbConfiguration.ProjectName}");
            }

            string folderName = this.GetMigrationFolderName();
            var folder = migrationsFolder.ProjectItems.AddFolder(folderName);

            string upFileContent = this.GetCurrentViewDefinition(databaseProject, dbConfiguration);
            string downFileContent = $"DROP VIEW[dbo].[{this.SqlViewName}]";

            if (folderName.IndexOf("Alter", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                upFileContent = upFileContent.Replace("CREATE VIEW", "ALTER VIEW");
                downFileContent = upFileContent;
            }

            // TODO NTH DROP 

            this.CreateFile(folder, $"{this.SqlViewName}Up.sql", upFileContent); // TODO better way to create file name
            this.CreateFile(folder, $"{this.SqlViewName}Down.sql", downFileContent);
        }

        private string GetMigrationFolderName()
        {
            return Path.GetFileNameWithoutExtension(this.GetActiveDocumentNameWithExtension());
        }

        private string GetActiveDocumentNameWithExtension()
        {
            return this.Dte2.ActiveDocument.Name;
        }

        private string GetCurrentViewDefinition(Project databaseProject, DatabaseProjectConfigurationElement dbConfiguration)
        {
            string projectPath = databaseProject.Properties.Cast<Property>().FirstOrDefault(x => x.Name == "FullPath")?.Value.ToString();
            if (projectPath != null)
            {
                string[] allFiles = Directory.GetFiles($@"{projectPath}\", "*", SearchOption.AllDirectories)
                    .Where(x => !x.Contains($@"{projectPath}\{dbConfiguration.MigrationsFolderName}\")) // Exclude the folder with migrations
                    .Where(x => !x.Contains($@"{projectPath}\{dbConfiguration.SeedFolderName}\")) // Exclude the folder with initil data
                    .ToArray();
                var sqlFilePath = allFiles.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == this.SqlViewName);
                if (sqlFilePath != null)
                {
                    return File.ReadAllText(sqlFilePath);
                }
            }

            return string.Empty;
        }

        private void CreateFile(ProjectItem folder, string fileNameWithExtension, string content)
        {
            string folderPath = folder.FileNames[0];

            File.AppendAllText(Path.Combine(folderPath, fileNameWithExtension), content);
            var fileFullNames = Directory.GetFiles(folderPath);

            foreach (string fileFullName in fileFullNames)
            {
                folder.ProjectItems.AddFromFile(fileFullName);
            }
        }
    }
}
