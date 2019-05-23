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
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
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
        private readonly UsersManager _userManager;
        private readonly IdeationsManager _ideationsManager;
        private readonly SurveysManager _surveysManager;
        private readonly DataTypeManager _dataTypeManager;
        private readonly IoTManager _ioTManager;

        public ProjectController()
        {
            _projectsManager = new ProjectsManager();
            _userManager = new UsersManager();
            _ideationsManager = new IdeationsManager();
            _surveysManager = new SurveysManager();
            _dataTypeManager = new DataTypeManager();
            _ioTManager = new IoTManager();
        }

        #region Project

        public IActionResult Project(int projectId, string platformName)
        {
            try
            {
                Project project = _projectsManager.GetProject(projectId);
                if (_projectsManager.GetPlatformByName(platformName) == project.Platform)
                {
                    List<IoTSetup> ioTSetups = _ioTManager.GetAllIoTSetupsForProject(projectId);
                    if (ioTSetups != null && ioTSetups.Count > 0)
                    {
                        ViewBag.hasIots = true;
                    }
                    else
                    {
                        ViewBag.hasIots = false;
                    }
                    return View("/UI/Views/Project/Project.cshtml", project);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return NotFound();
            }
            
                
        }

        #endregion

        #region Ideation
        public IActionResult Ideation(int ideationId)
        {
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            List<IoTSetup> ioTSetups = _ioTManager.GetAllIoTSetupsForIdeation(ideationId);

            if (ioTSetups != null && ioTSetups.Count > 0)
            {
                ViewBag.hasIots = true;
            }
            else
            {
                ViewBag.hasIots = false;
            }
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
        }


        #endregion
        
        #region Idea

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
        
        public IActionResult CreateIdea(int ideationId)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Idea idea = _ideationsManager.CreateNewIdea(ideationId, currentUserId);
            ViewBag.ideas = _ideationsManager.GetOtherIdeas(ideationId);
            return View("/UI/Views/Project/AddIdea.cshtml", idea);
        }
        [HttpPost]
        public IActionResult AddIdea(Idea idea, int ideaId, int ideationId)
        {
            _ideationsManager.EditIdea(idea, ideaId);
            Idea returnIdea = _ideationsManager.GetIdea(ideaId);

            ViewBag.tags = _ideationsManager.GetTags(returnIdea.IdeaId);
            return View("/UI/Views/Project/EditIdea.cshtml", returnIdea);
        }
        public IActionResult EditIdea(int ideaId)
        {
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
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
        public IActionResult DeleteIdea(int ideaId, int ideationId)
        {
            _ideationsManager.DeleteIdea(ideaId);
            Ideation ideation = _ideationsManager.GetIdeation(ideationId);
            return View("/UI/Views/Project/Ideation.cshtml", ideation);
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
        
        public IActionResult ViewOtherIdea(int newIdeaId, int otherIdeaId)
        {
            _ideationsManager.DeleteIdea(newIdeaId);
            Idea idea = _ideationsManager.GetIdea(otherIdeaId);
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = User;
                string currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.voteCheck = _ideationsManager.CheckVote(currentUserId, VoteType.VOTE, otherIdeaId);
                ViewBag.sharefbCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_FB, otherIdeaId);
                ViewBag.sharetwCheck = _ideationsManager.CheckVote(currentUserId, VoteType.SHARE_TW, otherIdeaId);
            }
            return View("/UI/Views/Project/Idea.cshtml", idea);
        }

        #region Tags

        public IActionResult AddTag(int ideaId, Tag tag)
        {
            _ideationsManager.CreateIdeaTag(ideaId, tag.TagId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
        public IActionResult RemoveIdeaTag(int ideaTagId, int ideaId)
        {
            _ideationsManager.DeleteIdeaTag(ideaTagId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        #endregion

        #region Position
        public IActionResult AddPosition(int ideaId, Position position)
        {
            _ideationsManager.AddPosition(position, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult EditPosition(Position position, int positionId, int ideaId)
        {
            _dataTypeManager.EditPosition(position,positionId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
        public IActionResult DeletePosition(int positionId, int ideaId)
        {
            _ideationsManager.DeleteLocationFromIdea(ideaId, positionId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }


        #endregion

        #region IdeaObjects
        public IActionResult OrderNrUp(int ideaObjectId, int ideaId)
        {
            _ideationsManager.OrderNrChange(ideaObjectId, "up", ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult OrderNrDown(int ideaObjectId, int ideaId)
        {
            _ideationsManager.OrderNrChange(ideaObjectId, "down", ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        #region TextFields

        public IActionResult AddTextField(TextField textField, int ideaId)
        {
            _ideationsManager.AddTextField(textField, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult EditTextField(TextField textField, int ideaId, int textFieldId)
        {
            _ideationsManager.EditTextField(textField, textFieldId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
        public IActionResult DeleteTextField(int textFieldId, int ideaId)
        {
            _ideationsManager.DeleteTextField(textFieldId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
        

        #endregion

        #region Videos
        public IActionResult AddVideo(Video video, int ideaId)
        {
            _ideationsManager.AddVideo(video, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
        public IActionResult DeleteVideo(int videoId, int ideaId)
        {
            _ideationsManager.DeleteVideo(videoId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }


        #endregion

        #region Images
        public IActionResult AddImage(IFormFile formFile, int ideaId)
        {
            UploadImage(formFile, ideaId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }

        public IActionResult DeleteImage(int imageId, int ideaId)
        {
            _ideationsManager.DeleteImage(imageId);
            Idea idea = _ideationsManager.GetIdea(ideaId);
            ViewBag.tags = _ideationsManager.GetTags(ideaId);
            return View("/UI/Views/Project/EditIdea.cshtml", idea);
        }
        private void UploadImage(IFormFile formFile, int ideaId)
        {
            string wwwroot = "wwwroot/";
            string uploads = "/images/uploads/";
            string path = wwwroot + uploads;

            
            if (formFile.Length > 0)
            {
                string imagePath = Guid.NewGuid() + Path.GetExtension(formFile.FileName);

                using (var fileStream = new FileStream(Path.Combine(path, imagePath), FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }

                _ideationsManager.CreateImage(Path.GetFileName(formFile.FileName), Path.Combine(uploads, imagePath),
                    ideaId);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Survey

        public IActionResult Survey(int surveyId)
        {
            Domain.Surveys.Survey survey = _surveysManager.GetSurvey(surveyId);
            return View("/UI/Views/Project/Survey.cshtml", survey);
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

        #endregion

        #region Reactions
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

        

        #endregion
        
        

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

        public IActionResult AskVerify()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _userManager.AskVerify(currentUserId);
            return RedirectToAction("Index", "Home");
        }

        
    }
}