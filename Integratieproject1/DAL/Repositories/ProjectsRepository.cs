using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly CityOfIdeasDbContext ctx;

       
        public ProjectsRepository( UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            ctx = unitOfWork.ctx ;
        }
        
        // Platform methods
        public IEnumerable<Platform> GetPlatforms()
        {
            return ctx.Platforms.AsEnumerable();
        }
        public Platform GetPlatform(int platformId)
        {
            return ctx.Platforms.Include(pl => pl.Projects).Single(pl => pl.PlatformId == platformId);
        }
        public Platform CreatePlatform(Platform platform)
        {
            ctx.Platforms.Add(platform);
            ctx.SaveChanges();
            return platform;
        }
        
        //ProjectMethods
        public IEnumerable<Project> GetProjects(int platformId)
        {
            return ctx.Projects
                .Where(p => p.Platform.PlatformId == platformId)
                .Include(p => p.Phases).ThenInclude(i => i.Ideations)
                .Include(p => p.Phases).ThenInclude(s => s.Surveys)
                .Include(l => l.Location)
                .Include(pl => pl.Platform)
                .AsEnumerable();
        }
        public Project GetProject(int projectId)
        {
            return ctx.Projects
                .Include(p => p.Phases).ThenInclude(i => i.Ideations)
                .Include(p => p.Phases).ThenInclude(s => s.Surveys)
                .Include(l => l.Location)
                .Include(pl => pl.Platform)
                .Single(pr => pr.ProjectId == projectId);
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
        public void EditProject(Project project)
        {
            ctx.Projects.Update(project);
            ctx.SaveChanges();
        }
        
        //PhaseMethods
        public IEnumerable<Phase> GetPhases(int projectId)
        {
            return ctx.Phases
                .Where(p => p.Project.ProjectId == projectId)
                .Include(i => i.Ideations)
                .Include(s => s.Surveys)
                .AsEnumerable();
        }
        public Phase GetPhase(int phaseId)
        {
            return ctx.Phases.Include(i => i.Ideations).Include(s => s.Surveys).Single(p => p.PhaseId == phaseId);
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