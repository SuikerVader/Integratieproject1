using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Models.Surveys;

namespace Integratieproject1.DAL.Repositorys
{
    public class SurveysRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public SurveysRepository(CityOfIdeasDbContext dbContext)
        {
            ctx = dbContext;
            //CityOfIdeasDbInitializer.Initialize(ctx, false);
        }
        
        // Survey methods
        public IEnumerable<Survey> GetSurveys()
        {
            return ctx.Surveys.AsEnumerable();
        }
        public Survey GetSurvey(int surveyId)
        {
            return ctx.Surveys.Find(surveyId);
        }
        public Survey CreateSurvey(Survey survey)
        {
            ctx.Surveys.Add(survey);
            ctx.SaveChanges();
            return survey;
        }
        
        // Question methods
        public IEnumerable<Question> GetQuestions()
        {
            return ctx.Questions.AsEnumerable();
        }
        public Question GetQuestion(int questionId)
        {
            return ctx.Questions.Find(questionId);
        }
        public Question CreateQuestion(Question question)
        {
            ctx.Questions.Add(question);
            ctx.SaveChanges();
            return question;
        }
        
        // Answer methods
        public IEnumerable<Answer> GetAnswers()
        {
            return ctx.Answers.AsEnumerable();
        }
        public Answer GetAnswer(int answerId)
        {
            return ctx.Answers.Find(answerId);
        }
        public Answer CreateAnswer(Answer answer)
        {
            ctx.Answers.Add(answer);
            ctx.SaveChanges();
            return answer;
        }
    }
}