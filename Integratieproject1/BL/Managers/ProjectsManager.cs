using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Microsoft.AspNetCore.Identity;


namespace Integratieproject1.BL.Managers
{
    public class ProjectsManager : IProjectsManager
    {
        private readonly ProjectsRepository _projectsRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public ProjectsManager()
        {
            _unitOfWorkManager = new UnitOfWorkManager();
            _projectsRepository = new ProjectsRepository(_unitOfWorkManager.UnitOfWork);
        }

        public ProjectsManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            this._unitOfWorkManager = unitOfWorkManager;
            this._projectsRepository = new ProjectsRepository(unitOfWorkManager.UnitOfWork);
        }

        #region Platform

        public Platform GetPlatform(int platformId)
        {
            return _projectsRepository.GetPlatform(platformId);
        }

        public void CreatePlatform(Platform platform)
        {
            _projectsRepository.CreatePlatform(platform);
            _unitOfWorkManager.Save();
        }

        #endregion

        #region Project

        public Project GetProject(int projectId)
        {
            return _projectsRepository.GetProject(projectId);
        }

        public IList<Project> GetAdminProjects(string userId)
        {
            List<AdminProject> adminProjects = _projectsRepository.GetAdminProjects(userId).ToList();
            List<Project> projects = new List<Project>();
            foreach (AdminProject adminProject in adminProjects)
            {
                projects.Add(adminProject.Project);
            }

            return projects;
        }
        
        public IList<AdminProject> GetAllAdminProjects(string userId)
        {
            List<AdminProject> adminProjects = _projectsRepository.GetAdminProjects(userId).ToList();
            return adminProjects;
        }
        
        public IList<Project> GetAllProjects()
        {
            List<Project> projects = _projectsRepository.GetAllProjects().ToList();
            return projects;
        }

        public IdentityUser GetUser(string id)
        {
            UsersManager userManager = new UsersManager(_unitOfWorkManager);
            return userManager.GetUser(id);
        }
        public void CreateProject(Project project, string userId, int platformId = 1)
        {
            IdentityUser identityUser = GetUser(userId);
            project.Platform = GetPlatform(platformId);
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            project.Location = dataTypeManager.CheckLocation(project.Location);
            //Project createdProject = projectsRepository.CreateProject(project);
            AdminProject adminProject = new AdminProject
            {
                Project = project,
                Admin = identityUser
            };
            _projectsRepository.CreateAdminProject(adminProject);
            _unitOfWorkManager.Save();
        }

        public void EditProject(Project project, int projectId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            project.Location = dataTypeManager.CheckLocation(project.Location);
            project.ProjectId = projectId;
            _projectsRepository.EditProject(project);
            _unitOfWorkManager.Save();
        }

        public void DeleteProject(int projectId)
        {
            Project project = _projectsRepository.GetProject(projectId);
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

            _projectsRepository.RemoveProject(project);
            _unitOfWorkManager.Save();
        }

        public void DeleteAdminProject(int adminProjectId)
        {
            AdminProject adminProject = _projectsRepository.GetAdminProject(adminProjectId);
            _projectsRepository.RemoveAdminProject(adminProject);
            _unitOfWorkManager.Save();
        }
        
        public AdminProject GetAdminProject(int adminProjectId)
        {
            return _projectsRepository.GetAdminProject(adminProjectId);
        }

        #endregion

        #region Phase

        public Phase GetPhase(int phaseId)
        {
            return _projectsRepository.GetPhase(phaseId);
        }

        public IList<Phase> GetPhases(int projectId)
        {
            return _projectsRepository.GetPhases(projectId).ToList();
        }

        public Phase CreatePhase(Phase phase, int phaseNr, int projectId)
        {
            phase.PhaseNr = phaseNr;
            phase.Project = GetProject(projectId);
            Project project = GetProject(projectId);
            
                foreach (var previousPhase in project.Phases)
                {
                    if (previousPhase.PhaseNr == phaseNr - 1)
                    {
                        previousPhase.EndDate = phase.StartDate;
                        if (previousPhase.EndDate < previousPhase.StartDate)
                        {
                            previousPhase.StartDate = previousPhase.EndDate;
                        }
                        EditPhase(previousPhase, previousPhase.PhaseId);
                    }
                }
            Phase createdPhase = _projectsRepository.CreatePhase(phase);
            _unitOfWorkManager.Save();
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
            IdeationsManager ideationsManager = new IdeationsManager(_unitOfWorkManager);
            SurveysManager surveysManager = new SurveysManager(_unitOfWorkManager);
            Phase phase = _projectsRepository.GetPhase(phaseId);
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

            _projectsRepository.RemovePhase(phase);
            _unitOfWorkManager.Save();
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
                    _projectsRepository.EditPhase(listPhase);
                }

                if (listPhase.PhaseNr == originalPhase.PhaseNr -1)
                {
                    listPhase.EndDate = originalPhase.StartDate;
                    _projectsRepository.EditPhase(listPhase);
                }
            }

            Phase editedPhase = _projectsRepository.EditPhase(originalPhase);
            _unitOfWorkManager.Save();
            return editedPhase;
        }


        #endregion
    }
}