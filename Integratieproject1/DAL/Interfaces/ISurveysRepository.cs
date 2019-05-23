using System.Collections.Generic;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.DAL.Interfaces
{
    public interface ISurveysRepository
    {
       #region Survey

       IEnumerable<Survey> GetSurveys(int phaseId);
        IEnumerable<Survey> GetProjectSurveys(int projectId);
        IEnumerable<Survey> GetAllSurveys();
        Survey GetOnlySurvey(int surveyId);
        Survey GetSurvey(int surveyId);
        Survey CreateSurvey(Survey survey);
        Survey EditSurvey(Survey survey);
        void RemoveSurvey(Survey survey);
        bool IsEmail(int id, int key);
        
        #endregion
        
        #region Question

        IEnumerable<Question> GetQuestions(int surveyId);
        Question GetQuestion(int questionId);
        Question EditQuestion(Question question);
        Question CreateQuestion(Question question);
        void RemoveQuestion(Question question);
        
        #endregion

        #region Answer
        // Answer methods
        IEnumerable<Answer> GetAnswers();
        Answer GetAnswer(int answerId);
        Answer CreateAnswer(Answer answer);
        Answer EditAnswer(Answer answer);
        Answer UpdateAnswer(Answer answer);
        void RemoveAnswer(Answer answer);

        #endregion
    }
}