using System;
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
                Idea = idea, Position = position, Question = question, Code = GenerateIoTUrl()
            };
            _ioTRepository.CreateIoTSetup(setup);
            _unitOfWorkManager.Save();
        }
        
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

        public IoTSetup GetIoT(string iotId)
        {
            return _ioTRepository.GetIoTSetup(iotId);
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
    }
}