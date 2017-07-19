namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class InitialDataParser
    {
        private static string fileGroup = "filegroup";

        public static IEnumerable<string> Parse()
        {
            return ParseInitialDataSql()
                .Cast<Match>()
                .Select(match => DbMigrationPath.CombineAndReadAll(DbMigrationPath.InitialDataFolderPath, match.Groups[fileGroup].Value))
                .SelectMany(SqlDataParser.SplitByGoStatements)
                .Where(sql => !string.IsNullOrWhiteSpace(sql));
        }

        private static MatchCollection ParseInitialDataSql()
        {
            return Regex.Matches(
                File.ReadAllText(DbMigrationPath.InitialDataSqlPath),
                $@"^\s*:r\s+(?<{fileGroup}>(.+)\.sql)\s*$",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        }
    }
}
