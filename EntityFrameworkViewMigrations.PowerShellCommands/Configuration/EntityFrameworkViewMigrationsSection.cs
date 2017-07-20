namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System.Configuration;

    public class EntityFrameworkViewMigrationsSection : ConfigurationSection
    {
        private const string DatabaseProjectKey = "databaseProject";

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
    }
}
