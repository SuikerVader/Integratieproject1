using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.BL.Managers
{
    public class IdeationsManager : IIdeationsManager
    {
        private readonly IdeationsRepository _ideationsRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly UsersManager _usersManager;
        private readonly DataTypeManager _dataTypeManager;

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
            ideation.IdeationId = ideationId;
            //ideation.Phase = GetIdeation(ideationId).Phase;
            _ideationsRepository.EditIdeation(ideation);
            _unitOfWorkManager.Save();
            return ideation;
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
        public IList<Idea> GetReportedIdeas(int projectId)
        {
            return _ideationsRepository.GetReportedIdeas(projectId).ToList();
        }
        
        public Idea PostIdea(ArrayList parameters, int ideationId, string userId)
        {
            Idea idea = new Idea
            {
                Ideation = GetIdeation(ideationId),
                IdentityUser = _usersManager.GetUser(userId),
                Title = parameters[0].ToString(),
                Text = parameters[1].ToString(),
                Video = parameters[2].ToString().Replace("watch?v=", "embed/")
            };

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

        private void DeleteIdea(int ideaId)
        {
            IoTManager ioTManager = new IoTManager(_unitOfWorkManager, new SurveysManager());
            Idea idea = GetIdea(ideaId);
            if (idea.IoTSetups != null)
            {
                foreach (var ioTSetup in idea.IoTSetups.ToList())
                {
                    ioTManager.DeleteIoTSetup(ioTSetup);
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
            ProjectsManager projectsManager = new ProjectsManager();
            IdentityUser identityUser = projectsManager.GetUser(userId);
            Reaction reaction = new Reaction();
            reaction.IdentityUser = identityUser;
            reaction.ReactionText = parameters[0].ToString();
            
            if (element.Equals("idea"))
            {
              reaction.Idea = GetIdea(id);
            }else if (element.Equals("ideation"))
            {
                reaction.Ideation = GetIdeation(id);
            }

            _ideationsRepository.CreateReaction(reaction);
            _unitOfWorkManager.Save();
        }

        public void LikeReaction(int reactionId, string user, string userId)
        {
            ProjectsManager projectsManager = new ProjectsManager();
            IdentityUser identityUser = projectsManager.GetUser(userId);
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
                ProjectsManager  projectsManager = new ProjectsManager();
                IdentityUser user = projectsManager.GetUser(userId);
                
                if (_ideationsRepository.CheckUserVote(user, voteType, idea) == true)
                {
                    vote.IdentityUser = user;
                    vote.VoteType = voteType;
                    vote.Idea = idea;
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


        
    }
}