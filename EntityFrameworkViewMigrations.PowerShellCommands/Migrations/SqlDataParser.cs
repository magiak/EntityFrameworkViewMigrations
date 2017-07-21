namespace EntityFrameworkViewMigrations.PowerShellCommands.Configuration
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class SqlDataParser
    {
        public static string WrapSqlFileWithExec(string sql)
        {
            var parsed = SplitByGoStatements(sql)
                .Select(WrapSqlWithExec).ToArray();

            return string.Join(" ", parsed);
        }

        public static string[] SplitByGoStatements(string sql)
        {
            return Regex.Split(
                sql,
                @"^\s*GO\s*\d*\s*($|\-\-.*$)",
                RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToArray();
        }

        private static string WrapSqlWithExec(string sql)
        {
            return "EXECUTE('" + sql.Replace("'", "''") + "');";
        }
    }
}
