using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.BL.Managers
{
    public class IdeationsManager : IIdeationsManager
    {
        private readonly IdeationsRepository _ideationsRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly UsersManager _usersManager;
        private readonly DataTypeManager _dataTypeManager;
        private SurveysManager surveysManager;

        public IdeationsManager()
        {
            _unitOfWorkManager = new UnitOfWorkManager();
            _ideationsRepository = new IdeationsRepository(_unitOfWorkManager.UnitOfWork);
            _usersManager = new UsersManager(_unitOfWorkManager);
            _dataTypeManager = new DataTypeManager(_unitOfWorkManager);
        }

        public IdeationsManager(UnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
            _ideationsRepository = new IdeationsRepository(_unitOfWorkManager.UnitOfWork);
        }

        #region Ideation

        // Returns the ideation based on given ID
        public Ideation GetIdeation(int ideationId)
        {
            return _ideationsRepository.GetIdeation(ideationId);
        }

        // Returns a list of ideations of a projects based on given ID of project
        public IList<Ideation> GetProjectIdeations(int projectId)
        {
            return _ideationsRepository.GetProjectsIdeations(projectId).ToList();
        }

        // Returns a list of ideations of a phase based on given ID of phase
        public IList<Ideation> GetIdeations(int phaseId)
        {
            return _ideationsRepository.GetIdeations(phaseId).ToList();
        }

        // Returns a list of ideations of a platform based on given ID of platform
        public IList<Ideation> GetIdeationsByPlatform(int platformId)
        {
            return _ideationsRepository.GetIdeationsByPlatform(platformId).ToList();
        }

        // Returns a list of all ideations in the database
        public IList<Ideation> GetAllIdeations()
        {
            return _ideationsRepository.GetAllIdeations().ToList();
        }

        // Returns a list of all ideations sorted by: CentralQuestion
        public IList<Ideation> GetAllIdeationsBySort(string sortOrder)
        {
            IEnumerable<Ideation> ideations = GetAllIdeations();
            switch (sortOrder)
            {
                case "name_desc":
                    ideations = ideations.OrderByDescending(t => t.CentralQuestion);
                    break;
                default:
                    ideations = ideations.OrderBy(t => t.CentralQuestion);
                    break;
            }
            return ideations.ToList();
        }

        // Creates an ideation in the database based on given ideation and ID
        // Returns newly created ideation
        public void CreateIdeation(Ideation ideation, int phaseId)
        {
            ProjectsManager projectsManager = new ProjectsManager(_unitOfWorkManager);
            Phase phase = projectsManager.GetPhase(phaseId);
            ideation.Phase = phase;
            _ideationsRepository.CreateIdeation(ideation);
            _unitOfWorkManager.Save();
        }

        // Updates the values of ideation based on new ideation and ID
        // Returns the updated ideation
        public Ideation EditIdeation(Ideation ideation, int ideationId)
        {
            Ideation originalIdeation = GetIdeation(ideationId);
            originalIdeation.CentralQuestion = ideation.CentralQuestion;
            originalIdeation.InputIdeation = ideation.InputIdeation;
            originalIdeation.ExternalLink = ideation.ExternalLink;
            originalIdeation.TextAllowed = ideation.TextAllowed;
            originalIdeation.TextRequired = ideation.TextRequired;
            originalIdeation.ImageAllowed = ideation.ImageAllowed;
            originalIdeation.ImageRequired = ideation.ImageRequired;
            originalIdeation.VideoAllowed = ideation.VideoAllowed;
            originalIdeation.VideoRequired = ideation.VideoRequired;
            originalIdeation.MapAllowed = ideation.MapAllowed;
            originalIdeation.MapRequired = ideation.MapRequired;
            foreach (Idea idea in originalIdeation.Ideas.ToList())
            {
                if (!originalIdeation.MapAllowed && idea.Position != null)
                {
                    _dataTypeManager.DeletePosition(idea.Position.PositionId);
                    idea.Position = null;
                }
                foreach (IdeaObject ideaObject in idea.IdeaObjects.ToList())
                {
                    if (!originalIdeation.TextAllowed && ideaObject.GetType() == typeof(TextField))
                    {
                        DeleteTextField(ideaObject.IdeaObjectId);
                    }
                    if (!originalIdeation.ImageAllowed && ideaObject.GetType() == typeof(Image))
                    {
                        DeleteImage(ideaObject.IdeaObjectId);
                    }
                    if (!originalIdeation.VideoAllowed && ideaObject.GetType() == typeof(Video))
                    {
                        DeleteVideo(ideaObject.IdeaObjectId);
                    }
                }
                if (originalIdeation.MapRequired)
                {
                    Position position = new Position()
                    {
                        Lng = "0",
                        Lat = "0"
                    };
                    idea.Position = position;
                }
                if (originalIdeation.TextRequired && idea.GetTextFields().Count == 0)
                {
                    TextField textField = new TextField
                    {
                        Text = "Verplicht tekstveld",
                        Idea = idea
                    };
                    AddTextField(textField, idea.IdeaId);
                }
                if (originalIdeation.ImageRequired && idea.GetImages().Count == 0)
                {
                    Image image = new Image
                    {
                        ImageName = "Verplichte afbeelding",
                        ImagePath = idea.Ideation.Phase.Project.BackgroundImage,
                        Idea = idea
                    };
                    CreateImage(image.ImageName, image.ImagePath, idea.IdeaId);
                }
                if (originalIdeation.VideoRequired && idea.GetVideos().Count == 0)
                {
                    Video video = new Video
                    {
                        Url = "https://www.youtube.com/embed/Ce7hJ24a8yM",
                        Idea = idea
                    };
                    AddVideo(video, idea.IdeaId);
                }
            }
            Ideation returnIdeation = _ideationsRepository.EditIdeation(originalIdeation);
            _unitOfWorkManager.Save();
            return returnIdeation;
        }

        // Deletes ideation from database based on ID
        public void DeleteIdeation(int ideationId)
        {
            Ideation ideation = GetIdeation(ideationId);
            if (ideation.Ideas != null)
            {
                foreach (var idea in ideation.Ideas.ToList())
                {
                    this.DeleteIdea(idea.IdeaId);
                }
            }

            if (ideation.Reactions != null)
            {
                foreach (var reaction in ideation.Reactions.ToList())
                {
                    this.DeleteReaction(reaction.ReactionId);
                }
            }

            _ideationsRepository.RemoveIdeation(ideation);
            _unitOfWorkManager.Save();
        }

        #endregion

        #region Idea

        // Returns a list of all ideas of a platform based on given ID of platform
        public IList<Idea> GetAllIdeas(int platformId)
        {
            return _ideationsRepository.GetAllIdeas(platformId).ToList();
        }

        // Returns a list of all ideas of a user based on given ID of user
        public IEnumerable<Idea> GetIdeasByUser(string currentUserId)
        {
            return _ideationsRepository.GetIdeasByUser(currentUserId);
        }

        // Returns a list of all ideas which are not yet published sorted by: Title, Username, CentralQuestion
        public IEnumerable<Idea> GetIdeasByUserSorted(string currentUserId, string sortOrder)
        {
            IEnumerable<Idea> ideas = GetIdeasByUser(currentUserId);
            switch (sortOrder)
            {
                case "title_desc":
                    ideas = ideas.OrderByDescending(i => i.Title);
                    break;
                case "Ideation":
                    ideas = ideas.OrderBy(i => i.Ideation.CentralQuestion);
                    break;
                case "ideation_desc":
                    ideas = ideas.OrderByDescending(i => i.Ideation.CentralQuestion);
                    break;
                case "Phase":
                    ideas = ideas.OrderBy(i => i.Ideation.Phase.PhaseName);
                    break;
                case "phase_desc":
                    ideas = ideas.OrderByDescending(i => i.Ideation.Phase.PhaseName);
                    break;
                case "Project":
                    ideas = ideas.OrderBy(i => i.Ideation.Phase.Project.ProjectName);
                    break;
                case "project_desc":
                    ideas = ideas.OrderByDescending(i => i.Ideation.Phase.Project.ProjectName);
                    break;
                default:
                    ideas = ideas.OrderBy(i => i.Title);
                    break;
            }
            return ideas;
        }

        // Returns a list of all ideas which are not yet published sorted by: Title, Username, CentralQuestion
        public IEnumerable<Idea> GetAllNonPublishedIdeas(string sortOrder)
        {
            IEnumerable<Idea> ideas = _ideationsRepository.GetAllNonPublishedIdeas().ToList();
            switch (sortOrder)
            {
                case "name_desc":
                    ideas = ideas.OrderByDescending(i => i.Title);
                    break;
                case "User":
                    ideas = ideas.OrderBy(i => i.IdentityUser.UserName);
                    break;
                case "user_desc":
                    ideas = ideas.OrderByDescending(i => i.IdentityUser.UserName);
                    break;
                case "Ideation":
                    ideas = ideas.OrderBy(i => i.Ideation.CentralQuestion);
                    break;
                case "ideation_desc":
                    ideas = ideas.OrderByDescending(i => i.Ideation.CentralQuestion);
                    break;
                default:
                    ideas = ideas.OrderBy(i => i.Title);
                    break;
            }
            return ideas;
        }

        // Returns a list of all ideas of an ideation based on given ID of ideation
        public IList<Idea> GetIdeas(int ideationId)
        {
            return _ideationsRepository.GetIdeas(ideationId).ToList();
        }

        // Returns a list of all ideas of an ideation based on given ID of ideation
        public IList<Idea> GetOtherIdeas(int ideationId)
        {
            Ideation ideation = GetIdeation(ideationId);
            List<Idea> ideas = ideation.Ideas.ToList();

            return ideas;
        }

        // Returns a list of all ideas of a project based on given ID of project which are reported
        public IList<Idea> GetReportedIdeas(int projectId)
        {
            return _ideationsRepository.GetReportedIdeas(projectId).ToList();
        }

        // Creates a new idea template with ideation from given ID and user from given ID
        // Returns newly created idea
        public Idea CreateNewIdea(int ideationId, string userId)
        {
            UsersManager usersManager = new UsersManager(_unitOfWorkManager);
            CustomUser user = usersManager.GetUser(userId);
            Ideation ideation = GetIdeation(ideationId);
            Idea idea = new Idea()
            {
                Ideation = ideation,
                Title = "_NewIdea_",
                IdentityUser = user,
                IdeaObjects = new List<IdeaObject>()
            };
            if (_usersManager.IsInRole(userId, "USER"))
            {
                idea.Published = false;
            }
            if (ideation.MapRequired)
            {
                Position position = new Position()
                {
                    Lng = "0",
                    Lat = "0"
                };
                idea.Position = position;
            }
            if (ideation.TextRequired)
            {
                TextField textField = new TextField
                {
                    Text = "Verplicht tekstveld",
                    Idea = idea
                };
                idea.IdeaObjects.Add(textField);
            }
            if (ideation.ImageRequired)
            {
                Image image = new Image
                {
                    ImageName = "Verplichte afbeelding",
                    ImagePath = idea.Ideation.Phase.Project.BackgroundImage,
                    Idea = idea
                };
                idea.IdeaObjects.Add(image);
            }
            if (ideation.VideoRequired)
            {
                Video video = new Video
                {
                    Url = "https://www.youtube.com/embed/Ce7hJ24a8yM",
                    Idea = idea
                };
                idea.IdeaObjects.Add(video);
            }
            Idea returnIdea = _ideationsRepository.CreateIdea(idea);
            _unitOfWorkManager.Save();
            return returnIdea;
        }

        // Gets the idea from given ID and publishes it
        public void PublishIdea(int ideaId)
        {
            Idea idea = GetIdea(ideaId);
            _ideationsRepository.PublishIdea(idea);
            _unitOfWorkManager.Save();
        }

        // Updates the idea from given ID
        public void ChangeIdea(Idea idea)
        {
            _ideationsRepository.UpdateIdea(idea);
            _unitOfWorkManager.Save();
        }

        // Returns the idea from given ID
        public Idea GetIdea(int ideaId)
        {
            return _ideationsRepository.GetIdea(ideaId);
        }

        // Updates title of the idea from given ID
        public void EditIdea(Idea idea, int ideaId)
        {
            Idea returnIdea = GetIdea(ideaId);
            returnIdea.Title = idea.Title;
            returnIdea.Published = false;
            _ideationsRepository.UpdateIdea(returnIdea);
            _unitOfWorkManager.Save();
        }

        // Deletes the idea from given ID
        public void DeleteIdea(int ideaId)
        {
            IoTManager ioTManager = new IoTManager(_unitOfWorkManager);
            Idea idea = GetIdea(ideaId);
            if (idea.IoTSetups != null)
            {
                foreach (var ioTSetup in idea.IoTSetups.ToList())
                {
                    ioTManager.DeleteIoTSetup(ioTSetup.Code);
                }
            }

            if (idea.Votes != null)
            {
                foreach (var vote in idea.Votes.ToList())
                {
                    this.DeleteVote(vote.VoteId);
                }
            }

            if (idea.Reactions != null)
            {
                foreach (var reaction in idea.Reactions.ToList())
                {
                    this.DeleteReaction(reaction.ReactionId);
                }
            }

            _ideationsRepository.RemoveIdea(idea);
            _unitOfWorkManager.Save();
        }
        
        // Gets the idea from given ID and add given position to that idea
        public void AddPosition(Position position, int ideaId)
        {
            
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            dataTypeManager.CreatePosition(position);
            

            Idea idea = GetIdea(ideaId);
            Idea editIdea = new Idea()
            {
                IdeaId = idea.IdeaId,
                Reported = idea.Reported,
                Title = idea.Title,
                IdentityUser = idea.IdentityUser,
                Ideation = idea.Ideation,
                IdeaObjects = idea.IdeaObjects,
                IoTSetups = idea.IoTSetups,
                Votes = idea.Votes,
                Reactions = idea.Reactions,
                
                Position = position,
            };
            _ideationsRepository.RemoveIdea(idea);
            _ideationsRepository.UpdateIdea(editIdea);
            _unitOfWorkManager.Save();
        }

        // Deletes the location from given ID from idea based on given ID
        public void DeleteLocationFromIdea(int ideaId, int positionId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            Idea idea = GetIdea(ideaId);
            idea.Position = null;
            _ideationsRepository.UpdateIdea(idea);
            dataTypeManager.DeletePosition(positionId);
            _unitOfWorkManager.Save();
        }
        

        #endregion

        #region IdeaObject

        // Returns ideaobject based on ID
        public IdeaObject GetIdeaObject(int ideaObjectId)
        {
            return _ideationsRepository.GetIdeaObject(ideaObjectId);
        }

        // Returns a list of all ideaobjects of an idea based on ID of idea
        public List<IdeaObject> GetIdeaObjects(int ideaId)
        {
            return _ideationsRepository.GetIdeaObjects(ideaId).ToList();
        }

        // Changes order of ideaobject based on ID's of ideaobject and idea and order changed by string
        public void OrderNrChange(int ideaObjectId, string changer, int ideaId)
        {
            IdeaObject ideaObject = GetIdeaObject(ideaObjectId);
            IList<IdeaObject> objects = GetIdeaObjects(ideaId);
            if (changer.Equals("up"))
            {
                foreach (var listObject in objects)
                {
                    if (listObject.OrderNr == ideaObject.OrderNr - 1)
                    {
                        listObject.OrderNr++;
                        ideaObject.OrderNr--;
                        EditIdeaObject(ideaObject);
                        EditIdeaObject(listObject);
                        break;
                    }
                }
            }
            else if (changer.Equals("down"))
            {
                foreach (var listObject in objects)
                {
                    if (listObject.OrderNr == ideaObject.OrderNr + 1)
                    {
                        listObject.OrderNr--;
                        ideaObject.OrderNr++;
                        EditIdeaObject(ideaObject);
                        EditIdeaObject(listObject);
                        break;
                    }
                }
            }

            _unitOfWorkManager.Save();
        }

        // Updates ideaobject based on given ideaobject
        public void EditIdeaObject(IdeaObject ideaObject)
        {
            if (ideaObject.GetType() == typeof(TextField))
            {
                TextField textField = (TextField) ideaObject;
                _ideationsRepository.EditTextField(textField);
            }

            if (ideaObject.GetType() == typeof(Video))
            {
                Video video = (Video) ideaObject;
                _ideationsRepository.EditVideo(video);
            }

            if (ideaObject.GetType() == typeof(Image))
            {
                Image image = (Image) ideaObject;
                _ideationsRepository.EditImage(image);
            }

            _unitOfWorkManager.Save();
        }

        #region TextField
            
        // Creates new textfield based on given textfield and ID of idea
            public void AddTextField(TextField textField, int ideaId)
        {
            Idea idea = GetIdea(ideaId);
            textField.Idea = idea;
            textField.OrderNr = GetIdeaObjects(ideaId).Count + 1;
            _ideationsRepository.AddTextField(textField);
            _unitOfWorkManager.Save();
        }

        // Updates textfield based on new given textfield and ID
        public void EditTextField(TextField textField, int textFieldId)
        {
            TextField editTextField = GetTextField(textFieldId);
            editTextField.Text = textField.Text;
            _ideationsRepository.EditTextField(editTextField);
            _unitOfWorkManager.Save();
        }

        // Deletes textfield from database from given ID
        public void DeleteTextField(int textFieldId)
        {
            TextField textField = GetTextField(textFieldId);
            List<IdeaObject> ideaObjects = GetIdeaObjects(textField.Idea.IdeaId);
            foreach (var ideaObject in ideaObjects)
            {
                if (ideaObject.OrderNr > textField.OrderNr)
                {
                    ideaObject.OrderNr--;
                    EditIdeaObject(ideaObject);
                }
            }

            _ideationsRepository.RemoveTextField(textField);
            _unitOfWorkManager.Save();
        }

        // Returns the textfield based on ID
        public TextField GetTextField(int textFieldId)
        {
            return _ideationsRepository.GetTextField(textFieldId);
        }
        #endregion
        
        #region VideoAllowed

        // Creates a new video from given video and adds it to idea of given ID
        public void AddVideo(Video video, int ideaId)
        {
            Idea idea = GetIdea(ideaId);
            video.Url = video.Url.Replace("watch?v=", "embed/");
            video.OrderNr = GetIdeaObjects(ideaId).Count + 1;
            video.Idea = idea;
            _ideationsRepository.AddVideo(video);
            _unitOfWorkManager.Save();
        }

        // Deletes video from database based on ID
        public void DeleteVideo(int videoId)
        {
            DataTypeManager dataTypeManager = new DataTypeManager(_unitOfWorkManager);
            Video video = GetVideo(videoId);
            List<IdeaObject> ideaObjects = GetIdeaObjects(video.Idea.IdeaId);
            foreach (var ideaObject in ideaObjects)
            {
                if (ideaObject.OrderNr > video.OrderNr)
                {
                    ideaObject.OrderNr--;
                    EditIdeaObject(ideaObject);
                }
            }

            _ideationsRepository.RemoveVideo(video);
            _unitOfWorkManager.Save();
        }

        // Returns video based on ID
        public Video GetVideo(int videoId)
        {
            return _ideationsRepository.GetVideo(videoId);
        }

        #endregion

        #region Images

        // Creates an image based on given name and path and add it to idea of given ID
        public void CreateImage(string name, string path, int ideaId)
        {
            Idea ideaToAddImageTo = GetIdea(ideaId);

            // Create image
            Image image = new Image
            {
                ImageName = name,
                ImagePath = path,
                Idea = ideaToAddImageTo
            };
            image.OrderNr = GetIdeaObjects(ideaId).Count + 1;
            // Add image to idea
            var ideaObject = GetIdeaObjects(ideaId);
            ideaToAddImageTo.IdeaObjects = ideaObject != null ? ideaObject.ToList() : new List<IdeaObject>();
            ideaToAddImageTo.IdeaObjects.Add(image);

            // Save in DB
            _ideationsRepository.CreateImage(image);
            ChangeIdea(ideaToAddImageTo);
            _unitOfWorkManager.Save();
        }

        // Returns a list of images from idea based on ID of idea
        public IEnumerable<Image> GetImages(int ideaId)
        {
            return _ideationsRepository.ReadImagesOfIdea(ideaId);
        }

        // Returns the image based on ID
        public Image GetImage(int imageId)
        {
            return _ideationsRepository.GetImage(imageId);
        }

        // Deletes the image from database based on ID
        public void DeleteImage(int imageId)
        {
            Image image = GetImage(imageId);
            List<IdeaObject> ideaObjects = GetIdeaObjects(image.Idea.IdeaId);
            foreach (var ideaObject in ideaObjects)
            {
                if (ideaObject.OrderNr > image.OrderNr)
                {
                    ideaObject.OrderNr--;
                    EditIdeaObject(ideaObject);
                }
            }

            _ideationsRepository.RemoveImage(image);
            _unitOfWorkManager.Save();
        }

        #endregion

        #endregion

        #region Like

        // Deletes the like from database based on ID
        public void DeleteLike(int likeId)
        {
            Like like = GetLike(likeId);
            _ideationsRepository.RemoveLike(like);
            _unitOfWorkManager.Save();
        }

        // Returns the like based on ID
        public Like GetLike(int likeId)
        {
            return _ideationsRepository.GetLike(likeId);
        }

        #endregion

        #region Reaction

        // Returns a list of all reactions of a platform based on ID of platform
        public IList<Reaction> GetAllReactions(int platformId)
        {
            return _ideationsRepository.GetAllReactions(platformId).ToList();
        }

        // Returns a list of all reactions of an idea based on ID of Idea
        public IList<Reaction> GetIdeaReactions(int id)
        {
            return _ideationsRepository.GetIdeaReactions(id).ToList();
        }

        // Returns a list of all reported reactions
        public IList<Reaction> GetReportedReactions(int projectId)
        {
            return _ideationsRepository.GetReportedReactions(projectId).ToList();
        }

        // Creates a new reaction based on arraylist and decides if reaction is for idea or ideation based on string
        // Sets reaction user and idea/ideation based on given ID's
        public void PostReaction(ArrayList parameters, int id, string userId, string element)
        {
            UsersManager usersManager = new UsersManager(_unitOfWorkManager);
            CustomUser identityUser = usersManager.GetUser(userId);
            Reaction reaction = new Reaction();
            reaction.IdentityUser = identityUser;
            reaction.ReactionText = parameters[0].ToString();

            if (element.Equals("idea"))
            {
                reaction.Idea = GetIdea(id);
            }
            else if (element.Equals("ideation"))
            {
                reaction.Ideation = GetIdeation(id);
            }

            _ideationsRepository.CreateReaction(reaction);
            _unitOfWorkManager.Save();
        }

        // Creates a new like for a reaction of given ID from user of given ID
        public void LikeReaction(int reactionId, string userId)
        {
            UsersManager usersManager = new UsersManager(_unitOfWorkManager);
            CustomUser identityUser = usersManager.GetUser(userId);
            Reaction reaction = _ideationsRepository.GetReaction(reactionId);

            Like like = new Like
            {
                Reaction = reaction, IdentityUser = identityUser
            };

            if (_ideationsRepository.CheckLike(reaction, identityUser))
            {
                _ideationsRepository.CreateLike(like);
                _unitOfWorkManager.Save();
            }
            else
            {
                throw new Exception("user has already liked this reaction");
            }
        }

        // Deletes the reaction of given ID from database
        public void DeleteReaction(int reactionId)
        {
            Reaction reaction = GetReaction(reactionId);
            if (reaction.Likes != null)
            {
                foreach (var like in reaction.Likes.ToList())
                {
                    this.DeleteLike(like.LikeId);
                }
            }

            _ideationsRepository.RemoveReaction(reaction);
            _unitOfWorkManager.Save();
        }

        // Returns the reaction based on ID
        public Reaction GetReaction(int reactionId)
        {
            return _ideationsRepository.GetReaction(reactionId);
        }

        #endregion

        #region Vote

        // Create a new vote based on idea and user of given ID's and give new vote the given votetype
        public void CreateVote(int ideaId, VoteType voteType, string userId)
        {
            Vote vote = new Vote();
            Idea idea = GetIdea(ideaId);
            if (userId != null)
            {
                UsersManager usersManager = new UsersManager(_unitOfWorkManager);
                CustomUser user = usersManager.GetUser(userId);

                if (_ideationsRepository.CheckUserVote(user, voteType, idea) == true)
                {
                    vote.IdentityUser = user;
                    vote.VoteType = voteType;
                    vote.Idea = idea;
                    vote.Confirmed = true;
                    _ideationsRepository.CreateVote(vote);
                    _unitOfWorkManager.Save();
                }
                else
                {
                    throw new Exception("user already voted in that type");
                }
            }
            else
            {
                vote.VoteType = voteType;
                vote.Idea = idea;
                _ideationsRepository.CreateVote(vote);
                _unitOfWorkManager.Save();
            }
        }
        
        // Checks if user already voted on Idea
        // Return true if user did not vote already
        // Return false if user already voted
        public bool CheckVote(string userId, VoteType voteType, int ideaId)
        {
            Idea idea = GetIdea(ideaId);
            foreach (var vote in idea.Votes)
            {
                if (vote.VoteType == voteType && vote.IdentityUser.Id.Equals(userId))
                {
                    return false;
                }
            }

            return true;
        }

        // Deletes vote from database based on ID
        public void DeleteVote(int voteId)
        {
            Vote vote = GetVote(voteId);
            _ideationsRepository.RemoveVote(vote);
            _unitOfWorkManager.Save();
        }

        // Returns vote based on ID
        public Vote GetVote(int voteId)
        {
            return _ideationsRepository.GetVote(voteId);
        }

        #endregion

        #region Tag

        // Returns a tag based on ID
        public Tag GetTag(int tagId)
        {
           return  _ideationsRepository.GetTag(tagId);
        }

        // Returns a list of all tags of an idea based on given ID of idea
        public List<Tag> GetTags(int ideaId)
        {
            List<Tag> tags = GetAllTags();
            Idea idea = GetIdea(ideaId);
            foreach (var ideaTag in idea.IdeaTags)
            {
                foreach (var tag in tags)
                {
                    if (ideaTag.Tag.TagId == tag.TagId)
                    {
                        tags.Remove(tag);
                        break;
                    }
                }
            }

            return tags;
        }

        // Returns a list of all tags
        public List<Tag> GetAllTags()
        {
            return _ideationsRepository.GetAllTags().ToList();
        }

        // Returns a list of all tags and sorted by: tagname
        public List<Tag> GetAllTagsBySort(string sortOrder)
        {
            IEnumerable<Tag> tags =  GetAllTags();
            switch (sortOrder)
            {
                case "name_desc":
                    tags = tags.OrderByDescending(t => t.TagName);
                    break;
                default:
                    tags = tags.OrderBy(t => t.TagName);
                    break;
            }
            return tags.ToList();
        }

        // Deletes ideatag from database based on ID
        public void DeleteIdeaTag(int ideaTagId)
        {
            IdeaTag ideaTag = GetIdeaTag(ideaTagId);
            _ideationsRepository.DeleteIdeaTag(ideaTag);
            _unitOfWorkManager.Save();
        }
        
        // Creates new ideatag based on ID's of idea and tag
        public void CreateIdeaTag(int ideaId, int tagId)
        {
            IdeaTag ideaTag = new IdeaTag()
            {
                Idea = GetIdea(ideaId),
                Tag = GetTag(tagId)
            };
            _ideationsRepository.CreateIdeaTag(ideaTag);
            _unitOfWorkManager.Save();
        }

        // Returns ideatag based on ID
        public IdeaTag GetIdeaTag(int ideaTagId)
        {
            return _ideationsRepository.GetIdeaTag(ideaTagId);
        }
        
        // Updates values of tag based on tag and ID
        public void EditTag(Tag tag, int tagId)
        {
            tag.TagId = tagId;
            _ideationsRepository.EditTag(tag);
            _unitOfWorkManager.Save();
        }

        // Deletes tag from database based on ID
        public void DeleteTag(int tagId)
        {
            Tag tag = GetTag(tagId);
            _ideationsRepository.DeleteTag(tag);
            _unitOfWorkManager.Save();
        }

        // Adds a tag to the database based on given tag
        public void AddTag(Tag tag)
        {
           _ideationsRepository.AddTag(tag);
           _unitOfWorkManager.Save();
        }
        #endregion

        #region Posts

        // Reports the reaction or idea based on given type
        public void ReportPost(int id, string type)
        {
            if (type.Equals("reaction"))
            {
                Reaction reaction = GetReaction(id);
                reaction.Reported = true;
                _ideationsRepository.UpdateReaction(reaction);
            }
            else
            {
                Idea idea = GetIdea(id);
                idea.Reported = true;
                _ideationsRepository.UpdateIdea(idea);
            }

            _unitOfWorkManager.Save();
        }
        
        // Updates reaction or idea based on given type
        public void PostCorrect(int id, string type)
        {
            if (type.Equals("reaction"))
            {
                Reaction reaction = GetReaction(id);
                reaction.Reported = false;
                _ideationsRepository.UpdateReaction(reaction);
            }
            else
            {
                Idea idea = GetIdea(id);
                idea.Reported = false;
                _ideationsRepository.UpdateIdea(idea);
            }

            _unitOfWorkManager.Save();
        }

        // Deletes the post from given ID based on type
        public void DeletePost(int id, string type)
        {
            if (type.Equals("reaction"))
            {
                DeleteReaction(id);
            }
            else
            {
                DeleteIdea(id);
            }

            _unitOfWorkManager.Save();
        }

        #endregion


        
    }
}