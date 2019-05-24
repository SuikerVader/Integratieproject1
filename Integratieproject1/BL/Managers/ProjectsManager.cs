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

        // Returns platform based onn ID
        public Platform GetPlatform(int platformId)
        {
            return _projectsRepository.GetPlatform(platformId);
        }

        // Returns platform based on NAME
        public Platform GetPlatformByName(string platformName)
        {
            return _projectsRepository.GetPlatformByName(platformName);
        }

        // Creates a new platform based on given platform
        public void CreatePlatform(Platform platform)
        {
            _projectsRepository.CreatePlatform(platform);
            _unitOfWorkManager.Save();
        }

        // Returns a list of all platforms
        public IList<Platform> GetAllPlatforms()
        {
            return _projectsRepository.GetPlatforms().ToList();
        }

        // Returns a list of all platforms sorted by: platformid or platformname
        public IList<Platform> GetAllPlatformsBySort(string sortOrder)
        {
            IEnumerable<Platform> platforms =  GetAllPlatforms().ToList();
            switch (sortOrder)
            {
                case "id_desc":
                    platforms = platforms.OrderByDescending(p => p.PlatformId);
                    break;
                case "Name":
                    platforms = platforms.OrderBy(p => p.PlatformName);
                    break;
                case "name_desc":
                    platforms = platforms.OrderByDescending(p => p.PlatformName);
                    break;
                default:
                    platforms = platforms.OrderBy(p => p.PlatformName);
                    break;
            }
            return platforms.ToList();
        }

        // Deletes platform from database based on ID
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
        
        // Updates the platform based on given platform and ID
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

        // Updates the layout of the given platform and ID
        public void EditPlatformLayout(Platform platform, int platformId)
        {
            Platform originalPlatform = GetPlatform(platformId);
            originalPlatform.BackgroundColor = platform.BackgroundColor;
            originalPlatform.ButtonColor = platform.ButtonColor;
            originalPlatform.TextColor = platform.TextColor;
            _projectsRepository.EditPlatform(originalPlatform);
            _unitOfWorkManager.Save();
        }

        // Deletes the backgroundimage from platform of given ID
        public void DeleteBackgroundImagePlatform(int platformId)
        {
            Platform platform = GetPlatform(platformId);
            platform.BackgroundImage = null;
            _projectsRepository.EditPlatform(platform);
            _unitOfWorkManager.Save();
        }

        // Deletes the logo from platform of given ID
        public void DeleteLogoPlatform(int platformId)
        {
            Platform platform = GetPlatform(platformId);
            platform.Logo = null;
            _projectsRepository.EditPlatform(platform);
            _unitOfWorkManager.Save();
        }

        #endregion

        #region Project

        // Returns project based on ID
        public Project GetProject(int projectId)
        {
            return _projectsRepository.GetProject(projectId);
        }

        // Returns a list of all projects of platform based on ID of platform
        public IList<Project> GetProjects(int platformId)
        {
            return _projectsRepository.GetProjects(platformId).ToList();
        }
        
        // Returns a list of all projects
        public IList<Project> GetAllProjects()
        {
            return _projectsRepository.GetAllProjects().ToList();
        }

        // Returns a list of all projects and sorted by: projectname, startdate, status, enddate, platformname
        public IList<Project> GetAllProjectsBySort(string sortOrder)
        {
            IEnumerable<Project> projects = _projectsRepository.GetAllProjects().ToList();
            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(p => p.ProjectName);
                    break;
                case "StartDate":
                    projects = projects.OrderBy(p => p.StartDate);
                    break;
                case "startdate_desc":
                    projects = projects.OrderByDescending(p => p.StartDate);
                    break;
                case "Status":
                    projects = projects.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    projects = projects.OrderByDescending(p => p.Status);
                    break;
                case "EndDate":
                    projects = projects.OrderBy(p => p.EndDate);
                    break;
                case "enddate_desc":
                    projects = projects.OrderByDescending(p => p.EndDate);
                    break;
                case "Platform":
                    projects = projects.OrderBy(p => p.Platform.PlatformName);
                    break;
                case "platform_desc":
                    projects = projects.OrderByDescending(p => p.Platform.PlatformName);
                    break;
                default:
                    projects = projects.OrderBy(s => s.ProjectName);
                    break;
            }
            return projects.ToList();
        }

        // Returns a list of all projects of a user based on ID of user
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

        // Returns a list of all projects of a user based on ID of user and sorted by: projectname, startdate, status, enddate, platformname
        public IList<Project> GetAdminProjectsBySort(string userId, string sortOrder)
        {
            IEnumerable<Project> projects = GetAdminProjects(userId);
            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(p => p.ProjectName);
                    break;
                case "StartDate":
                    projects = projects.OrderBy(p => p.StartDate);
                    break;
                case "startdate_desc":
                    projects = projects.OrderByDescending(p => p.StartDate);
                    break;
                case "Status":
                    projects = projects.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    projects = projects.OrderByDescending(p => p.Status);
                    break;
                case "EndDate":
                    projects = projects.OrderBy(p => p.EndDate);
                    break;
                case "enddate_desc":
                    projects = projects.OrderByDescending(p => p.EndDate);
                    break;
                case "Platform":
                    projects = projects.OrderBy(p => p.Platform.PlatformName);
                    break;
                case "platform_desc":
                    projects = projects.OrderByDescending(p => p.Platform.PlatformName);
                    break;
                default:
                    projects = projects.OrderBy(s => s.ProjectName);
                    break;
            }
            return projects.ToList();
            }

        // Returns a list of all adminprojects of a user based on ID of user
        public IList<AdminProject> GetAllAdminProjects(string userId)
        {
            return _projectsRepository.GetAdminProjectsByUser(userId).ToList();
        }

        // Returns a list of all adminprojects of a user by given ID of user and sorted by: projectname, startdate, status, enddate, platformname
        public IList<AdminProject> GetAllAdminProjectsBySort(string userId, string sortOrder)
        {
            IEnumerable<AdminProject> projects = GetAllAdminProjects(userId);
            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(p => p.Project.ProjectName);
                    break;
                case "StartDate":
                    projects = projects.OrderBy(p => p.Project.StartDate);
                    break;
                case "startdate_desc":
                    projects = projects.OrderByDescending(p => p.Project.StartDate);
                    break;
                case "Status":
                    projects = projects.OrderBy(p => p.Project.Status);
                    break;
                case "status_desc":
                    projects = projects.OrderByDescending(p => p.Project.Status);
                    break;
                case "EndDate":
                    projects = projects.OrderBy(p => p.Project.EndDate);
                    break;
                case "enddate_desc":
                    projects = projects.OrderByDescending(p => p.Project.EndDate);
                    break;
                case "Platform":
                    projects = projects.OrderBy(p => p.Project.Platform.PlatformName);
                    break;
                case "platform_desc":
                    projects = projects.OrderByDescending(p => p.Project.Platform.PlatformName);
                    break;
                default:
                    projects = projects.OrderBy(p => p.Project.ProjectName);
                    break;
            }
            return projects.ToList();
        }

        // Creates new project based on newly given project, user based on ID and platform based on ID
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

        // Updates project based on newly given project and ID
        // Returns updated project
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

        // Deletes backgroundimage from project of given ID
        public void DeleteBackgroundImageProject(int projectId)
        {
            Project project = GetProject(projectId);
            project.BackgroundImage = null;
            _projectsRepository.EditProject(project);
            _unitOfWorkManager.Save();
        }

        // Deletes project from database based on ID
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

        // Deletes adminproject from database based on ID
        public void DeleteAdminProject(int adminProjectId)
        {
            AdminProject adminProject = _projectsRepository.GetAdminProject(adminProjectId);
            _projectsRepository.RemoveAdminProject(adminProject);
            _unitOfWorkManager.Save();
        }
        
        // Returns adminproject based on ID
        public AdminProject GetAdminProject(int adminProjectId)
        {
            return _projectsRepository.GetAdminProject(adminProjectId);
        }

        // Creates a new adminproject so the admin based on ID is assigned to the project based on ID
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

        // Returns a list of all adminprojects based on ID of project
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

        // Returns a list of all adminprojects based on ID of project and sorted by: username, surname, name, email, age
        public IList<CustomUser> GetNotProjectAdminsBySort(int projectId, string sortOrder)
        {
            IEnumerable<CustomUser> admins = GetNotProjectAdmins(projectId);
            switch (sortOrder)
            {
                case "username_desc":
                    admins = admins.OrderByDescending(u => u.UserName);
                    break;
                case "Surname":
                    admins = admins.OrderBy(u => u.Surname);
                    break;
                case "surname_desc":
                    admins = admins.OrderByDescending(u => u.Surname);
                    break;
                case "Name":
                    admins = admins.OrderBy(u => u.Name);
                    break;
                case "name_desc":
                    admins = admins.OrderByDescending(u => u.Name);
                    break;
                case "Email":
                    admins = admins.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    admins = admins.OrderByDescending(p => p.Email);
                    break;
                case "Age":
                    admins = admins.OrderBy(u => u.Age);
                    break;
                case "age_desc":
                    admins = admins.OrderByDescending(u => u.Age);
                    break;
                default:
                    admins = admins.OrderBy(u => u.UserName);
                    break;
            }
            return admins.ToList();
        }

        #endregion

        #region Phase

        // Returns phase based on ID
        public Phase GetPhase(int phaseId)
        {
            return _projectsRepository.GetPhase(phaseId);
        }

        // Returns a list of all phases of a project based on ID of project
        public IList<Phase> GetPhases(int projectId)
        {
            return _projectsRepository.GetPhases(projectId).ToList();
        }

        // Returns a list of all phases of a platform based on ID of platform
        public IList<Phase> GetAllPhases(int platformId)
        {
            return _projectsRepository.GetAllPhases(platformId).ToList();
        }

        // Creates a new phase based on given phase and phasenumber and adds it to the project of given ID
        // Returns newly created phase
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

        // Returns the phase of a project based on ID of project
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

        // Deletes phase from database based on ID
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

        // Updates phase based on newly given phase and ID
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