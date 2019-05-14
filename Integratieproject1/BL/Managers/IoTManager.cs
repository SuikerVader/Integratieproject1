using System;
using System.Collections.Generic;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Project = Integratieproject1.Domain.Projects.Project;

namespace Integratieproject1.BL.Managers
{
    public class IoTManager : IIoTProjectsManager
    {
        private readonly IoTRepository _ioTRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly SurveysManager _surveysManager;
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;

        public IoTManager(SurveysManager surveysManager)
        {
            _surveysManager = surveysManager;
            _unitOfWorkManager = new UnitOfWorkManager();
            _ioTRepository = new IoTRepository(_unitOfWorkManager.UnitOfWork);
        }
        public IoTManager(UnitOfWorkManager unitOfWorkManager, SurveysManager surveysManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            _unitOfWorkManager = unitOfWorkManager;
            _surveysManager = surveysManager;
            _ioTRepository = new IoTRepository(_unitOfWorkManager.UnitOfWork);
        }

        public void DeleteIoTSetup(IoTSetup ioTSetup)
        {
            _ioTRepository.RemoveIoTSetup(ioTSetup);
            _unitOfWorkManager.Save();
        }
        
        public void CreateIoT(Position position, Idea idea, Question question = null)
        {
            IoTSetup setup = new IoTSetup
            {
                Idea = idea, Position = position, Question = question, Code = GenerateIoTUrl()
            };
            _ioTRepository.CreateIoTSetup(setup);
            _unitOfWorkManager.Save();
        }

        public IoTSetup CreateIoTWithIdea(Idea idea)
        {
            IoTSetup setup = new IoTSetup();
            setup.Idea = idea;
            return setup;
        }

        public IoTSetup CreateIoTWithQuestion(Question question)
        {
            IoTSetup setup = new IoTSetup();
            setup.Question = question;
            return setup;
        }
        
        public string GenerateIoTUrl()
        {
            //TODO: creeer link die doorverwijst naar sign-up page.
            return "randomSignUpUrl" + DateTime.Now;
        }

        //in case of an IoTSetup that offers multiple options (buttons)
        public void RegisterComplexVote(int id, int supportLv)
        {
            IoTSetup setup = _ioTRepository.GetIoTSetupByIdea(id);
            _surveysManager.UpdateSingleAnswer(setup.Question, supportLv);
        }

        public IList<IoTSetup> GetUserIoTProjects(string currentUserId)
        {
            IList<IoTSetup> list = new List<IoTSetup>();
            IList<Project> projects = _projectsManager.GetAdminProjects(currentUserId);
            
            foreach (var project in projects)
            {
                foreach (var phase in _projectsManager.GetPhases(project.ProjectId))
                {
                    foreach (var ideation in _ideationsManager.GetIdeations(phase.PhaseId))
                    {
                        foreach (var idea in _ideationsManager.GetIdeas(ideation.IdeationId))
                        {
                            if (_ioTRepository.IoTExistsFromIdea(idea))
                            {
                                list.Add(_ioTRepository.GetIoTSetupByIdea(idea.IdeaId));
                            }
                        }
                    }

                    foreach (var survey in phase.Surveys)
                    {
                        foreach (var question in survey.Questions)
                        {
                            if (_ioTRepository.IoTExistsFromQuestion(question))
                            {
                                list.Add(_ioTRepository.GetIoTSetupByQuestion(question.QuestionId));
                            }
                        }
                    }
                }
            }
            return list;
        }

        public IoTSetup AddPosition(Position position, IoTSetup setup)
        {
            setup.Position = position;
            return setup;
        }
    }
}