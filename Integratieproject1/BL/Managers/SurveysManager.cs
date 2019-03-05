using System;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Integratieproject1.BL.Managers
{
    public class SurveysManager : ISurveysManager
    {
        private SurveysRepository surveysRepository;
        private UnitOfWorkManager unitOfWorkManager;

        public SurveysManager()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            surveysRepository = new SurveysRepository(unitOfWorkManager.UnitOfWork);
        }
        public SurveysManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException("unitOfWorkManager");
            
            this.unitOfWorkManager = unitOfWorkManager;
            this.surveysRepository = new SurveysRepository(unitOfWorkManager.UnitOfWork);
        }

        public Survey GetSurvey(int surveyId)
        {
            return surveysRepository.GetSurvey(surveyId);
        }

        public void CreateSurvey(Survey survey)
        {
            surveysRepository.CreateSurvey(survey);
            unitOfWorkManager.Save();
        }

        public void CreateQuestion(Question question)
        {
            surveysRepository.CreateQuestion(question);
            unitOfWorkManager.Save();
        }

        public void CreateAnswer(Answer answer)
        {
            surveysRepository.CreateAnswer(answer);
            unitOfWorkManager.Save();
        }
    }
}