namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class InitialDataParser
    {
        private static string fileGroup = "filegroup";
        private readonly DbMigrationPath dbMigrationPath;

        public InitialDataParser(DbMigrationPath dbMigrationPath)
        {
            this.dbMigrationPath = dbMigrationPath;
        }

        public IEnumerable<string> Parse()
        {
            return ParseInitialDataSql()
                .Cast<Match>()
                .Select(match => DbMigrationPath.CombineAndReadAll(dbMigrationPath.InitialDataFolderPath, match.Groups[fileGroup].Value))
                .SelectMany(SqlDataParser.SplitByGoStatements)
                .Where(sql => !string.IsNullOrWhiteSpace(sql));
        }

        private MatchCollection ParseInitialDataSql()
        {
            return Regex.Matches(
                File.ReadAllText(dbMigrationPath.InitialDataSqlPath),
                $@"^\s*:r\s+(?<{fileGroup}>(.+)\.sql)\s*$",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        }
    }
}
