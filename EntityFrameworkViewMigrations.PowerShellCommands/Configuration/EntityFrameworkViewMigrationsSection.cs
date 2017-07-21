namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System.Configuration;
    using System.Text.RegularExpressions;
    using System.Xml;
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

        public static EntityFrameworkViewMigrationsSection GetSectionFromProject(Project project)
        {
            string configurationFilePath = FindConfigurationFilename(project);
            
            if (!string.IsNullOrEmpty(configurationFilePath))
            {
                // TODO this is just horrible but i really dont know how to read the app config :(
                //"C:\\Users\\lukas\\Source\\Repos\\EntityFrameworkViewMigration.Sample\\EntityFrameworkViewMigration.Models\\App.config"
                //configurationFilePath =
                //    configurationFilePath.Remove(configurationFilePath.Length - "App.config".Length)
                //    + "bin\\Debug\\";

                // found it, map it and expose salient members as properties
                //ExeConfigurationFileMap configFile = new ExeConfigurationFileMap { ExeConfigFilename = configurationFilePath + $"{project.Name}.config" };
                //.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None)

                //var result = (EntityFrameworkViewMigrationsSection)System.Configuration.ConfigurationManager
                //    .OpenExeConfiguration(configurationFilePath + $"{project.Name}.dll")
                //    .GetSection(EntityFrameworkViewMigrationsSectionName);

                XmlDocument config = new XmlDocument();
                config.Load(configurationFilePath);
                var databaseProject = config.DocumentElement.SelectSingleNode("/configuration/entityFrameworkViewMigrations/databaseProject");

                var result = new EntityFrameworkViewMigrationsSection()
                {
                    DatabaseProject = new DatabaseProjectConfigurationElement()
                    {
                        // TODO all conf or use xml serialization :)
                        ProjectName = databaseProject.Attributes[DatabaseProjectConfigurationElement.ProjectNameKey].Value,
                        MigrationsFolderName = databaseProject.Attributes[DatabaseProjectConfigurationElement.MigrationsFolderNameKey].Value,
                    }
                };

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
