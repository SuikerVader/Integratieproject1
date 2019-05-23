using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Integratieproject1.BL.Interfaces
{
    public interface IProjectsManager
    {
        #region Platform

        Platform GetPlatform(int platformId);
        Platform GetPlatformByName(string platformName);
        IList<Platform> GetAllPlatforms();
        IList<Platform> GetAllPlatformsBySort(string sortOrder);
        void CreatePlatform(Platform platform);
        void EditPlatform(Platform platform, int platformId);
        void EditPlatformLayout(Platform platform, int platformId);
        void DeletePlatform(int platformId);
        void DeleteBackgroundImagePlatform(int platformId);
        void DeleteLogoPlatform(int platformId);

        #endregion

        #region Project

        Project GetProject(int projectId);
        AdminProject GetAdminProject(int adminProjectId);
        IList<Project> GetProjects(int platformId);
        IList<Project> GetAllProjects();
        IList<Project> GetAllProjectsBySort(string sortOrder);
        IList<Project> GetAdminProjects(string userId);
        IList<AdminProject> GetAllAdminProjects(string userId);
        IList<Project> GetAdminProjectsBySort(string userId, string sortOrder);
        IList<AdminProject> GetAllAdminProjectsBySort(string userId, string sortOrder);
        IList<CustomUser> GetNotProjectAdmins(int projectId);
        IList<CustomUser> GetNotProjectAdminsBySort(int projectId, string sortOrder);
        void CreateProject(Project project, string id, int platformId);
        void CreateAdminProject(int projectId, string adminId);
        Project EditProject(Project project, int projectId);
        void DeleteProject(int projectId);
        void DeleteAdminProject(int adminProjectId);
        void DeleteBackgroundImageProject(int projectId);

        #endregion

        #region Phase

        Phase GetPhase(int phaseId);
        Phase GetNewPhase(int projectId);
        IList<Phase> GetPhases(int projectId);
        IList<Phase> GetAllPhases(int platformId);
        Phase CreatePhase(Phase phase, int phaseNr, int projectId);
        Phase EditPhase(Phase phase, int phaseId);
        void DeletePhase(int phaseId);

        #endregion
    }
}