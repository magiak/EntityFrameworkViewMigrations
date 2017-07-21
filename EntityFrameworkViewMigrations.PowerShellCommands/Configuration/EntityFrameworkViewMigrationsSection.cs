namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using EnvDTE;

    public class EntityFrameworkViewMigrationsSection : ConfigurationSection
    {
        private const string DatabaseProjectKey = "databaseProject";
        private const string EntityFrameworkViewMigrationsSectionName = "entityFrameworkViewMigrations";

        [ConfigurationProperty(DatabaseProjectKey, IsRequired = true)]
        public DatabaseProjectConfigurationElement DatabaseProject
        {
            get
            {
                return (DatabaseProjectConfigurationElement)this[DatabaseProjectKey];
            }

            set
            {
                this[DatabaseProjectKey] = value;
            }
        }

        public static EntityFrameworkViewMigrationsSection GetSectionFromCurrentAssembly()
        {
            // This is HACK 
            var assemblyPath = Assembly.GetCallingAssembly().EscapedCodeBase;
            assemblyPath = assemblyPath.Substring("file:///".Length); // Remove file:///

            return (EntityFrameworkViewMigrationsSection) System.Configuration.ConfigurationManager
                .OpenExeConfiguration(assemblyPath)
                .GetSection(EntityFrameworkViewMigrationsSectionName);
        }

        public static EntityFrameworkViewMigrationsSection GetSectionFromProject(Project project)
        {
            string configurationFilePath = FindConfigurationFilename(project);

            // Return the configuration object if we have a configuration file name
            // If we do not have a configuration file name, throw an exception
            if(!string.IsNullOrEmpty(configurationFilePath))
            {
                // found it, map it and expose salient members as properties
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap { ExeConfigFilename = configurationFilePath };

                var result = (EntityFrameworkViewMigrationsSection)System.Configuration.ConfigurationManager
                    .OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None)
                    .GetSection(EntityFrameworkViewMigrationsSectionName);

                return result;
            }

            throw new ConfigurationErrorsException("Unable to find a configuration file (web.config/app.config). If the config file is located in a different project, you must mark that project as either the Startup Project or pass the project location of the config file relative to the solution file.");
        }

        // Copy from https://stackoverflow.com/questions/25460348/how-to-make-connection-strings-available-in-a-t4-template
        private static string FindConfigurationFilename(Project project)
        {
            // examine each project item's filename looking for app.config or web.config
            foreach (ProjectItem item in project.ProjectItems)
            {
                if (Regex.IsMatch(item.Name, "(app|web).config", RegexOptions.IgnoreCase))
                {
                    return item.FileNames[0];
                }
            }

            // not found, return null
            return null;
        }
    }
}
