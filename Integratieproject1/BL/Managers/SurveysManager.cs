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
        private readonly SurveysRepository _surveysRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;

        public SurveysManager()
        {
            _unitOfWorkManager = new UnitOfWorkManager();
            _surveysRepository = new SurveysRepository(_unitOfWorkManager.UnitOfWork);
        }

        public SurveysManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            this._unitOfWorkManager = unitOfWorkManager;
            this._surveysRepository = new SurveysRepository(unitOfWorkManager.UnitOfWork);
        }

        #region Survey

        // Returns a survey based on ID
        public Survey GetSurvey(int surveyId)
        {
            return _surveysRepository.GetSurvey(surveyId);
        }

        // Returns a list of all surveys of a phase based on ID of phase
        public IList<Survey> GetSurveys(int phaseId)
        {
            return _surveysRepository.GetSurveys(phaseId).ToList();
        }
        
        // Returns a list of all surveys of a project based on ID of project
        public IList<Survey> GetProjectsSurveys(int projectId)
        {
            return _surveysRepository.GetProjectSurveys(projectId).ToList();
        }

        // Returns a list of all surveys
        public IList<Survey> GetAllSurveys()
        {
            return _surveysRepository.GetAllSurveys().ToList();
        }

        // Returns a list of all surveys sorted by: title
        public IList<Survey> GetAllSurveysBySort(string sortOrder)
        {
            IEnumerable<Survey> surveys = GetAllSurveys();
            switch (sortOrder)
            {
                case "name_desc":
                    surveys = surveys.OrderByDescending(t => t.Title);
                    break;
                default:
                    surveys = surveys.OrderBy(t => t.Title);
                    break;
            }
            return surveys.ToList();
        }

        // Creates new survey based on given survey
        public void CreateSurvey(Survey survey)
        {
            _surveysRepository.CreateSurvey(survey);
            _unitOfWorkManager.Save();
        }

        // Creates new template survey based on phase of given ID
        public void CreateNewSurvey(int phaseId)
        {
            ProjectsManager projectsManager = new ProjectsManager(_unitOfWorkManager);
            Survey survey = new Survey {Phase = projectsManager.GetPhase(phaseId), Title = "_NewSurvey_"};
            _surveysRepository.CreateSurvey(survey);
            _unitOfWorkManager.Save();
        }

        // Updates survey based on given survey and ID
        // Returns updated survey
        public Survey EditSurvey(Survey survey, int surveyId)
        {
            Survey originalSurvey = GetSurvey(surveyId);
            originalSurvey.Title = survey.Title;
            Survey returnSurvey = _surveysRepository.EditSurvey(originalSurvey);
            _unitOfWorkManager.Save();
            return returnSurvey;
        }

        // Deletes survey from database based on ID
        public void DeleteSurvey(int surveyId)
        {
            Survey survey = GetSurvey(surveyId);
            if (survey.Questions != null)
            {
                foreach (var question in survey.Questions.ToList())
                {
                    DeleteQuestion(question.QuestionId);
                }
            }

            _surveysRepository.RemoveSurvey(survey);
            _unitOfWorkManager.Save();
        }

        #endregion

        #region Question

        // Creates new question based on given question and add survey based on ID
        public void CreateQuestion(Question question, int surveyId)
        {
            IEnumerable<Question> questions = _surveysRepository.GetQuestions(surveyId);
            question.QuestionNr = questions.Count()+1;
            question.Survey = GetSurvey(surveyId);
            _surveysRepository.CreateQuestion(question);
            _unitOfWorkManager.Save();
        }

        // Updates question based on given question and ID
        public void EditQuestion(Question question, int questionId, int surveyId)
        {
            question.QuestionId = questionId;
            question.Survey = _surveysRepository.GetOnlySurvey(surveyId);
            _surveysRepository.EditQuestion(question);
            _unitOfWorkManager.Save();
        }

        // Updates order of question based on ID and string 
        public void QuestionNrChange(int questionId, string changer, int surveyId)
        {
            Question question = _surveysRepository.GetQuestion(questionId);
            IEnumerable<Question> questions = _surveysRepository.GetQuestions(surveyId);
            if (changer.Equals("up"))
            {
                foreach (var listQuestion in questions)
                {
                    if (listQuestion.QuestionNr == question.QuestionNr - 1)
                    {
                        listQuestion.QuestionNr = listQuestion.QuestionNr + 1;
                        question.QuestionNr = question.QuestionNr - 1;
                        _surveysRepository.EditQuestion(question);
                        _surveysRepository.EditQuestion(listQuestion);
                        break;
                    }
                }
            } else if (changer.Equals("down"))
            {
                foreach (var listQuestion in questions)
                {
                    if (listQuestion.QuestionNr == question.QuestionNr + 1)
                    {
                        listQuestion.QuestionNr = listQuestion.QuestionNr - 1;
                        question.QuestionNr = question.QuestionNr + 1;
                        _surveysRepository.EditQuestion(question);
                        _surveysRepository.EditQuestion(listQuestion);
                        break;
                    }
                } 
            }
            _unitOfWorkManager.Save();
        }

        // Deletes question from database based on ID
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

            _surveysRepository.RemoveQuestion(question);
            _unitOfWorkManager.Save();
        }

        // Returns a question based on ID
        public Question GetQuestion(int questionId)
        {
            return _surveysRepository.GetQuestion(questionId);
        }

        // Returns a list of all questions of a survey based on ID of survey
        public IEnumerable<Question> GetQuestions(int surveyId)
        {
            return _surveysRepository.GetQuestions(surveyId);
        }
        
        #endregion

        #region Answer

        // Creates new answer based on given answer
        public void CreateAnswer(Answer answer)
        {
            _surveysRepository.CreateAnswer(answer);
            _unitOfWorkManager.Save();
        }

        // Deletes answer from database based on ID
        public void DeleteAnswer(int answerId)
        {
            Answer answer = GetAnswer(answerId);
            _surveysRepository.RemoveAnswer(answer);
            _unitOfWorkManager.Save();
        }

        // Returns answer based on ID
        public Answer GetAnswer(int answerId)
        {
            return _surveysRepository.GetAnswer(answerId);
        }

        // Update answer from question based on given answer and ID
        public void EditAnswer(Answer answer, int answerId, int questionId)
        {
            answer.AnswerId = answerId;
            answer.Question = GetQuestion(questionId);
            _surveysRepository.EditAnswer(answer);
            _unitOfWorkManager.Save();
        }

        // Updates arraylist of answers from survey based on ID of survey
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
                                    _surveysRepository.UpdateAnswer(answer);
                                }
                            }
                        }
                    }
                }
            }
        }

        //using IoTAPI you can answer a single question (doesn't have to be a question from a survey)
        // Updates single answer based on question and ID
        public void UpdateSingleAnswer(int questionId, int response)
        {
            Console.WriteLine("updating answer response");
            Question question = _surveysRepository.GetQuestion(questionId);
            foreach (Answer answer in question.Answers)
            {
                if (answer.AnswerId == response)
                {
                    _surveysRepository.UpdateAnswer(answer);
                }
            }
        }

        public List<Answer> GetAnswersFromQuestion(Question question)
        {
            return _surveysRepository.GetAnswersFromQuestion(question).ToList();
        }

        // Checks if answer is email or not
        // Returns true if email
        // Returns false if not email
        public bool IsEmail(int id, int key)
        {
            return _surveysRepository.IsEmail(id, key);
        }
        #endregion
    }
}