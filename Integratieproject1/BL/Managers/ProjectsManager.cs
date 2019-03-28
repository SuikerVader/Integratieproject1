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

        public IList<Project> GetAdminProjects(int userId)
        {
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            LoggedInUser user = usersManager.GetLoggedInUser(userId);
            List<AdminProject> adminProjects = projectsRepository.GetAdminProjects(user).ToList();
            List<Project> projects = new List<Project>();
            foreach (AdminProject adminProject in adminProjects)
            {
                projects.Add(adminProject.Project);
            }

            return projects;
        }

        public void CreateProject(Project project, int userId)
        {
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            LoggedInUser user = usersManager.GetLoggedInUser(userId);
            project.Platform = GetPlatform(user.Platform.PlatformId);
            DataTypeManager dataTypeManager = new DataTypeManager(unitOfWorkManager);
            project.Location = dataTypeManager.CheckLocation(project.Location);
            //Project createdProject = projectsRepository.CreateProject(project);
            AdminProject adminProject = new AdminProject
            {
                Project = project,
                Admin = user
            };
            projectsRepository.CreateAdminProject(adminProject);
            unitOfWorkManager.Save();
        }

        public void EditProject(Project project, int projectId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(unitOfWorkManager);
            project.Location = dataTypeManager.CheckLocation(project.Location);
            project.ProjectId = projectId;
            projectsRepository.EditProject(project);
            unitOfWorkManager.Save();
        }

        public void DeleteProject(int projectId)
        {
            Project project = projectsRepository.GetProject(projectId);
            if (project.Phases != null)
            {
                foreach (Phase phase in project.Phases.ToList())
                {
                    this.DeletePhase(phase.PhaseId);
                }
            }

            if (project.AdminProjects != null)
            {
                foreach (AdminProject adminProject in project.AdminProjects.ToList())
                {
                    this.DeleteAdminProject(adminProject.AdminProjectId);
                }
            }

            projectsRepository.RemoveProject(project);
            unitOfWorkManager.Save();
        }

        public void DeleteAdminProject(int adminProjectId)
        {
            AdminProject adminProject = projectsRepository.GetAdminProject(adminProjectId);
            projectsRepository.RemoveAdminProject(adminProject);
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

        public Phase CreatePhase(Phase phase)
        {
            Phase createdPhase = projectsRepository.CreatePhase(phase);
            unitOfWorkManager.Save();
            return createdPhase;
        }

        public void DeletePhase(int phaseId)
        {
            IdeationsManager ideationsManager = new IdeationsManager(unitOfWorkManager);
            SurveysManager surveysManager = new SurveysManager(unitOfWorkManager);
            Phase phase = projectsRepository.GetPhase(phaseId);
            if (phase.Ideations != null)
            {
                foreach (var ideation in phase.Ideations.ToList())
                {
                    ideationsManager.DeleteIdeation(ideation.IdeationId);
                }
            }

            if (phase.Surveys != null)
            {
                foreach (var survey in phase.Surveys.ToList())
                {
                    surveysManager.DeleteSurvey(survey.SurveyId);
                }
            }

            projectsRepository.RemovePhase(phase);
            unitOfWorkManager.Save();
        }

        public Phase EditPhase(Phase phase, int phaseId)
        {
            phase.PhaseId = phaseId;
            Phase editedPhase = projectsRepository.EditPhase(phase);
            unitOfWorkManager.Save();
            return editedPhase;
        }

        #endregion
    }
}