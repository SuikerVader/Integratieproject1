using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.BL.Managers
{
    public class IoTManager : IIoTProjectsManager
    {
        private readonly IoTRepository _ioTRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
    

        public IoTManager()
        {
            
            _unitOfWorkManager = new UnitOfWorkManager();
            _ioTRepository = new IoTRepository(_unitOfWorkManager.UnitOfWork);
        }
        public IoTManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            _unitOfWorkManager = unitOfWorkManager;
            _ioTRepository = new IoTRepository(_unitOfWorkManager.UnitOfWork);
        }
        
        public string GenerateIoTUrl(string id)
        {
            //TODO: creeer link die doorverwijst naar sign-up page.
            return "http://34.76.196.101/Antwerpen/Home/QrCode/" + id;
        }

        //in case of an IoTSetup that offers multiple options (buttons)
        public void RegisterComplexVote(int questionId, int answer, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _surveysManager.UpdateSingleAnswer(questionId, answer);
            }
        }

        public void RegisterSimpleVote(int ideaId, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine("registering votes on idea: "+ ideaId + "times " +amount);
                _ideationsManager.CreateVote(ideaId, VoteType.IOT, null);
            }
        }

        #region Gets
        public IoTSetup GetIoT(string iotId)
        {
            return _ioTRepository.GetIoTSetup(iotId);
        }
        public List<IoTSetup> GetAllIoTSetupsForPlatform(int platformId)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForPlatform(platformId).ToList();
            return ioTSetups;
        }

        public List<IoTSetup> GetAllIoTSetupsForProject(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForProject(id).ToList();
            return ioTSetups;
        }

        public List<IoTSetup> GetAllIoTSetupsForIdeation(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForIdeation(id).ToList();
            return ioTSetups;
        }

        public List<IoTSetup> GetAllIoTSetupsForIdea(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForIdea(id).ToList();
            return ioTSetups;
        }

        public List<IoTSetup> GetAllIoTSetupsForQuestion(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForQuestion(id).ToList();
            return ioTSetups;
        }

        #endregion
        
        #region CUD
        public void DeleteIoTSetup(string ioTId)
        {
            IoTSetup ioTSetup = _ioTRepository.GetIoTSetup(ioTId);
            _ioTRepository.RemoveIoTSetup(ioTSetup);
            _unitOfWorkManager.Save();
        }
        
        public void CreateIoT(Position position, Idea idea, Question question)
        {
            IoTSetup setup = new IoTSetup
            {
                Idea = idea, Position = position, Question = question
            };
            _ioTRepository.CreateIoTSetup(setup);
            _unitOfWorkManager.Save();
        }

        public void RegisterVotes(string payload)
        {
            var contents = payload.Split('-');
            if (contents.Length < 3)
            {
                Console.WriteLine("Error in mqttpacket");
            }
            else
            {

                if (contents.Length == 3)
                {
                    RegisterIdeaVotes(contents[1], contents[2]);
                }
                else
                {
                    RegisterQuestionVotes(contents);
                }
            }
        }

        public void RegisterQuestionVotes(string[] contents)
        {
            try
            {
                var setup = GetIoT(contents[1]);
                for (int q = 0; q < contents.Length-2; q++)
                {
                    if (Int32.TryParse(contents[q+2], out int x))
                    {
                        for (int i=0; i < x; i++)
                        {
                            _surveysManager.UpdateSingleAnswer(setup.Question.QuestionId, q);
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Error in mqttpacket");
            }
        }

        public void RegisterIdeaVotes(string id, string votes)
        {
            var setup = GetIoT(id);
            if (Int32.TryParse(votes, out int i))
            {
                for (int j = 0; j < i; j++)
                {
                    _ideationsManager.CreateVote(setup.Idea.IdeaId, VoteType.IOT, null);
                    Console.WriteLine("simple vote " + i);
                }
            }
        }

        public void CreateIoTSetup(IoTSetup ioTSetup, int id, string type)
        {
            
            if (type.Equals("question"))
            {
                SurveysManager surveysManager = new SurveysManager(_unitOfWorkManager);
                ioTSetup.Question = surveysManager.GetQuestion(id);
            }
            else
            {
                IdeationsManager ideationsManager = new IdeationsManager(_unitOfWorkManager);
                ioTSetup.Idea = ideationsManager.GetIdea(id);
            }
            _ioTRepository.CreateIoTSetup(ioTSetup);
            _unitOfWorkManager.Save();
        }


        public void EditIoTSetup(IoTSetup ioTSetup, string iotId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            IoTSetup original = GetIoT(iotId);
            dataTypeManager.EditPosition(ioTSetup.Position, original.Position.PositionId);
            original.Position = ioTSetup.Position;
            _ioTRepository.UpdateIoTSetup(original);
            _unitOfWorkManager.Save();
        }
        
        public List<IoTSetup> GetAllIoTSetups()
        {
            List<IoTSetup> list = _ioTRepository.GetIoTSetups().ToList();return list;
        }


        #endregion
        
    }
}