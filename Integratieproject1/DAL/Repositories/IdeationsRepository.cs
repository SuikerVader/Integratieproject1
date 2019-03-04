using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;

namespace Integratieproject1.DAL.Repositories
{
  public class IdeationsRepository
  {
    private readonly CityOfIdeasDbContext ctx;

   
    public IdeationsRepository(UnitOfWork unitOfWork)
    {
      if (unitOfWork == null)
        throw new ArgumentNullException("unitOfWork");
      
      ctx = unitOfWork.ctx;
    }
    
    // Ideation methods
    public IEnumerable<Ideation> GetIdeations(Phase phase)
    {
      return ctx.Ideations.Where(ideation => ideation.Phase == phase).AsEnumerable();
    }
    public Ideation GetIdeation(int ideationId)
    {
      return ctx.Ideations.Find(ideationId);
    }
    public Ideation CreateIdeation(Ideation ideation)
    {
      ctx.Ideations.Add(ideation);
      ctx.SaveChanges();
      return ideation;
    }
    
    //Idea methods
    public IEnumerable<Idea> GetIdeas(Ideation ideation)
    {
      return ctx.Ideas.Where(idea => idea.Ideation == ideation ).AsEnumerable();
    }
    public Idea GetIdea(int ideaId)
    {
      return ctx.Ideas.Find(ideaId);
    }
    public Idea CreateIdea(Idea idea)
    {
      ctx.Ideas.Add(idea);
      ctx.SaveChanges();
      return idea;
    }
    
    //Reaction methods
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
      ctx.Votes.Add(vote);
      ctx.SaveChanges();
      return vote;
    }
  }
 
}
