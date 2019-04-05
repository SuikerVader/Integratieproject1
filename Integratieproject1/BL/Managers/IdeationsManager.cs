using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Integratieproject1.BL.Managers
{
    public class IdeationsManager : IIdeationsManager
    {
        private IdeationsRepository ideationsRepository;
        private UnitOfWorkManager unitOfWorkManager;

        public IdeationsManager()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            ideationsRepository = new IdeationsRepository(unitOfWorkManager.UnitOfWork);
        }

        public IdeationsManager(UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            this.unitOfWorkManager = unitOfWorkManager;
            ideationsRepository = new IdeationsRepository(this.unitOfWorkManager.UnitOfWork);
        }

        #region Ideation

        public Ideation GetIdeation(int ideationId)
        {
            return ideationsRepository.GetIdeation(ideationId);
        }

        public IList<Ideation> GetIdeations(int phaseId)
        {
            return ideationsRepository.GetIdeations(phaseId).ToList();
        }

        public void CreateIdeation(Ideation ideation, int phaseId)
        {
            ProjectsManager projectsManager = new ProjectsManager(unitOfWorkManager);
            Phase phase = projectsManager.GetPhase(phaseId);
            ideation.Phase = phase;
            ideationsRepository.CreateIdeation(ideation);
            unitOfWorkManager.Save();
        }

        public Ideation EditIdeation(Ideation ideation, int ideationId)
        {
            ideation.IdeationId = ideationId;
            //ideation.Phase = GetIdeation(ideationId).Phase;
            ideationsRepository.EditIdeation(ideation);
            unitOfWorkManager.Save();
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

            ideationsRepository.RemoveIdeation(ideation);
            unitOfWorkManager.Save();
        }

        #endregion

        #region Idea

        public void PostIdea(ArrayList parameters, string fileName, int ideationId)
        {
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            Idea idea = new Idea();
            idea.Ideation = GetIdeation(ideationId); 
            idea.LoggedInUser = usersManager.GetLoggedInUser(Int32.Parse(parameters[0].ToString()));
            idea.Title = parameters[1].ToString();
            idea.Text = parameters[2].ToString();
            idea.Image = fileName;
            idea.Video = parameters[3].ToString();
            
            ideationsRepository.CreateIdea(idea);
            unitOfWorkManager.Save();
        }
        
        public void CreateIdea(Idea idea)
        {
            ideationsRepository.CreateIdea(idea);
            unitOfWorkManager.Save();
        }

        public Idea GetIdea(int ideaId)
        {
            return ideationsRepository.GetIdea(ideaId);
        }

        public void DeleteIdea(int ideaId)
        {
            IoTManager ioTManager = new IoTManager(unitOfWorkManager);
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

            ideationsRepository.RemoveIdea(idea);
            unitOfWorkManager.Save();
        }

        #endregion

        #region Like

        public void DeleteLike(int likeId)
        {
            Like like = Getlike(likeId);
            ideationsRepository.RemoveLike(like);
            unitOfWorkManager.Save();
        }

        public Like Getlike(int likeId)
        {
            return ideationsRepository.GetLike(likeId);
        }

        #endregion

        #region Reaction

        public void PostReaction(ArrayList parameters, int ideaId)
        {
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            Reaction reaction = new Reaction();
            reaction.Idea = GetIdea(ideaId);
            reaction.LoggedInUser = usersManager.GetLoggedInUser(Int32.Parse(parameters[0].ToString()));
            reaction.ReactionText = parameters[1].ToString();
            ideationsRepository.CreateReaction(reaction);
            unitOfWorkManager.Save();
        }

        public void LikeReaction(int reactionId, string user)
        {
            Reaction reaction = ideationsRepository.GetReaction(reactionId);
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            LoggedInUser loggedInUser = usersManager.GetLoggedInUser(Int32.Parse(user));
            Like like = new Like {Reaction = reaction, LoggedInUser = loggedInUser};
            if (ideationsRepository.CheckLike(reaction, loggedInUser) == true)
            {
                ideationsRepository.CreateLike(like);
                unitOfWorkManager.Save();
            }
            else
            {
                throw new Exception("user has already liked this reaction");
            }
        }

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

            ideationsRepository.RemoveReaction(reaction);
            unitOfWorkManager.Save();
        }

        public Reaction GetReaction(int reactionId)
        {
            return ideationsRepository.GetReaction(reactionId);
        }

        #endregion

        #region Vote

        public void CreateVote(int ideaId, VoteType voteType, string userId = null)
        {
            Vote vote = new Vote();
            Idea idea = GetIdea(ideaId);
            if (userId != null)
            {
                UsersManager usersManager = new UsersManager(unitOfWorkManager);
                User user = usersManager.GetUser(Int32.Parse(userId));
                if (ideationsRepository.CheckUserVote(user, voteType, idea) == true)
                {
                    vote.User = user;
                    vote.VoteType = voteType;
                    vote.Idea = idea;
                    ideationsRepository.CreateVote(vote);
                    unitOfWorkManager.Save();
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
                ideationsRepository.CreateVote(vote);
                unitOfWorkManager.Save();
            }
        }

        private void DeleteVote(int voteId)
        {
            Vote vote = GetVote(voteId);
            ideationsRepository.RemoveVote(vote);
            unitOfWorkManager.Save();
        }

        private Vote GetVote(int voteId)
        {
            return ideationsRepository.GetVote(voteId);
        }

        #endregion
    }
}