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
        private IoTRepository ioTRepository;
        private UnitOfWorkManager unitOfWorkManager;
        private SurveysManager surveysManager;

        public IoTManager()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            ioTRepository = new IoTRepository(unitOfWorkManager.UnitOfWork);
        }
        public IoTManager( UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException("unitOfWorkManager");

            this.unitOfWorkManager = unitOfWorkManager;
            ioTRepository = new IoTRepository(this.unitOfWorkManager.UnitOfWork);
        }

        public void DeleteIoTSetup(IoTSetup ioTSetup)
        {
            ioTRepository.RemoveIoTSetup(ioTSetup);
            unitOfWorkManager.Save();
        }
        
        public void CreateIoT(Position position, Idea idea, Question question)
        {
            IoTSetup setup = new IoTSetup
            {
                Idea = idea, Position = position, Question = question, Code = GenerateIoTUrl()
            };
            ioTRepository.CreateIoTSetup(setup);
            unitOfWorkManager.Save();
        }
        
        public string GenerateIoTUrl()
        {
            //TODO: creeer link die doorverwijst naar sign-up page.
            return "randomsignupurl" + DateTime.Now;
        }

        //in case of an IoTSetup that offers multiple options (buttons)
        public void RegisterComplexVote(int id, int supportLv)
        {
            IoTSetup setup = ioTRepository.GetIoTSetupByIdea(id);
            surveysManager.UpdateSingleAnswer(setup.Question, supportLv);
        }
    }
}