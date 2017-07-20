namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System;
    using System.IO;

    /// <summary>
    /// Handles path operations that are relative to
    /// the Model project assembly after build.
    /// We assume that the location of the assembly is
    /// "ProjectFolder\bin\BuildConfigurationFolder", e.g.
    /// Model\bin\Debug
    /// </summary>
    public class DbMigrationPath
    {
        private readonly DatabaseProjectConfigurationElement databaseProject;

        public DbMigrationPath(DatabaseProjectConfigurationElement databaseProject)
        {
            this.databaseProject = databaseProject;
        }

        /// <summary>
        /// Gets location of the Database project relative to this assembly
        /// after build
        /// </summary>
        public string DbProjectPath => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            $@"..\..\..\{databaseProject.ProjectName}\");

        /// <summary>
        ///  Gets location of initial data folder in the Database project relative to this assembly
        ///  after build
        /// </summary>
        public string InitialDataFolderPath => Path.Combine(DbProjectPath, databaseProject.SeedFolderName);

        /// <summary>
        /// Gets location of initial data sql file in theDatabase project relative to this assembly
        /// after build
        /// </summary>
        public string InitialDataSqlPath => Path.Combine(InitialDataFolderPath, databaseProject.SeedFileName);

        /// <summary>
        /// Gets location of Migrations folder in the Database project relative to this assembly
        /// after build
        /// </summary>
        public string MigrationsFolderPath => Path.Combine(DbProjectPath, databaseProject.MigrationsFolderName);

        /// <summary>
        /// Combines path parts to a single path and then reads
        /// all text from that path
        /// </summary>
        /// <param name="paths">path parts to combine</param>
        /// <returns>String content of a file identified by the paths parts</returns>
        public static string CombineAndReadAll(params string[] paths)
        {
            return File.ReadAllText(Path.Combine(paths));
        }
    }
}
