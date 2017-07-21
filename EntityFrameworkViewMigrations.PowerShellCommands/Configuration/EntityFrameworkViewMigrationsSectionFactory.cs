namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System;
    using System.Reflection;

    public class EntityFrameworkViewMigrationsSectionFactory
    {
        private const string EntityFrameworkViewMigrationsSectionName = "entityFrameworkViewMigrations";

        public EntityFrameworkViewMigrationsSection GetSectionFromCurrentAssembly(string assemblyPath)
        {
            // This is HACK
            assemblyPath = assemblyPath.Substring("file:///".Length); // Remove file:///

            var configurationSection = (EntityFrameworkViewMigrationsSection)System.Configuration.ConfigurationManager
                .OpenExeConfiguration(assemblyPath)
                .GetSection(EntityFrameworkViewMigrationsSectionName);

            if (configurationSection == null)
            {
                throw new Exception($"Can not find config file with entityFrameworkViewMigrations section at location {assemblyPath}");
            }

            return configurationSection;
        }
    }
}
