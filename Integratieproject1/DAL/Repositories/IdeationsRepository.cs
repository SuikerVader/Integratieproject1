using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class IdeationsRepository : IIdeationsRepository
    {
        private readonly CityOfIdeasDbContext ctx;


        public IdeationsRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            ctx = unitOfWork.ctx;
        }

        #region Ideation methods

        public IEnumerable<Ideation> GetIdeations(int phaseId)
        {
            return ctx.Ideations.Where(ideation => ideation.Phase.PhaseId == phaseId).AsEnumerable();
        }

        public Ideation GetIdeation(int ideationId)
        {
            return ctx.Ideations
                .Include(i => i.Ideas)
                .ThenInclude(r => r.Reactions)
                .Single(id => id.IdeationId == ideationId);
        }

        public Ideation CreateIdeation(Ideation ideation)
        {
            ctx.Ideations.Add(ideation);
            ctx.SaveChanges();
            return ideation;
        }
        
        public Ideation EditIdeation(Ideation ideation)
        {
            ctx.Ideations.Update(ideation);
            ctx.SaveChanges();
            return ideation;
        }

        public void RemoveIdeation(Ideation ideation)
        {
            ctx.Ideations.Remove(ideation);
            ctx.SaveChanges();
        }
        #endregion

        #region Idea methods

        //Idea methods
        public IEnumerable<Idea> GetIdeas(Ideation ideation)
        {
            return ctx.Ideas.Where(idea => idea.Ideation == ideation).AsEnumerable();
        }

        public Idea GetIdea(int ideaId)
        {
            return ctx.Ideas
                .Include(r => r.Reactions).ThenInclude(l => l.LoggedInUser)
                .Include(r => r.Reactions).ThenInclude(l => l.Likes)
                .Include(v => v.Votes)
                .Single(i => i.IdeaId == ideaId);
        }

        public Idea CreateIdea(Idea idea)
        {
            ctx.Ideas.Add(idea);
            ctx.SaveChanges();
            return idea;
        }

        public void RemoveIdea(Idea idea)
        {
            ctx.Ideas.Remove(idea);
            ctx.SaveChanges();
        }
        #endregion

        #region Reaction methods

        public IEnumerable<Reaction> GetReactionsOnIdeation(Ideation ideation)
        {
            return ctx.Reactions.Where(reaction => reaction.Ideation == ideation).AsEnumerable();
        }

        public IEnumerable<Reaction> GetReactionsOnIdea(Idea idea)
        {
            return ctx.Reactions.Where(reaction => reaction.Idea == idea).AsEnumerable();
        }

        public Reaction GetReaction(int reactionId)
        {
            return ctx.Reactions.Find(reactionId);
        }

        public Reaction CreateReaction(Reaction reaction)
        {
            ctx.Reactions.Add(reaction);
            ctx.SaveChanges();
            return reaction;
        }

        public void RemoveReaction(Reaction reaction)
        {
            ctx.Reactions.Remove(reaction);
            ctx.SaveChanges();
        }
        #endregion


        #region Vote methods

        //Vote methods
        public IEnumerable<Vote> GetVotes()
        {
            return ctx.Votes.AsEnumerable();
        }

        public Vote GetVote(int voteId)
        {
            return ctx.Votes.Find(voteId);
        }

        public Vote CreateVote(Vote vote)
        {
            /*Idea idea = GetIdea(vote.Idea.IdeaId);
            idea.Votes.Add(vote);
            ctx.Ideas.Update(idea);*/
            ctx.Votes.Add(vote);
            ctx.SaveChanges();
            return vote;
        }

        public bool CheckUserVote(User user, VoteType voteType, Idea idea)
        {
            if (ctx.Votes.Where(v => v.Idea == idea).Where(v => v.User == user).Where(v => v.VoteType == voteType)
                .AsEnumerable().Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void RemoveVote(Vote vote)
        {
            ctx.Votes.Remove(vote);
            ctx.SaveChanges();
        }

        #endregion

        #region Like

        public bool CheckLike(Reaction reaction, LoggedInUser loggedInUser)
        {
            if (ctx.Likes.Where(l => l.Reaction == reaction).Where(l => l.LoggedInUser == loggedInUser).AsEnumerable()
                .Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Like CreateLike(Like like)
        {
            ctx.Likes.Add(like);
            ctx.SaveChanges();
            return like;
        }

        public Like GetLike(int likeId)
        {
            return ctx.Likes.Find(likeId);
        }

        public void RemoveLike(Like like)
        {
            ctx.Likes.Remove(like);
            ctx.SaveChanges();
        }
        #endregion


       
    }
}