using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Models.Projects;

namespace Integratieproject1.DAL.Repositorys
{
    public class ProjectsRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public ProjectsRepository( CityOfIdeasDbContext dbContext)
        {
            ctx = dbContext ;
            //CityOfIdeasDbInitializer.Initialize(ctx, false);
        }
        
        // Platform methods
        public IEnumerable<Platform> GetPlatforms()
        {
            return ctx.Platforms.AsEnumerable();
        }
        public Platform GetPlatform(int platformId)
        {
            return ctx.Platforms.Find(platformId);
        }
        public Platform CreatePlatform(Platform platform)
        {
            ctx.Platforms.Add(platform);
            ctx.SaveChanges();
            return platform;
        }
        
        //ProjectMethods
        public IEnumerable<Project> GetProjects()
        {
            return ctx.Projects.AsEnumerable();
        }
        public Project GetProject(int projectId)
        {
            return ctx.Projects.Find(projectId);
        }
        public Project CreateProject(Project project)
        {
            ctx.Projects.Add(project);
            Platform platform = GetPlatform(project.Platform.PlatformId) ;
            /*platform.Projects.Add(project);
            ctx.Platforms.Update(platform);*/
            Console.WriteLine(platform.Projects.First().ToString());
            ctx.SaveChanges();
            Console.WriteLine(ctx.Platforms.Find(project.Platform.PlatformId).Projects.Count);
            return project;
        }
        
        //PhaseMethods
        public IEnumerable<Phase> GetPhases()
        {
            return ctx.Phases.AsEnumerable();
        }
        public Phase GetPhase(int phaseId)
        {
            return ctx.Phases.Find(phaseId);
        }
        public Phase CreatePhase(Phase phase)
        {
            ctx.Phases.Add(phase);
            Project project = phase.Project;
            project.Phases.Add(phase);
            ctx.SaveChanges();
            return phase;
        }
    }
}