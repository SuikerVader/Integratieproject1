using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Areas.Identity;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.IdentityModel.Tokens;
using Platform = Integratieproject1.Domain.Projects.Platform;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.DAL
{
    public sealed class CityOfIdeasDbContext : IdentityDbContext<IdentityUser>
    {
        public CityOfIdeasDbContext()
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            CityOfIdeasDbInitializer.Initialize(this, true);
            Console.WriteLine("dbContext constructor");
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Ideation> Ideations { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Like> Likes { get; set; }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<IoTSetup> IoTSetups { get; set; }

        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<LoggedInUser> LoggedInUsers { get; set; }
        public DbSet<AdminProject> AdminProjects { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite("Data Source=CityOfIdeas.db");
            optionsBuilder.UseLoggerFactory(new LoggerFactory(
                providers: new[]
                {
                    new DebugLoggerProvider(
                        (category, level) => category == DbLoggerCategory.Database.Command.Name &&
                                             level == LogLevel.Information),
                }));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            if (_delaySave)
                return -1;

            Helper.PrintDbContextTrackedEntitiesStates(this,
                "STATES BEFORE correction of EntityState's ('Added' to 'Unchanged' if has (existing) PK!");

            foreach (EntityEntry e in ChangeTracker.Entries().ToList())
            {
                if (e.State == EntityState.Added && e.IsKeySet)
                {
                    //'temporary values'? => e.IsKeySet (=> PK-waarde != default(Type)) is altijd 'true', want gebruikt 'temporary values' (gelijk aan +/-Type.MinValue)
                    foreach (PropertyEntry p in e.Properties)
                    {
                        if (p.Metadata.IsKey() && !p.IsTemporary)
                        {
                            e.State = EntityState.Unchanged;
                        }
                    }
                }
            }

            Helper.PrintDbContextTrackedEntitiesStates(this,
                "STATES AFTER correction of EntityState's ('Added' to 'Unchanged' if has (existing) PK!");

            //'SaveChanges' aanroepen op base-type!
            Helper.PrintDbContextTrackedEntitiesStates(this, "STATES BEFORE SaveChanges() (TO DB)");

            int infectedRecords = base.SaveChanges();

            Helper.PrintDbContextTrackedEntitiesStates(this, "STATES AFTER SaveChanges() (TO DB)");

            return infectedRecords;
        }

        private readonly bool _delaySave = false;

        public CityOfIdeasDbContext(bool isUnitOfWorkPresent)
            : this()
        {
            _delaySave = isUnitOfWorkPresent;
        }


        internal int CommitChanges()
        {
            if (_delaySave)
            {
                Helper.PrintDbContextTrackedEntitiesStates(this, "STATES BEFORE CommitChanges() (TO DB)");

                int infectedRecords = base.SaveChanges();

                Helper.PrintDbContextTrackedEntitiesStates(this, "STATES AFTER CommitChanges() (TO DB)");

                return infectedRecords;
            }

            throw new InvalidOperationException("No UnitOfWork present, use SaveChanges instead");
        }
    }
}