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
        throw new ArgumentNullException("unitOfWork");
      
      ctx = unitOfWork.ctx;
    }

    #region Ideation methods

    public IEnumerable<Ideation> GetIdeations(Phase phase)
        {
          return ctx.Ideations.Where(ideation => ideation.Phase == phase).AsEnumerable();
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

    #endregion

    #region Idea methods

     //Idea methods
        public IEnumerable<Idea> GetIdeas(Ideation ideation)
        {
          return ctx.Ideas.Where(idea => idea.Ideation == ideation ).AsEnumerable();
        }
        public Idea GetIdea(int ideaId)
        {
          return ctx.Ideas
            .Include(r => r.Reactions).ThenInclude(l => l.LoggedInUser)
            .Include(v => v.Votes)
            .Single(i => i.IdeaId == ideaId);
        }
        public Idea CreateIdea(Idea idea)
        {
          ctx.Ideas.Add(idea);
          ctx.SaveChanges();
          return idea;
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

    #endregion
    //Reaction methods

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
        #endregion

        public bool CheckUserVote(User user, VoteType voteType, Idea idea)
        {
          if (ctx.Votes.Where(v => v.Idea == idea).Where(v => v.User == user).Where(v => v.VoteType == voteType).AsEnumerable().Any())
          {
            return false;
          }
          else
          {
            return true;
          }
        }
  }

    
    
 
}
