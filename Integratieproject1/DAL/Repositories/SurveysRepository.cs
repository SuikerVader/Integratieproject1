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
        private readonly CityOfIdeasDbContext _ctx;

        
        public SurveysRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _ctx = unitOfWork.Ctx;
        }

        #region Survey

        // Survey methods
        public IEnumerable<Survey> GetSurveys(int phaseId)
        {
            return _ctx.Surveys
                .Where(p => p.Phase.PhaseId == phaseId)
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .AsEnumerable();
        }

        public IEnumerable<Survey> GetAllSurveys()
        {
            return _ctx.Surveys
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .AsEnumerable();
        }

        public Survey GetOnlySurvey(int surveyId)
        {
            return _ctx.Surveys.Find(surveyId);
        }
        public Survey GetSurvey(int surveyId)
        {
            return _ctx.Surveys
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .Include(p => p.Phase).ThenInclude(pr => pr.Project)
                .Single(s => s.SurveyId == surveyId);
        }
        public Survey CreateSurvey(Survey survey)
        {
            _ctx.Surveys.Add(survey);
            _ctx.SaveChanges();
            return survey;
        }
        public Survey EditSurvey(Survey survey)
        {
            _ctx.Surveys.Update(survey);
            _ctx.SaveChanges();
            return survey;
        }
        public void RemoveSurvey(Survey survey)
        {
            _ctx.Surveys.Remove(survey);
            _ctx.SaveChanges();
        }

        public bool IsEmail(int id, int key)
        {
            Survey survey = _ctx.Surveys.Include(s => s.Questions).Single(s => s.SurveyId == id);
            Question question = survey.Questions.Where(q => q.QuestionNr == key).Single();
            return question.QuestionType == QuestionType.EMAIL;
        }

        #endregion
        
        #region Question

        // Question methods
        public IEnumerable<Question> GetQuestions(int surveyId)
        {
            return _ctx.Questions.Where(q => q.Survey.SurveyId == surveyId).OrderBy(q => q.QuestionNr).AsEnumerable();
        }

        public Question GetQuestion(int questionId)
        {
            return _ctx.Questions
                .Include(q => q.Survey)
                .Single(q => q.QuestionId == questionId);
        }
        
        public Question EditQuestion(Question question)
        {
            _ctx.Questions.Update(question);
            _ctx.SaveChanges();
            return question;
        }

        public Question CreateQuestion(Question question)
        {
            _ctx.Questions.Add(question);
            _ctx.SaveChanges();
            return question;
        }
        public void RemoveQuestion(Question question)
        {
            _ctx.Questions.Remove(question);
            _ctx.SaveChanges();
        }

        #endregion

        #region Answer
        // Answer methods
        public IEnumerable<Answer> GetAnswers()
        {
            return _ctx.Answers.AsEnumerable();
        }

        public Answer GetAnswer(int answerId)
        {
            return _ctx.Answers.Find(answerId);
        }

        public Answer CreateAnswer(Answer answer)
        {
            _ctx.Answers.Add(answer);
            _ctx.SaveChanges();
            return answer;
        }
        public Answer EditAnswer(Answer answer)
        {
            _ctx.Answers.Update(answer);
            _ctx.SaveChanges();
            return answer;
        }

        public Answer UpdateAnswer(Answer answer)
        {
            Console.WriteLine("repo update called!");
            Answer answer1 = _ctx.Answers.Find(answer.AnswerId);
            answer1.TotalTimesChosen += 1;
            _ctx.Entry(answer1).State = EntityState.Modified;
            _ctx.SaveChanges();
            return answer;
        }

        public void RemoveAnswer(Answer answer)
        {
            _ctx.Answers.Remove(answer);
            _ctx.SaveChanges();
        }
        

        #endregion
    }
}