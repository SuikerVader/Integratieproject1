using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void UpdateAnswers(ArrayList answers, int surveyId)
        {
            Survey survey = GetSurvey(surveyId);
            for (int i = 0; i < answers.Count; i++)
            {
                foreach (Question surveyQuestion in survey.Questions)
                {
                    if (surveyQuestion.QuestionNr == i + 1 && surveyQuestion.Answers.First().AnswerType == AnswerType.OPEN)
                    {
                        CreateAnswer(new Answer
                        {
                            AnswerText = answers[i].ToString(),
                            AnswerType = AnswerType.OPEN,
                            TotalTimesChosen = 1,
                            Question = surveyQuestion
                        });
                    }
                    else if (surveyQuestion.QuestionNr == i + 1 && surveyQuestion.Answers.First().AnswerType == AnswerType.EMAIL)
                    {
                        CreateAnswer(new Answer
                        {
                            AnswerText = answers[i].ToString(),
                            AnswerType = AnswerType.EMAIL,
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
    }
}