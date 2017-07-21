namespace EntityFrameworkViewMigrations.PowerShellCommands.Migrations
{
    using Base;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.Utilities;

    /// <summary>
    /// Code generator class that modifies the behaviour of
    /// migration scaffolding
    /// </summary>
    public class ViewMigrationsCodeGenerator : CSharpMigrationCodeGenerator
    {
        /// <summary>
        /// Generates a namespace, using statements and class definition.
        /// If the base class of the scaffolded migration should be <see cref="DbMigration"/>
        /// (which is the default EF migration base class),
        /// we override it with <see cref="BaseDbMigration"/> class. We also have to
        /// overwrite the generated usings, so that the result derived class
        /// is scaffoleded without any errors.
        /// </summary>
        /// <param name="namespace">Namespace that code should be generated in</param>
        /// <param name="className">Name of the class that should be generated</param>
        /// <param name="writer">Text writer to add the generated code to</param>
        /// <param name="base">Base class for the generated class</param>
        /// <param name="designer">A value indicating if this class is being generated for a code-behind file</param>
        /// <param name="namespaces">
        /// Namespaces for which using directives will be added. If null, then the namespaces returned
        /// from GetDefaultNamespaces will be used.
        /// </param>
        protected override void WriteClassStart(
            string @namespace,
            string className,
            IndentedTextWriter writer,
            string @base,
            bool designer = false,
            IEnumerable<string> namespaces = null)
        {
            // override EF DbMigration to our DbMigration
            // this method might be also used in other parts of
            // the scaffolding process, thus we have to use
            // the if statement to only modify the scaffolding process
            // in this particular case
            if (@base == typeof(DbMigration).Name)
            {
                // set base class of the scaffolded migration class
                // to our BaseDbMigration
                var type = typeof(BaseDbMigration);
                @base = type.Name;

                // override generated usings to contain namespace
                // of our BaesDbMigration class, that is necessary
                // to scaffold the migration without errors
                namespaces = new[] { type.Namespace };
            }

            base.WriteClassStart(@namespace, className, writer, @base, designer, namespaces);
        }
    }
}
