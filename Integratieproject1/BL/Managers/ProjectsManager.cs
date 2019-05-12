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
        private readonly ProjectsRepository _projectsRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly UsersManager _usersManager;

        public ProjectsManager()
        {
            _unitOfWorkManager = new UnitOfWorkManager();
            _projectsRepository = new ProjectsRepository(_unitOfWorkManager.UnitOfWork);
            _usersManager = new UsersManager(_unitOfWorkManager);
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

        public Platform GetPlatformByName(string platformName)
        {
            return _projectsRepository.GetPlatformByName(platformName);
        }

        public void CreatePlatform(Platform platform)
        {
            _projectsRepository.CreatePlatform(platform);
            _unitOfWorkManager.Save();
        }

        public IList<Platform> GetAllPlatforms()
        {
            return _projectsRepository.GetPlatforms().ToList();
        }

        public void DeletePlatform(int platformId)
        {
            Platform platform = _projectsRepository.GetPlatform(platformId);
            if (platform.Projects != null)
            {
                foreach (Project project in platform.Projects.ToList())
                {
                    DeleteProject(project.ProjectId);
                }
            }

            if (platform.Users != null)
            {
                foreach (CustomUser identityUser in platform.Users.ToList())
                {
                    _usersManager.DeleteUser(identityUser.Id);
                }
            }

            _projectsRepository.RemovePlatform(platform);
            _unitOfWorkManager.Save();
        }
        
        public void EditPlatform(Platform platform, int platformId)
        {
            Platform originalPlatform = GetPlatform(platformId);
            originalPlatform.Address = platform.Address;
            originalPlatform.BackgroundImage = platform.BackgroundImage;
            originalPlatform.Description = platform.Description;
            originalPlatform.Logo = platform.Logo;
            originalPlatform.Phonenumber = platform.Phonenumber;
            originalPlatform.PlatformName = platform.PlatformName;
            originalPlatform.Projects = platform.Projects;
            originalPlatform.Users = platform.Users;
            _projectsRepository.EditPlatform(originalPlatform);
            _unitOfWorkManager.Save();
        }

        public void EditPlatformLayout(Platform platform, int platformId)
        {
            Platform originalPlatform = GetPlatform(platformId);
            originalPlatform.BackgroundColor = platform.BackgroundColor;
            originalPlatform.ButtonColor = platform.ButtonColor;
            originalPlatform.TextColor = platform.TextColor;
            _projectsRepository.EditPlatform(originalPlatform);
            _unitOfWorkManager.Save();
        }
        public void DeleteBackgroundImagePlatform(int platformId)
        {
            Platform platform = GetPlatform(platformId);
            platform.BackgroundImage = null;
            _projectsRepository.EditPlatform(platform);
            _unitOfWorkManager.Save();
        }
        public void DeleteLogoPlatform(int platformId)
        {
            Platform platform = GetPlatform(platformId);
            platform.Logo = null;
            _projectsRepository.EditPlatform(platform);
            _unitOfWorkManager.Save();
        }

        #endregion

        #region Project

        public Project GetProject(int projectId)
        {
            return _projectsRepository.GetProject(projectId);
        }

        public IList<Project> GetProjects(int platformId)
        {
            return _projectsRepository.GetProjects(platformId).ToList();
        }
        
        public IList<Project> GetAllProjects()
        {
            return _projectsRepository.GetAllProjects().ToList();
        }

        public IList<Project> GetAdminProjects(string userId)
        {
            List<Project> projects = new List<Project>();
            if (_usersManager.IsInRole(userId, "Admin"))
            {
                List<AdminProject> adminProjects = _projectsRepository.GetAdminProjectsByUser(userId).ToList();
                foreach (AdminProject adminProject in adminProjects)
                {
                    projects.Add(adminProject.Project);
                }
            }else
            {
                projects = _projectsRepository.GetAllProjects().ToList();
            }


            return projects;
        }

        public IList<AdminProject> GetAllAdminProjects(string userId)
        {
            return _projectsRepository.GetAdminProjectsByUser(userId).ToList();
        }

        public IdentityUser GetUser(string id)
        {
            UsersManager userManager = new UsersManager(_unitOfWorkManager);
            return userManager.GetUser(id);
        }
        
        public void CreateProject(Project project, string userId, int platformId)
        {
            CustomUser identityUser = _usersManager.GetUser(userId);
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

        public Project EditProject(Project project, int projectId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            Project originalProject = GetProject(projectId);
            originalProject.AdminProjects = project.AdminProjects;
            originalProject.Description = project.Description;
            originalProject.EndDate = project.EndDate;
            originalProject.Objective = project.Objective;
            originalProject.Phases = project.Phases;
            originalProject.Platform = project.Platform;
            originalProject.ProjectName = project.ProjectName;
            originalProject.StartDate = project.StartDate;
            originalProject.Status = project.Status;
            project.Location = dataTypeManager.CheckLocation(project.Location);
            if (originalProject.BackgroundImage == null)
            {
                originalProject.BackgroundImage = project.BackgroundImage;
            }
            _unitOfWorkManager.Save();
            return _projectsRepository.EditProject(originalProject);
        }
        public void DeleteBackgroundImageProject(int projectId)
        {
            Project project = GetProject(projectId);
            project.BackgroundImage = null;
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
                    DeletePhase(phase.PhaseId);
                }
            }

            if (project.AdminProjects != null)
            {
                foreach (AdminProject adminProject in project.AdminProjects.ToList())
                {
                    DeleteAdminProject(adminProject.AdminProjectId);
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
        public void CreateAdminProject(int projectId, string adminId)
        {
            UsersManager usersManager = new UsersManager(_unitOfWorkManager);
            CustomUser identityUser = usersManager.GetUser(adminId);
            Project project = _projectsRepository.GetProject(projectId);
            AdminProject adminProject = new AdminProject
            {
                Project = project,
                Admin = identityUser
            };
            _projectsRepository.CreateAdminProject(adminProject);
            _unitOfWorkManager.Save();
        }
        public IList<CustomUser> GetNotProjectAdmins(int projectId)
        {
            UsersManager usersManager = new UsersManager(_unitOfWorkManager);
            IList<CustomUser> allAdmins = usersManager.GetUsers("ADMIN");
            IList<AdminProject> adminProjects = _projectsRepository.GetAdminProjectsByProject(projectId).ToList();
            for (int i = 0; i < allAdmins.Count; i++)
            {
                foreach (var adminProject in adminProjects)
                {
                    if (adminProject.Admin == allAdmins[i])
                    {
                        allAdmins.RemoveAt(i);
                    }
                }
                
            }

            return allAdmins;
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

        public IList<Phase> GetAllPhases(int platformId)
        {
            return _projectsRepository.GetAllPhases(platformId).ToList();
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
                phase.StartDate = project.Phases.Last().EndDate;
            }
            else
            {
                phase.PhaseNr = 1;
                phase.StartDate = project.StartDate;
            }

            phase.EndDate = project.EndDate;
            return phase;
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
                    if (listPhase.StartDate > listPhase.EndDate)
                    {
                        listPhase.EndDate = listPhase.StartDate;
                        EditPhase(listPhase, listPhase.PhaseId);
                    }

                    _projectsRepository.EditPhase(listPhase);
                }

                if (listPhase.PhaseNr == originalPhase.PhaseNr -1)
                {
                    listPhase.EndDate = originalPhase.StartDate;
                    if (listPhase.StartDate > listPhase.EndDate)
                    {
                        listPhase.StartDate = listPhase.EndDate;
                        EditPhase(listPhase, listPhase.PhaseId);
                    }
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