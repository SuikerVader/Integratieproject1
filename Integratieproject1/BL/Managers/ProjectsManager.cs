using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

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

        public IList<Project> GetAdminProjects(string userId)
        {
            List<AdminProject> adminProjects = projectsRepository.GetAdminProjects(userId).ToList();
            List<Project> projects = new List<Project>();
            foreach (AdminProject adminProject in adminProjects)
            {
                projects.Add(adminProject.Project);
            }

            return projects;
        }

        public IdentityUser GetUser(string id)
        {
            UsersManager userManager = new UsersManager(unitOfWorkManager);
            return userManager.GetUser(id);
        }
        public void CreateProject(Project project, string userId)
        {
            IdentityUser identityUser = GetUser(userId);
            project.Platform = GetPlatform(1);
            DataTypeManager dataTypeManager = new DataTypeManager(unitOfWorkManager);
            project.Location = dataTypeManager.CheckLocation(project.Location);
            //Project createdProject = projectsRepository.CreateProject(project);
            AdminProject adminProject = new AdminProject
            {
                Project = project,
                Admin = identityUser
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

        public Phase CreatePhase(Phase phase, int phaseNr, int projectId)
        {
            phase.PhaseNr = phaseNr;
            phase.Project = this.GetProject(projectId);
            Project project = GetProject(projectId);
            
                foreach (var previousPhase in project.Phases)
                {
                    if (previousPhase.PhaseNr == phaseNr - 1)
                    {
                        previousPhase.EndDate = phase.StartDate;
                        projectsRepository.EditPhase(previousPhase);
                    }
                }
            Phase createdPhase = projectsRepository.CreatePhase(phase);
            unitOfWorkManager.Save();
            return createdPhase;
        }


        public Phase GetNewPhase(int projectId)
        {
            Phase phase = new Phase();
            Project project = GetProject(projectId);
            phase.Project = project;
            if (project.Phases != null && project.Phases.Count > 0)
            {
                phase.PhaseNr = project.Phases.Last().PhaseNr + 1;
                //phase.StartDate = project.Phases.Last().EndDate;
            }
            else
            {
                phase.PhaseNr = 1;
                phase.StartDate = project.StartDate;
            }

            phase.EndDate = project.EndDate;
            return phase;
        }

        private bool CheckPhase(Phase phase)
        {
            if (phase.StartDate >= phase.Project.StartDate)
            {
                return false;
            }

            if (phase.EndDate <= phase.Project.EndDate)
            {
                return false;
            }

            IList<Phase> phases = phase.Project.Phases.ToList();
            foreach (var listPhase in phases)
            {
                if (phase.PhaseNr > listPhase.PhaseNr)
                {
                    if (phase.StartDate <= listPhase.EndDate)
                    {
                        return false;
                    }
                }
                else
                {
                    if (phase.EndDate <= listPhase.StartDate)
                    {
                        return false;
                    }
                }
            }

            return true;
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
            Phase originalPhase = GetPhase(phaseId);
            originalPhase.PhaseName = phase.PhaseName;
            originalPhase.StartDate = phase.StartDate;
            originalPhase.EndDate = phase.EndDate;
            originalPhase.Description = phase.Description;
            Project project = GetProject(originalPhase.Project.ProjectId);
            foreach (var listPhase in project.Phases)
            {
                if (listPhase.PhaseNr == originalPhase.PhaseNr + 1)
                {
                    listPhase.StartDate = originalPhase.EndDate;
                    projectsRepository.EditPhase(listPhase);
                }

                if (listPhase.PhaseNr == originalPhase.PhaseNr -1)
                {
                    listPhase.EndDate = originalPhase.StartDate;
                    projectsRepository.EditPhase(listPhase);
                }
            }

            Phase editedPhase = projectsRepository.EditPhase(originalPhase);
            unitOfWorkManager.Save();
            return editedPhase;
        }


        #endregion
    }
}