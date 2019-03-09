using System;
using System.Collections;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Users;

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
        public IdeationsManager( UnitOfWorkManager unitOfWorkManager)
                 {
                     if (unitOfWorkManager == null)
                         throw new ArgumentNullException("unitOfWorkManager");

                     this.unitOfWorkManager = unitOfWorkManager;
                     ideationsRepository = new IdeationsRepository(this.unitOfWorkManager.UnitOfWork);
                 }
        public Ideation GetIdeation(int ideationId)
        {
            return ideationsRepository.GetIdeation(ideationId);
        }

        public void CreateIdeation(Ideation ideation)
        {
            ideationsRepository.CreateIdeation(ideation);
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

        public void PostReaction(ArrayList parameters, int ideaId)
        {
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            Reaction reaction = new Reaction();
            reaction.Idea = GetIdea(ideaId);
            reaction.LoggedInUser= usersManager.GetLoggedInUser(Int32.Parse(parameters[0].ToString()));
            reaction.ReactionText = parameters[1].ToString();
            ideationsRepository.CreateReaction(reaction);
            unitOfWorkManager.Save();
        }

        public void CreateVote(int ideaId, VoteType voteType, string userId = null)
        {
            Vote vote = new Vote();
            Idea idea = GetIdea(ideaId);
            if (userId != null)
            {                 
                UsersManager usersManager = new UsersManager(unitOfWorkManager);
                User user = usersManager.GetUser(Int32.Parse(userId));
                if (ideationsRepository.CheckUserVote(user, voteType, idea)==true)
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

        public void LikeReaction(int reactionId, string user)
        {
            Reaction reaction = ideationsRepository.GetReaction(reactionId);
            UsersManager usersManager = new UsersManager(unitOfWorkManager);
            LoggedInUser loggedInUser = usersManager.GetLoggedInUser(Int32.Parse(user));
            Like like = new Like{ Reaction = reaction, LoggedInUser = loggedInUser};
            if (ideationsRepository.CheckLike(reaction,loggedInUser) == true)
            {
                ideationsRepository.CreateLike(like);
                unitOfWorkManager.Save();
            }
            else
            {
                throw new Exception("user has already liked this reaction");
            }
           

        }

        public void CreateIoTVote(Vote vote)
        {
            ideationsRepository.CreateVote(vote);
        }
    }
}