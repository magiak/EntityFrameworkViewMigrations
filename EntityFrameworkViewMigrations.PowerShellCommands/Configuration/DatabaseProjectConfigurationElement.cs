namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System.Configuration;

    public class DatabaseProjectConfigurationElement : ConfigurationElement
    {
        private const string ProjectNameKey = "projectName";
        private const string MigrationsFolderNameKey = "migrationsFolderName";
        private const string SeedFolderNameKey = "seedFolderName";
        private const string SeedFileNameKey = "seedFileName";

        [ConfigurationProperty(ProjectNameKey, IsRequired = true)]
        public string ProjectName
        {
            get
            {
                return (string)this[ProjectNameKey];
            }

            set
            {
                this[ProjectNameKey] = value;
            }
        }

        [ConfigurationProperty(MigrationsFolderNameKey, IsRequired = true)]
        public string MigrationsFolderName
        {
            get
            {
                return (string)this[MigrationsFolderNameKey];
            }

            set
            {
                this[MigrationsFolderNameKey] = value;
            }
        }

        [ConfigurationProperty(SeedFolderNameKey, IsRequired = true)]
        public string SeedFolderName
        {
            get
            {
                return (string)this[SeedFolderNameKey];
            }

            set
            {
                this[SeedFolderNameKey] = value;
            }
        }

        [ConfigurationProperty(SeedFileNameKey, IsRequired = true)]
        public string SeedFileName
        {
            get
            {
                return (string)this[SeedFileNameKey];
            }

            set
            {
                this[SeedFileNameKey] = value;
            }
        }
    }
}
