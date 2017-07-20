
namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System;
    using System.IO;
    using System.Configuration;

    /// <summary>
    /// Handles path operations that are relative to
    /// the Model project assembly after build.
    /// We assume that the location of the assembly is
    /// "ProjectFolder\bin\BuildConfigurationFolder", e.g.
    /// Model\bin\Debug
    /// </summary>
    public static class DbMigrationPathConfiguration
    {
        //public DbMigrationPathConfiguration()
        //{
        //    ConfigurationManager.GetSection()
        //}

        /// <summary>
        /// Gets location of the Database project relative to this assembly
        /// after build
        /// </summary>
        public static string DbProjectPath { get; } = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\Database\");

        /// <summary>
        ///  Gets location of initial data folder in the Database project relative to this assembly
        ///  after build
        /// </summary>
        public static string InitialDataFolderPath { get; } = Path.Combine(DbProjectPath, "InitialData");

        /// <summary>
        /// Gets location of initial data sql file in theDatabase project relative to this assembly
        /// after build
        /// </summary>
        public static string InitialDataSqlPath { get; } = Path.Combine(InitialDataFolderPath, "InitialData.sql");

        /// <summary>
        /// Gets location of Migrations folder in the Database project relative to this assembly
        /// after build
        /// </summary>
        public static string MigrationsFolderPath { get; } = Path.Combine(DbProjectPath, "Migrations");

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
