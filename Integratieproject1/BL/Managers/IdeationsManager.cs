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
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            _unitOfWorkManager = unitOfWorkManager;
            _ideationsRepository = new IdeationsRepository(_unitOfWorkManager.UnitOfWork);
        }


        #region Posts

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


        #region Ideation

        public Ideation GetIdeation(int ideationId)
        {
            return _ideationsRepository.GetIdeation(ideationId);
        }

        public IList<Ideation> GetProjectIdeation(int projectId)
        {
            return _ideationsRepository.GetProjectsIdeations(projectId).ToList();
        }

        public IList<Ideation> GetIdeations(int phaseId)
        {
            return _ideationsRepository.GetIdeations(phaseId).ToList();
        }

        public IList<Ideation> GetAllIdeations(int platformId)
        {
            return _ideationsRepository.GetAllIdeations(platformId).ToList();
        }

        public void CreateIdeation(Ideation ideation, int phaseId)
        {
            ProjectsManager projectsManager = new ProjectsManager(_unitOfWorkManager);
            Phase phase = projectsManager.GetPhase(phaseId);
            ideation.Phase = phase;
            _ideationsRepository.CreateIdeation(ideation);
            _unitOfWorkManager.Save();
        }

        public Ideation EditIdeation(Ideation ideation, int ideationId)
        {
            Ideation originalIdeation = GetIdeation(ideationId);
            originalIdeation.CentralQuestion = ideation.CentralQuestion;
            originalIdeation.InputIdeation = ideation.InputIdeation;
            originalIdeation.ExternalLink = ideation.ExternalLink;
            originalIdeation.Text = ideation.Text;
            originalIdeation.TextRequired = ideation.TextRequired;
            originalIdeation.Image = ideation.Image;
            originalIdeation.ImageRequired = ideation.ImageRequired;
            originalIdeation.Video = ideation.Video;
            originalIdeation.VideoRequired = ideation.VideoRequired;
            originalIdeation.Map = ideation.Map;
            originalIdeation.MapRequired = ideation.MapRequired;
            foreach (Idea idea in originalIdeation.Ideas.ToList())
            {
                if (!originalIdeation.Map && idea.Position != null)
                {
                    _dataTypeManager.DeletePosition(idea.Position);
                    idea.Position = null;
                }
                foreach (IdeaObject ideaObject in idea.IdeaObjects.ToList())
                {
                    if (!originalIdeation.Text && ideaObject.GetType() == typeof(TextField))
                    {
                        DeleteTextField(ideaObject.IdeaObjectId);
                    }
                    if (!originalIdeation.Image && ideaObject.GetType() == typeof(Image))
                    {
                        DeleteImage(ideaObject.IdeaObjectId);
                    }
                    if (!originalIdeation.Video && ideaObject.GetType() == typeof(Video))
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

        public IList<Idea> GetAllIdeas(int platformId)
        {
            return _ideationsRepository.GetAllIdeas(platformId).ToList();
        }

        public IList<Idea> GetIdeas(int ideationId)
        {
            return _ideationsRepository.GetIdeas(ideationId).ToList();
        }
        
        public IList<Idea> GetOtherIdeas(int ideationId)
        {
            Ideation ideation = GetIdeation(ideationId);
            List<Idea> ideas = ideation.Ideas.ToList();

            return ideas;
        }

        public IList<Idea> GetReportedIdeas(int projectId)
        {
            return _ideationsRepository.GetReportedIdeas(projectId).ToList();
        }

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

        public Idea PostIdea(ArrayList parameters, int ideationId, string userId)
        {
            Idea idea = new Idea
            {
                Ideation = GetIdeation(ideationId),
                IdentityUser = _usersManager.GetUser(userId),
                Title = parameters[0].ToString(),
            };
            TextField textField = new TextField
            {
                Text = parameters[1].ToString(),
            };
            Video video = new Video
            {
                Url = parameters[2].ToString().Replace("watch?v=", "embed/"),
            };
            idea.IdeaObjects = new List<IdeaObject>() {textField, video};
            return CreateIdea(idea);
        }

        public Idea CreateIdea(Idea idea)
        {
            Idea created = _ideationsRepository.CreateIdea(idea);
            _unitOfWorkManager.Save();
            return created;
        }

        public void ChangeIdea(Idea idea)
        {
            _ideationsRepository.UpdateIdea(idea);
            _unitOfWorkManager.Save();
        }

        public Idea GetIdea(int ideaId)
        {
            return _ideationsRepository.GetIdea(ideaId);
        }


        public void EditIdea(Idea idea, int ideaId)
        {
            Idea returnIdea = GetIdea(ideaId);
            returnIdea.Title = idea.Title;
            _ideationsRepository.UpdateIdea(returnIdea);
            _unitOfWorkManager.Save();
        }


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
        

        #endregion

        #region IdeaObject

        public IdeaObject GetIdeaObject(int ideaObjectId)
        {
            return _ideationsRepository.GetIdeaObject(ideaObjectId);
        }

        public List<IdeaObject> GetIdeaObjects(int ideaId)
        {
            return _ideationsRepository.GetIdeaObjects(ideaId).ToList();
        }

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

        private void EditIdeaObject(IdeaObject ideaObject)
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
            
            public void AddTextField(TextField textField, int ideaId)
        {
            Idea idea = GetIdea(ideaId);
            textField.Idea = idea;
            textField.OrderNr = GetIdeaObjects(ideaId).Count + 1;
            _ideationsRepository.AddTextField(textField);
            _unitOfWorkManager.Save();
        }

        public void EditTextField(TextField textField, int textFieldId)
        {
            TextField editTextField = GetTextField(textFieldId);
            editTextField.Text = textField.Text;
            _ideationsRepository.EditTextField(editTextField);
            _unitOfWorkManager.Save();
        }

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

        private TextField GetTextField(int textFieldId)
        {
            return _ideationsRepository.GetTextField(textFieldId);
        }
        #endregion
        
        #region Video

        public void AddVideo(Video video, int ideaId)
        {
            Idea idea = GetIdea(ideaId);
            video.Url = video.Url.Replace("watch?v=", "embed/");
            video.OrderNr = GetIdeaObjects(ideaId).Count + 1;
            video.Idea = idea;
            _ideationsRepository.AddVideo(video);
            _unitOfWorkManager.Save();
        }

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

        private Video GetVideo(int videoId)
        {
            return _ideationsRepository.GetVideo(videoId);
        }

        #endregion

        #region Images

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

        public IEnumerable<Image> GetImages(int ideaId)
        {
            return _ideationsRepository.ReadImagesOfIdea(ideaId);
        }

        public Image GetImage(int imageId)
        {
            return _ideationsRepository.GetImage(imageId);
        }

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

        private void DeleteLike(int likeId)
        {
            Like like = GetLike(likeId);
            _ideationsRepository.RemoveLike(like);
            _unitOfWorkManager.Save();
        }

        private Like GetLike(int likeId)
        {
            return _ideationsRepository.GetLike(likeId);
        }

        #endregion

        #region Reaction

        public IList<Reaction> GetAllReactions(int platformId)
        {
            return _ideationsRepository.GetAllReactions(platformId).ToList();
        }

        public IList<Reaction> GetReportedReactions(int projectId)
        {
            return _ideationsRepository.GetReportedReactions(projectId).ToList();
        }

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

        private void DeleteReaction(int reactionId)
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

        public Reaction GetReaction(int reactionId)
        {
            return _ideationsRepository.GetReaction(reactionId);
        }

        #endregion

        #region Vote

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

        private void DeleteVote(int voteId)
        {
            Vote vote = GetVote(voteId);
            _ideationsRepository.RemoveVote(vote);
            _unitOfWorkManager.Save();
        }

        private Vote GetVote(int voteId)
        {
            return _ideationsRepository.GetVote(voteId);
        }

        #endregion


        #region Tag

        public Tag GetTag(int tagId)
        {
           return  _ideationsRepository.GetTag(tagId);
        }
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

        public List<Tag> GetAllTags()
        {
            return _ideationsRepository.GetAllTags().ToList();
        }

        public void DeleteIdeaTag(int ideaTagId)
        {
            IdeaTag ideaTag = GetIdeaTag(ideaTagId);
            _ideationsRepository.DeleteIdeaTag(ideaTag);
            _unitOfWorkManager.Save();
        }
        
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


        private IdeaTag GetIdeaTag(int ideaTagId)
        {
            return _ideationsRepository.GetIdeaTag(ideaTagId);
        }
        
        public void EditTag(Tag tag, int tagId)
        {
            tag.TagId = tagId;
            _ideationsRepository.EditTag(tag);
            _unitOfWorkManager.Save();
        }

        public void DeleteTag(int tagId)
        {
            Tag tag = GetTag(tagId);
            _ideationsRepository.DeleteTag(tag);
            _unitOfWorkManager.Save();
        }

        public void AddTag(Tag tag)
        {
           _ideationsRepository.AddTag(tag);
           _unitOfWorkManager.Save();
        }
        #endregion


       
    }
}