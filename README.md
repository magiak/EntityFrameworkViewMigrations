# EntityFrameworkViewMigrations

This nuget package will add support for views migrations to Entity Framework.
https://www.nuget.org/packages/EntityFrameworkViewMigrations

Version 0.0.6.0 finally provides basic functionality!

Requirements:

- Entity Framework 6.1
- Database project inside solution
- Set up app.config/web.config

PowerShell commands: (you can run from package manager console):

- Add-ViewMigration Name -ViewName MyAwesomeView
- Add-ModelChangeOnlyMigration MyEmptyMigration

C# Migration code:

- DatabaseSqlFile("SqlFileInsideDatabaseProject")
- DatabaseSqlFileUp(); // runs Up.sql
- DatabaseSqlFileDown(); // runs Down.sql
- Seed();


To learn more about the nuget package i have to recommend sample project
https://github.com/magiak/EntityFrameworkViewMigrationsSample

Can't wait to implement v.0.1.0.0! 
