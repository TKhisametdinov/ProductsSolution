using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;

namespace Products.WebApi.DAL
{
    /// <summary>
    /// A custom database initializer used to initialize the underlying database when
    /// an instance of a <see cref="T:System.Data.Entity.DbContext" /> derived class is used for the first time.
    /// This initialization seeds database only when migrations are applied.
    /// </summary>
    /// <typeparam name="TContext">Database context</typeparam>
    /// <typeparam name="TMigrationsConfiguration">сonfiguration relating to the use of migrations for a given model</typeparam>
    public class CheckAndMigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        public virtual void InitializeDatabase(TContext context)
        {
            var migratorBase = (MigratorBase)new DbMigrator(Activator.CreateInstance<TMigrationsConfiguration>());
            // update and seed only when we have migrations pending
            if (migratorBase.GetPendingMigrations().Any())
                migratorBase.Update();
        }
    }
}
