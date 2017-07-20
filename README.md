# EntityFrameworkViewMigrations

This NuGet package will add support for the actualization of views and procedures to the Entity Framework.
https://www.nuget.org/packages/EntityFrameworkViewMigrations

Version 0.0.6.0 finally provides basic functionality!

Requirements:

- Entity Framework 6.1 with enabled code first migrations
- Database project inside the solution
- Set up app.config/web.config

PowerShell commands: (can be run from package manager console):

- Add-ViewMigration Name -ViewName MyAwesomeView
- Add-ModelChangeOnlyMigration MyEmptyMigration

C# Migration code:

- DatabaseSqlFile("SqlFileInsideDatabaseProject")
- DatabaseSqlFileUp(); // runs Up.sql
- DatabaseSqlFileDown(); // runs Down.sql
- Seed();


To get more information about the nuget package I would recommend to use the sample project
https://github.com/magiak/EntityFrameworkViewMigrationsSample

Can't wait to implement v.0.1.0.0! 
