using System;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.BL.Managers
{
    public class IoTManager : IIoTProjectsManager
    {
        private readonly IoTRepository _ioTRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly SurveysManager _surveysManager;

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
    }
}