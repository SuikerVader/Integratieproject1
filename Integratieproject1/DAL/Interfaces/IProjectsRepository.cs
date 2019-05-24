using System.Collections.Generic;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IProjectsRepository
    {
     
        #region Platform

        IEnumerable<Platform> GetPlatforms();
        Platform GetPlatform(int platformId);
        Platform GetPlatformByName(string platformName);
        Platform CreatePlatform(Platform platform);
        void RemovePlatform(Platform platform);
        void EditPlatform(Platform platform);
        
        #endregion

        #region Project

        IEnumerable<Project> GetProjects(int platformId);
        IEnumerable<Project> GetAllProjects();
        IEnumerable<AdminProject> GetAdminProjectsByUser(string userId);
        IEnumerable<AdminProject> GetAdminProjectsByProject(int projectId);
        Project GetProject(int projectId);
        Project CreateProject(Project project);
        Project EditProject(Project project);
        AdminProject CreateAdminProject(AdminProject adminProject);
        void RemoveProject(Project project);
        AdminProject GetAdminProject(int adminProjectId);
        void RemoveAdminProject(AdminProject adminProject);

        #endregion

        #region Phase

        IEnumerable<Phase> GetPhases(int projectId);
        IEnumerable<Phase> GetAllPhases(int platformId);
        Phase GetPhase(int phaseId);
        Phase CreatePhase(Phase phase);
        void RemovePhase(Phase phase);
        Phase EditPhase(Phase phase);

        #endregion
    }
}