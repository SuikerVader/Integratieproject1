using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Surveys;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Ideations;
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

        #region Survey

        public Survey GetSurvey(int surveyId)
        {
            return surveysRepository.GetSurvey(surveyId);
        }
        public IList<Survey> GetSurveys(int phaseId)
        {
            return surveysRepository.GetSurveys(phaseId).ToList();
        }

        public void CreateSurvey(Survey survey)
        {
            surveysRepository.CreateSurvey(survey);
            unitOfWorkManager.Save();
        }
        public void CreateNewSurvey(int phaseId)
        {
            ProjectsManager projectsManager = new ProjectsManager(unitOfWorkManager);
            Survey survey = new Survey {Phase = projectsManager.GetPhase(phaseId), Title = "_NewSurvey_"};
            surveysRepository.CreateSurvey(survey);
            unitOfWorkManager.Save();
        }
        public void EditSurvey(Survey survey, int surveyId)
        {
            survey.SurveyId = surveyId;
            surveysRepository.EditSurvey(survey);
            unitOfWorkManager.Save();
        }

        public void DeleteSurvey(int surveyId)
        {
            Survey survey = GetSurvey(surveyId);
            if (survey.Questions != null)
            {
                foreach (var question in survey.Questions.ToList())
                {
                    this.DeleteQuestion(question.QuestionId);
                }
            }

            surveysRepository.RemoveSurvey(survey);
            unitOfWorkManager.Save();
        }

        #endregion

        #region Question

        public void CreateQuestion(Question question, int surveyId)
        {
            question.Survey = GetSurvey(surveyId);
            surveysRepository.CreateQuestion(question);
            unitOfWorkManager.Save();
        }
        
        public void EditQuestion(Question question, int questionId, int surveyId)
        {
            question.QuestionId = questionId;
            question.Survey = surveysRepository.GetOnlySurvey(surveyId);
            surveysRepository.EditQuestion(question);
            unitOfWorkManager.Save();
        }

        public void DeleteQuestion(int questionId)
        {
            Question question = GetQuestion(questionId);
            if (question.Answers != null)
            {
                foreach (var answer in question.Answers.ToList())
                {
                    this.DeleteAnswer(answer.AnswerId);
                }
            }

            surveysRepository.RemoveQuestion(question);
            unitOfWorkManager.Save();
        }

        public Question GetQuestion(int questionId)
        {
            return surveysRepository.GetQuestion(questionId);
        }

        #endregion

        #region Answer

        public void CreateAnswer(Answer answer)
        {
            surveysRepository.CreateAnswer(answer);
            unitOfWorkManager.Save();
        }

        public void DeleteAnswer(int answerId)
        {
            Answer answer = GetAnswer(answerId);
            surveysRepository.RemoveAnswer(answer);
            unitOfWorkManager.Save();
        }

        public Answer GetAnswer(int answerId)
        {
            return surveysRepository.GetAnswer(answerId);
        }
        
        public void EditAnswer(Answer answer, int answerId, int questionId)
        {
            answer.AnswerId = answerId;
            answer.Question = GetQuestion(questionId);
            surveysRepository.EditAnswer(answer);
            unitOfWorkManager.Save();
        }

        public void UpdateAnswers(ArrayList answers, int surveyId)
        {
            Survey survey = GetSurvey(surveyId);
            for (int i = 0; i < answers.Count; i++)
            {
                foreach (Question surveyQuestion in survey.Questions)
                {
                    if (surveyQuestion.QuestionNr == i + 1 && surveyQuestion.QuestionType == QuestionType.OPEN)
                    {
                        CreateAnswer(new Answer
                        {
                            AnswerText = answers[i].ToString(),
                            TotalTimesChosen = 1,
                            Question = surveyQuestion
                        });
                    }
                    else if (surveyQuestion.QuestionNr == i + 1 && surveyQuestion.QuestionType == QuestionType.EMAIL)
                    {
                        CreateAnswer(new Answer
                        {
                            AnswerText = answers[i].ToString(),
                            TotalTimesChosen = 1,
                            Question = surveyQuestion
                        });
                    }
                    else if (surveyQuestion.QuestionNr == i + 1)
                    {
                        string[] list = answers[i].ToString().Split(",");
                        foreach (Answer answer in surveyQuestion.Answers)
                        {
                            foreach (string s in list)
                            {
                                if (answer.AnswerText.Equals(s))
                                {
                                    surveysRepository.UpdateAnswer(answer);
                                }
                            }
                        }
                    }
                }
            }
        }

        //using IoTAPI you can answer a single question (doesn't have to be a question from a survey)
        public void UpdateSingleAnswer(Question question, int response)
        {
            foreach ( Answer answer in question.Answers)
            {
                if (answer.AnswerId == response)
                {
                    surveysRepository.UpdateAnswer(answer);
                }
            }
        }
        #endregion


        
    }
}