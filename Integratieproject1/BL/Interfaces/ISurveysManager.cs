using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.BL.Interfaces
{
    public interface ISurveysManager
    {
        Survey GetSurvey(int surveyId);
        void CreateSurvey(Survey survey);
        void CreateQuestion(Question question);
        void CreateAnswer(Answer answer);
        void UpdateSingleAnswer(Question question, int response);
    }
}