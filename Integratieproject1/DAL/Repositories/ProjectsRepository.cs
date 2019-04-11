using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly CityOfIdeasDbContext _ctx;

        public ProjectsRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _ctx = unitOfWork.Ctx;
        }

        #region Platform

        public IEnumerable<Platform> GetPlatforms()
        {
            return _ctx.Platforms.AsEnumerable();
        }

        public Platform GetPlatform(int platformId)
        {
            return _ctx.Platforms
                .Include(pl => pl.Projects).ThenInclude(ph => ph.Phases)
                .Single(pl => pl.PlatformId == platformId);
        }

        public Platform CreatePlatform(Platform platform)
        {
            _ctx.Platforms.Add(platform);
            _ctx.SaveChanges();
            return platform;
        }

        #endregion

        #region Project

        public IEnumerable<Project> GetProjects(int platformId)
        {
            return _ctx.Projects
                .Where(p => p.Platform.PlatformId == platformId)
                .Include(p => p.Phases).ThenInclude(i => i.Ideations)
                .Include(p => p.Phases).ThenInclude(s => s.Surveys)
                .Include(l => l.Location).ThenInclude(a => a.Address)
                .Include(pl => pl.Platform)
                .AsEnumerable();
        }
        
        public IEnumerable<Project> GetAllProjects()
        {
            return _ctx.Projects
                .Include(l => l.Location).ThenInclude(a => a.Address)
                .Include(p => p.Platform)
                .AsEnumerable();
        }

        public IEnumerable<AdminProject> GetAdminProjects(string userId)
        {
            UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(_ctx);
            IdentityUser identityUser = userStore.FindByIdAsync(userId).Result;
            return _ctx.AdminProjects
                .Where(p => p.Admin == identityUser)
                .Include(p => p.Project).ThenInclude(l => l.Location).ThenInclude(a => a.Address)
                .Include(p => p.Project).ThenInclude(p => p.Platform)
                .AsEnumerable();
        }

        
        public Project GetProject(int projectId)
        {
            return _ctx.Projects
                .Include(p => p.Phases).ThenInclude(i => i.Ideations).ThenInclude(id => id.Ideas)
                .Include(p => p.Phases).ThenInclude(s => s.Surveys)
                .Include(l => l.Location).ThenInclude(a => a.Address)
                .Include(pl => pl.Platform)
                .Single(pr => pr.ProjectId == projectId);
        }

        public Project CreateProject(Project project)
        {
            _ctx.Projects.Add(project);
            Platform platform = GetPlatform(project.Platform.PlatformId);
            /*platform.Projects.Add(project);
            ctx.Platforms.Update(platform);*/
            Console.WriteLine(platform.Projects.First().ToString());
            _ctx.SaveChanges();
            Console.WriteLine(_ctx.Platforms.Find(project.Platform.PlatformId).Projects.Count);
            return project;
        }

        public void EditProject(Project project)
        {
            _ctx.Projects.Update(project);
            _ctx.SaveChanges();
        }

        public AdminProject CreateAdminProject(AdminProject adminProject)
        {
            _ctx.AdminProjects.Add(adminProject);
            Project project = adminProject.Project;
            project.AdminProjects.Add(adminProject);
            _ctx.SaveChanges();
            return adminProject;
        }


        public void RemoveProject(Project project)
        {
            _ctx.Projects.Remove(project);
            _ctx.SaveChanges();
        }

        public AdminProject GetAdminProject(int adminProjectId)
        {
            return _ctx.AdminProjects.Find(adminProjectId);
        }

        public void RemoveAdminProject(AdminProject adminProject)
        {
            _ctx.AdminProjects.Remove(adminProject);
            _ctx.SaveChanges();
        }

        #endregion

        #region Phase

        //PhaseMethods
        public IEnumerable<Phase> GetPhases(int projectId)
        {
            return _ctx.Phases
                .Where(p => p.Project.ProjectId == projectId)
                .Include(i => i.Ideations)
                .Include(s => s.Surveys)
                .AsEnumerable();
        }

        public IEnumerable<Phase> GetAllPhases(int platformId)
        {
            return _ctx.Phases
                .Where(p => p.Project.Platform.PlatformId == platformId)
                .AsEnumerable();

        }

        public Phase GetPhase(int phaseId)
        {
            return _ctx.Phases
                .Include(p => p.Project).ThenInclude(ph => ph.Phases)
                .Include(i => i.Ideations)
                .Include(s => s.Surveys)
                .Single(p => p.PhaseId == phaseId);
        }

        public Phase CreatePhase(Phase phase)
        {
            _ctx.Phases.Add(phase);
            Project project = phase.Project;
            project.Phases.Add(phase);
            _ctx.SaveChanges();
            return phase;
        }
        public void RemovePhase(Phase phase)
        {
            _ctx.Phases.Remove(phase);
            _ctx.SaveChanges();
        }
        public Phase EditPhase(Phase phase)
        {
            _ctx.Phases.Update(phase);
            _ctx.SaveChanges();
            return _ctx.Phases.Find(phase.PhaseId);
        }


        #endregion     
    }
}