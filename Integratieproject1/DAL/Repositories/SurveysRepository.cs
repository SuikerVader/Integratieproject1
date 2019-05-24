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

        // Returns enumerable of surveys of phase based on ID of phase
        public IEnumerable<Survey> GetSurveys(int phaseId)
        {
            return _ctx.Surveys
                .Where(p => p.Phase.PhaseId == phaseId)
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .Include(q => q.Questions).ThenInclude(q => q.IoTSetups).ThenInclude(i => i.Position)
                .AsEnumerable();
        }

        // Returns enumerable of all surveys of a project based on ID of project
        public IEnumerable<Survey> GetProjectSurveys(int projectId)
        {
            return _ctx.Surveys
                .Where(s => s.Phase.Project.ProjectId == projectId)
                .Include(p => p.Phase).ThenInclude(ph => ph.Project)
                .AsEnumerable();

        }

        // Returns enumerable of all surveys
        public IEnumerable<Survey> GetAllSurveys()
        {
            return _ctx.Surveys
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .AsEnumerable();
        }

        // Returns only survey based on ID
        public Survey GetOnlySurvey(int surveyId)
        {
            return _ctx.Surveys.Find(surveyId);
        }

        // Returns survey based on ID
        public Survey GetSurvey(int surveyId)
        {
            return _ctx.Surveys
                .Include(q => q.Questions).ThenInclude(a => a.Answers)
                .Include(q => q.Questions).ThenInclude(q => q.IoTSetups).ThenInclude(i => i.Position)
                .Include(p => p.Phase).ThenInclude(pr => pr.Project).ThenInclude(pl => pl.Platform)
                .Single(s => s.SurveyId == surveyId);
        }

        // Creates survey based on given survey
        public Survey CreateSurvey(Survey survey)
        {
            _ctx.Surveys.Add(survey);
            _ctx.SaveChanges();
            return survey;
        }

        // Updates survey based on given survey
        // Returns updated survey
        public Survey EditSurvey(Survey survey)
        {
            _ctx.Surveys.Update(survey);
            _ctx.SaveChanges();
            return survey;
        }

        // Deletes given survey from database
        public void RemoveSurvey(Survey survey)
        {
            _ctx.Surveys.Remove(survey);
            _ctx.SaveChanges();
        }

        #endregion
        
        #region Question

        // Question methods

        // Returns enumerable of questions of survey based on ID of survey
        public IEnumerable<Question> GetQuestions(int surveyId)
        {
            return _ctx.Questions.Where(q => q.Survey.SurveyId == surveyId).OrderBy(q => q.QuestionNr)
                .Include(s=>s.Answers)
                .AsEnumerable();
        }

        // Returns question based on ID
        public Question GetQuestion(int questionId)
        {
            return _ctx.Questions
                .Include(q => q.Survey)
                .Include(q => q.IoTSetups).ThenInclude(i => i.Position)
                .Single(q => q.QuestionId == questionId);
        }
        
        // Updates question based on given question
        public Question EditQuestion(Question question)
        {
            _ctx.Questions.Update(question);
            _ctx.SaveChanges();
            return question;
        }

        // Creates question based on given question
        public Question CreateQuestion(Question question)
        {
            _ctx.Questions.Add(question);
            _ctx.SaveChanges();
            return question;
        }

        // Deletes question based on given question
        public void RemoveQuestion(Question question)
        {
            _ctx.Questions.Remove(question);
            _ctx.SaveChanges();
        }

        #endregion

        #region Answer
        // Answer methods

        // Returns a enumerable of all answers
        public IEnumerable<Answer> GetAnswers()
        {
            return _ctx.Answers.AsEnumerable();
        }

        // Returns answer based on ID
        public Answer GetAnswer(int answerId)
        {
            return _ctx.Answers.Find(answerId);
        }

        // Creates new answer based on given answer
        public Answer CreateAnswer(Answer answer)
        {
            _ctx.Answers.Add(answer);
            _ctx.SaveChanges();
            return answer;
        }

        // Updates answer based on given answer
        public Answer EditAnswer(Answer answer)
        {
            _ctx.Answers.Update(answer);
            _ctx.SaveChanges();
            return answer;
        }

        // Updates answer based on given answer
        public Answer UpdateAnswer(Answer answer)
        {
            Console.WriteLine("repo update called!");
            Answer answer1 = _ctx.Answers.Find(answer.AnswerId);
            answer1.TotalTimesChosen += 1;
            _ctx.Entry(answer1).State = EntityState.Modified;
            _ctx.SaveChanges();
            return answer;
        }

        // Deletes given answer from database
        public void RemoveAnswer(Answer answer)
        {
            _ctx.Answers.Remove(answer);
            _ctx.SaveChanges();
        }

        public IEnumerable<Answer> GetAnswersFromQuestion(Question question)
        {
            return _ctx.Answers.Where(q => q.Question == question).AsEnumerable();
        }
        

        // Checks if answer is email or not
        // Returns true if email
        // Returns false if not email
        public bool IsEmail(int id, int key)
        {
            Survey survey = _ctx.Surveys.Include(s => s.Questions).Single(s => s.SurveyId == id);
            Question question = survey.Questions.Where(q => q.QuestionNr == key).Single();
            return question.QuestionType == QuestionType.EMAIL;
        }

        #endregion
    }
}