using System;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;

namespace Integratieproject1.BL.Managers
{
    public class ProjectsManager
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
         public void CreateProject(Project project)
                {
                    projectsRepository.CreateProject(project);
                    unitOfWorkManager.Save();
                }
        #endregion

        #region Phase

        public Phase GetPhase(int phaseId)
        {
            return projectsRepository.GetPhase(phaseId);
        }

        public void CreatePhase(Phase phase)
        {
            projectsRepository.CreatePhase(phase);
            unitOfWorkManager.Save();
        }
        

        #endregion
        


        

        
        
       
    }
}