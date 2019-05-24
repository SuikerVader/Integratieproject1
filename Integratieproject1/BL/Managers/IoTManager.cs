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
        
        // Returns a newly generated string which returns to sign-up page
        public string GenerateIoTUrl()
        {
            //TODO: creeer link die doorverwijst naar sign-up page.
            return "randomSignUpUrl" + DateTime.Now;
        }

        //in case of an IoTSetup that offers multiple options (buttons)
        public void RegisterComplexVote(int id, int supportLv)
        {
            SurveysManager surveysManager = new SurveysManager(_unitOfWorkManager);
            IoTSetup setup = _ioTRepository.GetIoTSetupByIdea(id);
            surveysManager.UpdateSingleAnswer(setup.Question, supportLv);
        }

        #region Gets
        // Returns iot based on ID
        public IoTSetup GetIoT(string iotId)
        {
            return _ioTRepository.GetIoTSetup(iotId);
        }

        // Returns a list of all iotsetups for a platform based on ID of platform
        public List<IoTSetup> GetAllIoTSetupsForPlatform(int platformId)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForPlatform(platformId).ToList();
            return ioTSetups;
        }

        // Returns a list of all iotsetups for a project based on ID of project
        public List<IoTSetup> GetAllIoTSetupsForProject(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForProject(id).ToList();
            return ioTSetups;
        }

        // Returns a list of all iotsetups for an ideation based on ID of ideation
        public List<IoTSetup> GetAllIoTSetupsForIdeation(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForIdeation(id).ToList();
            return ioTSetups;
        }

        // Returns a list of all iotsetups for an idea based on ID of idea
        public List<IoTSetup> GetAllIoTSetupsForIdea(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForIdea(id).ToList();
            return ioTSetups;
        }

        // Returns a list of all iotsetups for a question based on ID of question
        public List<IoTSetup> GetAllIoTSetupsForQuestion(int id)
        {
            List<IoTSetup> ioTSetups = _ioTRepository.GetAllIoTSetupsForQuestion(id).ToList();
            return ioTSetups;
        }

        #endregion
        
        #region CUD
        // Deletes iotsetup from database based on ID
        public void DeleteIoTSetup(string ioTId)
        {
            IoTSetup ioTSetup = _ioTRepository.GetIoTSetup(ioTId);
            _ioTRepository.RemoveIoTSetup(ioTSetup);
            _unitOfWorkManager.Save();
        }
        
        // Creates new iotsetup based on given position, idea and question
        public void CreateIoT(Position position, Idea idea, Question question)
        {
            IoTSetup setup = new IoTSetup
            {
                Idea = idea, Position = position, Question = question, Code = GenerateIoTUrl()
            };
            _ioTRepository.CreateIoTSetup(setup);
            _unitOfWorkManager.Save();
        }

        // Creates new iotsetup based on newly given iotsetup and ID and with given type
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


        // Updates iotsetup based on newly given iotsetup and ID
        public void EditIoTSetup(IoTSetup ioTSetup, string iotId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            IoTSetup original = GetIoT(iotId);
            dataTypeManager.EditPosition(ioTSetup.Position, original.Position.PositionId);
            original.Position = ioTSetup.Position;
            _ioTRepository.UpdateIoTSetup(original);
            _unitOfWorkManager.Save();
        }


        #endregion
        
    }
}