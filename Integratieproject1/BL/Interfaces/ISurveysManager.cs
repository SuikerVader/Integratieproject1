using Integratieproject1.Domain.Surveys;
using System.Collections;
using System.Collections.Generic;

namespace Integratieproject1.BL.Interfaces
{
    public interface ISurveysManager
    {
        #region Survey

        Survey GetSurvey(int surveyId);
        IList<Survey> GetSurveys(int phaseId);
        IList<Survey> GetAllSurveys();
        IList<Survey> GetAllSurveysBySort(string sortOrder);
        IList<Survey> GetProjectsSurveys(int projectId);
        void CreateSurvey(Survey survey);
        void CreateNewSurvey(int phaseId);
        Survey EditSurvey(Survey survey, int surveyId);
        void DeleteSurvey(int surveyId);

        #endregion

        #region Question

        Question GetQuestion(int questionId);
        void CreateQuestion(Question question, int surveyId);
        void EditQuestion(Question question, int questionId, int surveyId);
        void QuestionNrChange(int questionId, string changer, int surveyId);
        void DeleteQuestion(int questionId);

        #endregion

        #region Answer

        Answer GetAnswer(int answerId);
        void CreateAnswer(Answer answer);
        void EditAnswer(Answer answer, int answerId, int questionId);
        void UpdateSingleAnswer(Question question, int response);
        void UpdateAnswers(ArrayList answers, int surveyId);
        void DeleteAnswer(int answerId);
        bool IsEmail(int id, int key);

        #endregion
    }
}