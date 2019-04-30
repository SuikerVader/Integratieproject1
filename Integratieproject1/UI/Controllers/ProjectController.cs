using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Primitives;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Integratieproject1.UI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectsManager _projectsManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly DataTypeManager _dataTypeManager;

        public ProjectController()
        {
            _projectsManager = new ProjectsManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _dataTypeManager = new DataTypeManager();
        }

        public IActionResult Project(int projectId)
        {
            Project project = _projectsManager.GetProject(projectId);
            return View("/UI/Views/Project/Project.cshtml", project);
        }

        public IActionResult Ideation(int ideationId)
        {
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }

        public IActionResult Survey(int surveyId)
        {
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Survey.cshtml", survey);
        }

        public IActionResult Idea(int ideaId)
        {
            Idea idea = _ideationsManager.GetIdea(ideaId);

            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, ideaId);
                ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, ideaId);
                ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, ideaId);
            }

            return View("/UI/Views/Project/Idea.cshtml", idea);
        }
        

        public IActionResult ReportPost(int id, string type)
        {
            _ideationsManager.ReportPost(id, type);
            if (type.Equals("reaction"))
            {
                Reaction reaction = _ideationsManager.GetReaction(id);
                if (reaction.Idea == null && reaction.Ideation != null)
                {
                    Ideation ideation = _ideationsManager.GetIdeation(reaction.Ideation.IdeationId);
                    return View("/UI/Views/Project/Ideation.cshtml", ideation);
                }
                else
                {
                    Idea idea = _ideationsManager.GetIdea(reaction.Idea.IdeaId);
                    if (User.Identity.IsAuthenticated)
                    {
                        ClaimsPrincipal currentUser = User;
                        string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                        ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, idea.IdeaId);
                        ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, idea.IdeaId);
                        ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, idea.IdeaId);
                    }
                    return View("/UI/Views/Project/Idea.cshtml", idea);
                }
            }
            else
            {
                Idea idea = _ideationsManager.GetIdea(id);
                if (User.Identity.IsAuthenticated)
                {
                    ClaimsPrincipal currentUser = User;
                    string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                    ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, idea.IdeaId);
                    ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, idea.IdeaId);
                    ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, idea.IdeaId);
                }
                return View("/UI/Views/Project/Idea.cshtml", idea);
            }
        }

        [HttpPost]
        public IActionResult PostReaction(IFormCollection formCollection, int id, string element)
        {
            ArrayList parameters = new ArrayList();
            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }

            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _ideationsManager.PostReaction(parameters, id, currentUserId, element);
            if (element.Equals("idea"))
            {
                Idea idea = _ideationsManager.GetIdea(id);
                    ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, id);
                    ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, id);
                    ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, id);
                return View("/UI/Views/Project/Idea.cshtml", idea);
            }
            else if (element.Equals("ideation"))

            {
                Domain.Ideations.Ideation ideation = _ideationsManager.GetIdeation(id);
                return View("/UI/Views/Project/Ideation.cshtml", ideation);
            }
            else
            {
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpPost]
        public IActionResult SaveSurveyFormData(IFormCollection formCollection, int surveyId)
        {
            ArrayList answers = new ArrayList();
            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                try
                {
                    if (_surveysManager.IsEmail(surveyId, Convert.ToInt32(pair.Key)))
                    {
                        var apiKey = "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";
                        var client = new SendGridClient(apiKey);
                        var msg = new SendGridMessage()
                        {
                            From = new EmailAddress("CityOfIdeas@coi.com", "City Of Ideas"),
                            Subject = "Register",
                            PlainTextContent = "Hi!",
                            HtmlContent = "<strong>Thanks for filling in our survey! If you're interested in future projects and would like to be up to date then you can register here: https://localhost:44305/Identity/Account/Register </strong>"
                        };
                        msg.AddTo(new EmailAddress(pair.Value));
                        client.SendEmailAsync(msg);
                    }
                    answers.Add(pair.Value);
                }
                catch
                {

                }
            }

            _surveysManager.UpdateAnswers(answers, surveyId);
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            Project project = _projectsManager.GetProject(survey.Phase.Project.ProjectId);
            return View("/UI/Views/Project/Project.cshtml", project);
        }

        public IActionResult SurveyResults(int surveyId)
        {
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/SurveyResults.cshtml", survey);
        }

        public IActionResult CreateVote(int ideaId, VoteType voteType)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _ideationsManager.CreateVote(ideaId, voteType, currentUserId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
                ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, ideaId);
                ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, ideaId);
                ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, ideaId);
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        public IActionResult LikeReaction(int id, string type, int reactionId)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _ideationsManager.LikeReaction(reactionId, currentUserId);
            if (type.Equals("idea"))
            {
                Idea idea = _ideationsManager.GetIdea(id);
                    ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, id);
                    ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, id);
                    ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, id);
                return View("/UI/Views/Project/Idea.cshtml", idea);
            }
            else
            {
                Ideation ideation = _ideationsManager.GetIdeation(id);
                return View("/UI/Views/Project/Ideation.cshtml", ideation);
            }
        }

        [HttpPost]
        public IActionResult PostIdea(IFormCollection formCollection, List<IFormFile> formFiles, int ideationId)
        {
            ArrayList parameters = new ArrayList();

            foreach (KeyValuePair<string, StringValues> pair in formCollection)
            {
                parameters.Add(pair.Value);
            }

            if (parameters.Count > 0)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                Idea idea = _ideationsManager.PostIdea(parameters, ideationId, currentUserId);

                if (formFiles.Count > 0)
                {
                    UploadImages(formFiles, idea.IdeaId);
                }

                Ideation ideation = _ideationsManager.GetIdeation(ideationId);
                return View("/UI/Views/Project/Ideation.cshtml", ideation);
            }
            else
            {
                throw new Exception("fout createIdea");
            }
        }

        private void UploadImages(List<IFormFile> formFiles, int ideaId)
        {
            string wwwroot = "wwwroot/";
            string uploads = "/images/uploads/";
            string path = wwwroot + uploads;

            foreach (var file in formFiles)
            {
                if (file.Length > 0)
                {
                    string imagePath = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(path, imagePath), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    _ideationsManager.CreateImage(Path.GetFileName(file.FileName), Path.Combine(uploads, imagePath),
                        ideaId);
                }
            }
        }


        public IActionResult CreateIdea(int ideationId)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Idea idea = _ideationsManager.CreateNewIdea(ideationId, currentUserId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult EditIdea(int ideaId)
        {
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        [HttpPost]
        public IActionResult EditIdea(Idea idea, int ideaId, int ideationId)
        {
            _ideationsManager.EditIdea(idea, ideaId);
            Idea returnIdea = _ideationsManager.GetIdea(ideaId);
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, ideaId);
                ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, ideaId);
                ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, ideaId);
            }
            return View("/UI/Views/Project/Idea.cshtml", returnIdea);
        }

        public IActionResult OrderNrUp(int ideaObjectId, int ideaId)
        {
            _ideationsManager.OrderNrChange(ideaObjectId, "up", ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult OrderNrDown(int ideaObjectId, int ideaId)
        {
            _ideationsManager.OrderNrChange(ideaObjectId, "down", ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult DeleteIdea(int ideaId, int ideationId)
        {
            _ideationsManager.DeleteIdea(ideaId);
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }

        public IActionResult AddVideo(Video video, int ideaId)
        {
            _ideationsManager.AddVideo(video, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult AddTextField(TextField textField, int ideaId)
        {
            _ideationsManager.AddTextField(textField, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult EditTextField(TextField textField, int ideaId, int textFieldId)
        {
            _ideationsManager.EditTextField(textField, textFieldId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult AddImage(List<IFormFile> formFiles, int ideaId)
        {
            UploadImages(formFiles, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult DeleteImage(int imageId, int ideaId)
        {
            _ideationsManager.DeleteImage(imageId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult DeleteVideo(int videoId, int ideaId)
        {
            _ideationsManager.DeleteVideo(videoId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult DeleteTextField(int textFieldId, int ideaId)
        {
            _ideationsManager.DeleteTextField(textFieldId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult AddPosition(Position position, int ideaId)
        {
            _ideationsManager.AddPosition(position, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);

            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
    }
}