using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class IdeationsRepository : IIdeationsRepository
    {
        private readonly CityOfIdeasDbContext _ctx;

        public IdeationsRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _ctx = unitOfWork.Ctx;
        }

        #region Ideation methods

        public IEnumerable<Ideation> GetIdeations(int phaseId)
        {
            return _ctx.Ideations
                .Where(ideation => ideation.Phase.PhaseId == phaseId)
                .AsEnumerable();
        }

        public IEnumerable<Ideation> GetAllIdeations(int platformId)
        {
            return _ctx.Ideations
                .Where(i => i.Phase.Project.Platform.PlatformId == platformId)
                .AsEnumerable();
        }

        public Ideation GetIdeation(int ideationId)
        {
            return _ctx.Ideations
                .Include(i => i.Ideas)
                .ThenInclude(r => r.Reactions)
                .Single(id => id.IdeationId == ideationId);
        }

        public Ideation CreateIdeation(Ideation ideation)
        {
            _ctx.Ideations.Add(ideation);
            _ctx.SaveChanges();
            return ideation;
        }
        
        public Ideation EditIdeation(Ideation ideation)
        {
            _ctx.Ideations.Update(ideation);
            _ctx.SaveChanges();
            return ideation;
        }

        public void RemoveIdeation(Ideation ideation)
        {
            _ctx.Ideations.Remove(ideation);
            _ctx.SaveChanges();
        }
        #endregion

        #region Idea methods

        //Idea methods
        public IEnumerable<Idea> GetIdeas(Ideation ideation)
        {
            return _ctx.Ideas.Where(idea => idea.Ideation == ideation).AsEnumerable();
        }
        
        public IEnumerable<Idea> GetAllIdeas(int platformId)
        {
            return _ctx.Ideas
                .Where(i => i.Ideation.Phase.Project.Platform.PlatformId == platformId)
                .AsEnumerable();
        }

        public Idea GetIdea(int ideaId)
        {
            return _ctx.Ideas
                .Include(r => r.Reactions).ThenInclude(l => l.IdentityUser)
                .Include(r => r.Reactions).ThenInclude(l => l.Likes)
                .Include(v => v.Votes)
                .Single(i => i.IdeaId == ideaId);
        }

        public Idea CreateIdea(Idea idea)
        {
            _ctx.Ideas.Add(idea);
            _ctx.SaveChanges();
            return idea;
        }

        public void UpdateIdea(Idea idea)
        {
            _ctx.Entry(idea).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        public void RemoveIdea(Idea idea)
        {
            _ctx.Ideas.Remove(idea);
            _ctx.SaveChanges();
        }
        
        #endregion

        #region Reaction methods

        public IEnumerable<Reaction> GetAllReactions(int platformId)
        {
            return _ctx.Reactions
                .Where(r => r.Ideation.Phase.Project.Platform.PlatformId == platformId || r.Idea.Ideation.Phase.Project.Platform.PlatformId == platformId)
                .AsEnumerable();
        }
        
        public IEnumerable<Reaction> GetReactionsOnIdeation(Ideation ideation)
        {
            return _ctx.Reactions.Where(reaction => reaction.Ideation == ideation).AsEnumerable();
        }

        public IEnumerable<Reaction> GetReactionsOnIdea(Idea idea)
        {
            return _ctx.Reactions.Where(reaction => reaction.Idea == idea).AsEnumerable();
        }

        public Reaction GetReaction(int reactionId)
        {
            return _ctx.Reactions.Find(reactionId);
        }

        public Reaction CreateReaction(Reaction reaction)
        {
            _ctx.Reactions.Add(reaction);
            _ctx.SaveChanges();
            return reaction;
        }

        public void RemoveReaction(Reaction reaction)
        {
            _ctx.Reactions.Remove(reaction);
            _ctx.SaveChanges();
        }
        #endregion

        #region Vote methods

        //Vote methods
        public IEnumerable<Vote> GetVotes()
        {
            return _ctx.Votes.AsEnumerable();
        }

        public Vote GetVote(int voteId)
        {
            return _ctx.Votes.Find(voteId);
        }

        public Vote CreateVote(Vote vote)
        {
            /*Idea idea = GetIdea(vote.Idea.IdeaId);
            idea.Votes.Add(vote);
            ctx.Ideas.Update(idea);*/
            _ctx.Votes.Add(vote);
            _ctx.SaveChanges();
            return vote;
        }

        public bool CheckUserVote(IdentityUser user, VoteType voteType, Idea idea)
        {
            if (_ctx.Votes.Where(v => v.Idea == idea).Where(v => v.IdentityUser == user).Where(v => v.VoteType == voteType)
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
            _ctx.Votes.Remove(vote);
            _ctx.SaveChanges();
        }

        #endregion

        #region Like methods

        public bool CheckLike(Reaction reaction, IdentityUser loggedInUser)
        {
            if (_ctx.Likes.Where(l => l.Reaction == reaction).Where(l => l.IdentityUser == loggedInUser).AsEnumerable()
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
            _ctx.Likes.Add(like);
            _ctx.SaveChanges();
            return like;
        }

        public Like GetLike(int likeId)
        {
            return _ctx.Likes.Find(likeId);
        }

        public void RemoveLike(Like like)
        {
            _ctx.Likes.Remove(like);
            _ctx.SaveChanges();
        }
        #endregion
    }
}