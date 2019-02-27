using Integratieproject1.Models;
using Integratieproject1.Models.Ideations;
using Integratieproject1.Models.IoT;
using Integratieproject1.Models.Projects;
using Integratieproject1.Models.Surveys;
using Integratieproject1.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.DAL
{
    public class CityOfIdeasDbContext : DbContext
    {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=CityOfIdeas.db");
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Phase> Phases { get; set; }
    public DbSet<Platform> Platforms { get; set; }

    public DbSet<Ideation> Ideations { get; set; }
    public DbSet<Idea> Ideas { get; set; }
    public DbSet<Reaction> Reactions { get; set; }
    public DbSet<Vote> Votes { get; set; }

    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }

    public DbSet<IoTSetup> IoTSetups { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<LoggedInUser> LoggedInUsers { get; set; }

  }
}
