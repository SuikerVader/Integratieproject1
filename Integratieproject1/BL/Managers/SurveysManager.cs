using Integratieproject1.BL.Models.Surveys;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositorys;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Integratieproject1.BL.Managers
{
    public class SurveysManager
    {
        private SurveysRepository surveysRepository;
        private CityOfIdeasDbContext ctx;

        public SurveysManager()
        {
            ctx = Program.uow.ctx;
            this.surveysRepository = new SurveysRepository(ctx);
        }

        public Survey GetSurvey(int surveyId)
        {
            return surveysRepository.GetSurvey(surveyId);
        }
    }
}