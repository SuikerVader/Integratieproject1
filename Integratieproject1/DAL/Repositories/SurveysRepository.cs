using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Surveys;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class SurveysRepository : ISurveysRepository
    {
        private readonly CityOfIdeasDbContext ctx = null;

        
        public SurveysRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            ctx = unitOfWork.ctx;
        }

        #region Survey

        // Survey methods
        public IEnumerable<Survey> GetSurveys(int phaseId)
        {
            return ctx.Surveys
                .Where(p => p.Phase.PhaseId == phaseId)
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .AsEnumerable();
        }
        public Survey GetOnlySurvey(int surveyId)
        {
            return ctx.Surveys.Find(surveyId);
        }
        public Survey GetSurvey(int surveyId)
        {
            return ctx.Surveys
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .Single(s => s.SurveyId == surveyId);
        }
        public Survey CreateSurvey(Survey survey)
        {
            ctx.Surveys.Add(survey);
            ctx.SaveChanges();
            return survey;
        }
        public Survey EditSurvey(Survey survey)
        {
            ctx.Surveys.Update(survey);
            ctx.SaveChanges();
            return survey;
        }
        public void RemoveSurvey(Survey survey)
        {
            ctx.Surveys.Remove(survey);
            ctx.SaveChanges();
        }

        #endregion
        
        

        #region Question

        // Question methods
        public IEnumerable<Question> GetQuestions()
        {
            return ctx.Questions.AsEnumerable();
        }

        public Question GetQuestion(int questionId)
        {
            return ctx.Questions.Find(questionId);
        }
        
        public Question EditQuestion(Question question)
        {
            ctx.Questions.Update(question);
            ctx.SaveChanges();
            return question;
        }

        public Question CreateQuestion(Question question)
        {
            ctx.Questions.Add(question);
            ctx.SaveChanges();
            return question;
        }
        public void RemoveQuestion(Question question)
        {
            ctx.Questions.Remove(question);
            ctx.SaveChanges();
        }

        #endregion
        

        #region Answer
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
        public Answer EditAnswer(Answer answer)
        {
            ctx.Answers.Update(answer);
            ctx.SaveChanges();
            return answer;
        }

        public Answer UpdateAnswer(Answer answer)
        {
            Console.WriteLine("repo update called!");
            Answer answer1 = new Answer();
            answer1 = ctx.Answers.Find(answer.AnswerId);
            answer1.TotalTimesChosen += 1;
            ctx.Entry(answer1).State = EntityState.Modified;
            ctx.SaveChanges();
            return answer;
        }

        public void RemoveAnswer(Answer answer)
        {
            ctx.Answers.Remove(answer);
            ctx.SaveChanges();
        }
        

        #endregion


        
    }
}