using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.BL.Managers
{
    public class ProjectsManager : IProjectsManager
    {
        private ProjectsRepository projectsRepository;
        private UnitOfWorkManager unitOfWorkManager;

        public ProjectsManager()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            projectsRepository = new ProjectsRepository(unitOfWorkManager.UnitOfWork);
        }
        public ProjectsManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException("unitOfWorkManager");

            this.unitOfWorkManager = unitOfWorkManager;
            this.projectsRepository = new ProjectsRepository(unitOfWorkManager.UnitOfWork);
        }

        #region Platform


        public Platform GetPlatform(int platformId)
                {
                    return projectsRepository.GetPlatform(platformId);
                }
        public void CreatePlatform(Platform platform)
                                 {
                                     projectsRepository.CreatePlatform(platform);
                                     unitOfWorkManager.Save();
                                 }

        #endregion

        #region Project

        public Project GetProject(int projectId)
                {
                    return projectsRepository.GetProject(projectId);
                }
        public IList<Project> GetProjects(int userId)
                {
                    UsersManager usersManager = new UsersManager(unitOfWorkManager);
                    User user = usersManager.GetUser(userId);
                    return projectsRepository.GetProjects(user.Platform.PlatformId).ToList();
                }
         public void CreateProject(Project project)
                {
                    projectsRepository.CreateProject(project);
                    unitOfWorkManager.Save();
                }
         public void EditProject(Project project, int projectId, int locationId)
         {
             project.ProjectId = projectId;
             projectsRepository.EditProject(project);
             unitOfWorkManager.Save();
         }
         
        #endregion

        #region Phase

        public Phase GetPhase(int phaseId)
        {
            return projectsRepository.GetPhase(phaseId);
        }
        public IList<Phase> GetPhases(int projectId)
        {
            return projectsRepository.GetPhases(projectId).ToList();
        }
        public void CreatePhase(Phase phase)
        {
            projectsRepository.CreatePhase(phase);
            unitOfWorkManager.Save();
        }
        

        #endregion


        
    }
}