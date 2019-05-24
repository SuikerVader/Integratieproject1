using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Datatypes;
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

        // Returns enumerable of all ideations of phase based on ID of phase
        public IEnumerable<Ideation> GetIdeations(int phaseId)
        {
            return _ctx.Ideations
                .Where(ideation => ideation.Phase.PhaseId == phaseId)
                .AsEnumerable();
        }

        // Returns enumerable of all ideations of project based on ID of project
        public IEnumerable<Ideation> GetProjectsIdeations(int projectId)
        {
            return _ctx.Ideations
                .Include(ph => ph.Phase)
                .Where(ideation => ideation.Phase.Project.ProjectId == projectId)
                .Include(i=>i.Ideas).ThenInclude(id =>id.IdeaObjects )
                .Include(i => i.Ideas).ThenInclude(v => v.Votes)
                .Include(i => i.Ideas).ThenInclude(r => r.Reactions)
                .AsEnumerable();
        }

        // Returns enumerable of all ideations
        public IEnumerable<Ideation> GetAllIdeations()
        {
            return _ctx.Ideations
                .Include(i => i.Phase).ThenInclude(p => p.Project)
                .AsEnumerable();
        }

        // Returns enumerable of all ideations of platform based on ID of platform
        public IEnumerable<Ideation> GetIdeationsByPlatform(int platformId)
        {
            return _ctx.Ideations
                .Where(i => i.Phase.Project.Platform.PlatformId == platformId)
                .Include(i => i.Phase).ThenInclude(p => p.Project)
                .AsEnumerable();
        }

        // Returns ideation based on ID
        public Ideation GetIdeation(int ideationId)
        {
            return _ctx.Ideations
                .Include(i => i.Ideas).ThenInclude(r => r.Reactions)
                .Include(i => i.Ideas).ThenInclude(v => v.Votes)
                .Include(i => i.Ideas).ThenInclude(im => im.IdeaObjects)
                .Include(i => i.Ideas).ThenInclude(u => u.IdentityUser)
                .Include(p => p.Phase).ThenInclude(p => p.Project).ThenInclude(pl => pl.Platform)
                .Include(r => r.Reactions).ThenInclude(l => l.Likes)
                .Include(r => r.Reactions).ThenInclude(u => u.IdentityUser)
                .Single(id => id.IdeationId == ideationId);
        }

        // Creates ideation based on given ideation
        // Returns new ideation
        public Ideation CreateIdeation(Ideation ideation)
        {
            _ctx.Ideations.Add(ideation);
            _ctx.SaveChanges();
            return ideation;
        }

        // Updates ideation based on given ideation
        // Returns updated ideation
        public Ideation EditIdeation(Ideation ideation)
        {
            _ctx.Ideations.Update(ideation);
            _ctx.SaveChanges();
            return ideation;
        }


        // Deletes given ideation from database
        public void RemoveIdeation(Ideation ideation)
        {
            _ctx.Ideations.Remove(ideation);
            _ctx.SaveChanges();
        }

        #endregion

        #region Idea methods

        //Idea methods

        // Returns enumerable of all ideas of ideation based on ID of ideation
        public IEnumerable<Idea> GetIdeas(int ideationId)
        {
            return _ctx.Ideas.Where(idea => idea.Ideation.IdeationId == ideationId).AsEnumerable();
        }

        // Returns enumerable of all ideas of platform based on ID of platform
        public IEnumerable<Idea> GetAllIdeas(int platformId)
        {
            return _ctx.Ideas
                .Where(i => i.Ideation.Phase.Project.Platform.PlatformId == platformId)
                .Include(i => i.IdeaObjects)
                .Include(i => i.IdeaTags).ThenInclude(i => i.Tag)
                .AsEnumerable();
        }

        // Returns enumerable of all ideas of user based on ID of user
        public IEnumerable<Idea> GetIdeasByUser(string currentUserId)
        {
            return _ctx.Ideas
                .Where(i => i.IdentityUser.Id == currentUserId)
                .Include(i => i.Ideation).ThenInclude(i => i.Phase).ThenInclude(p => p.Project)
                .AsEnumerable();
        }

        // Returns enumerable of all ideas that are not yet published
        public IEnumerable<Idea> GetAllNonPublishedIdeas()
        {
            return _ctx.Ideas
                .Include(i => i.IdeaObjects)
                .Include(i => i.IdeaTags).ThenInclude(i => i.Tag)
                .Include(i => i.Ideation)
                .Include(i => i.IdentityUser)
                .Where(i => i.Published == false)
                .AsEnumerable();
        }

        // Returns idea based on ID
        public Idea GetIdea(int ideaId)
        {
            return _ctx.Ideas
                .Include(r => r.Reactions).ThenInclude(l => l.IdentityUser)
                .Include(r => r.Reactions).ThenInclude(l => l.Likes)
                .Include(v => v.Votes).ThenInclude(v => v.IdentityUser)
                .Include(i => i.IdeaObjects)
                .Include(i => i.IdentityUser)
                .Include(i => i.Position)
                .Include(i => i.Ideation).ThenInclude(id => id.Phase).ThenInclude(p => p.Project).ThenInclude(pl => pl.Platform)
                .Include(i => i.IdeaTags).ThenInclude(it => it.Tag)
                .Include(i => i.IoTSetups).ThenInclude(i => i.Position)
                .Single(i => i.IdeaId == ideaId);
        }

        // Returns enumerable of all reported ideas of project based on ID of project
        public IEnumerable<Idea> GetReportedIdeas(int projectId)
        {
            return _ctx.Ideas
                .Where(i => i.Ideation.Phase.Project.ProjectId == projectId)
                .Where(i => i.Reported == true)
                .Include(i => i.Ideation)
                .Include(i => i.IdentityUser)
                .AsEnumerable();
        }

        // Creates new idea based on given idea
        // Return new idea
        public Idea CreateIdea(Idea idea)
        {
            _ctx.Ideas.Add(idea);
            _ctx.SaveChanges();
            return idea;
        }

        // Updates idea based on given idea
        public void UpdateIdea(Idea idea)
        {
            _ctx.Ideas.Update(idea);
            _ctx.SaveChanges();
        }

        // Publishes given idea
        public void PublishIdea(Idea idea)
        {
            idea.Published = true;
            _ctx.Ideas.Update(idea);
            _ctx.SaveChanges();
        }

        // Deletes given idea from database
        public void RemoveIdea(Idea idea)
        {
            _ctx.Ideas.Remove(idea);
            _ctx.SaveChanges();
        }


        #endregion

        #region IdeaObject methods

        // Returns enumerable of all ideaobjects of idea based on ID of idea
        public IEnumerable<IdeaObject> GetIdeaObjects(int ideaId)
        {
            return _ctx.IdeaObjects.Where(i => i.Idea.IdeaId == ideaId).AsEnumerable();
        }

        // Returns ideaobject based on ID
        public IdeaObject GetIdeaObject(int ideaObjectId)
        {
            return _ctx.IdeaObjects.Include(i => i.Idea).Single(i => i.IdeaObjectId == ideaObjectId);
        }

        #region Images

        public Image CreateImage(Image image)
        {
            _ctx.Images.Add(image);
            _ctx.SaveChanges();
            return image;
        }

        public IEnumerable<Image> ReadImagesOfIdea(int ideaId)
        {
            return _ctx.Images.Where(i => i.Idea.IdeaId == ideaId).Where(i => i.GetType() == typeof(Image))
                .AsEnumerable().Cast<Image>();
        }

        public Image GetImage(int imageId)
        {
            return _ctx.Images.Include(i => i.Idea).Single(i => i.IdeaObjectId == imageId);
        }

        public void RemoveImage(Image image)
        {
            _ctx.Images.Remove(image);
            _ctx.SaveChanges();
        }

        public void EditImage(Image image)
        {
            _ctx.Images.Update(image);
            _ctx.SaveChanges();
        }

        #endregion

        #region VideoAllowed

        public Video GetVideo(int videoId)
        {
            return _ctx.Videos.Include(v => v.Idea).Single(v => v.IdeaObjectId == videoId);
        }

        public void AddVideo(Video video)
        {
            _ctx.Videos.Add(video);
            _ctx.SaveChanges();
        }

        public void EditVideo(Video video)
        {
            _ctx.Videos.Update(video);
            _ctx.SaveChanges();
        }

        public void RemoveVideo(Video video)
        {
            _ctx.Videos.Remove(video);
            _ctx.SaveChanges();
        }

        #endregion

        #region TextField

        public void EditTextField(TextField textField)
        {
            _ctx.TextFields.Update(textField);
            _ctx.SaveChanges();
        }

        public void AddTextField(TextField textField)
        {
            _ctx.TextFields.Add(textField);
            _ctx.SaveChanges();
        }

        public TextField GetTextField(int textFieldId)
        {
            return _ctx.TextFields.Include(t => t.Idea).Single(t => t.IdeaObjectId == textFieldId);
        }

        public void RemoveTextField(TextField textField)
        {
            _ctx.TextFields.Remove(textField);
            _ctx.SaveChanges();
        }

        #endregion

        #endregion

        #region Reaction methods

        // Returns enumerable of all reactions of platform based on ID of platform
        public IEnumerable<Reaction> GetAllReactions(int platformId)
        {
            return _ctx.Reactions
                .Where(r => r.Ideation.Phase.Project.Platform.PlatformId == platformId ||
                            r.Idea.Ideation.Phase.Project.Platform.PlatformId == platformId)
                .AsEnumerable();
        }

        // Returns enumerable of all reported reactions of project based on ID of project
        public IEnumerable<Reaction> GetReportedReactions(int projectId)
        {
            return _ctx.Reactions
                .Where(r => r.Idea.Ideation.Phase.Project.ProjectId == projectId ||
                            r.Ideation.Phase.Project.ProjectId == projectId)
                .Where(r => r.Reported == true)
                .Include(r => r.Idea)
                .Include(r => r.Ideation)
                .Include(r => r.IdentityUser)
                .AsEnumerable();
        }

        // Returns enumerable of all reactions of ideation based on given ideation
        public IEnumerable<Reaction> GetReactionsOnIdeation(Ideation ideation)
        {
            return _ctx.Reactions.Where(reaction => reaction.Ideation == ideation).AsEnumerable();
        }

        // Returns enumerable of all reactions of idea based on given idea
        public IEnumerable<Reaction> GetReactionsOnIdea(Idea idea)
        {
            return _ctx.Reactions.Where(reaction => reaction.Idea == idea).AsEnumerable();
        }
        
        // Returns enumerable of all reactions of idea based on ID of idea
        public IEnumerable<Reaction> GetIdeaReactions(int id)
        {
            return _ctx.Reactions.Where(reaction => reaction.Idea.IdeaId == id).AsEnumerable();
        }

        // Returns reaction based on ID
        public Reaction GetReaction(int reactionId)
        {
            return _ctx.Reactions
                .Include(r => r.Idea)
                .Include(r => r.Ideation)
                .Single(r => r.ReactionId == reactionId);
        }

        // Create reaction based on given reaction
        // Returns new reaction
        public Reaction CreateReaction(Reaction reaction)
        {
            _ctx.Reactions.Add(reaction);
            _ctx.SaveChanges();
            return reaction;
        }

        // Updates reaction based on given reaction
        public void UpdateReaction(Reaction reaction)
        {
            _ctx.Reactions.Update(reaction);
            _ctx.SaveChanges();
        }

        // Deletes given reaction from database
        public void RemoveReaction(Reaction reaction)
        {
            _ctx.Reactions.Remove(reaction);
            _ctx.SaveChanges();
        }

        #endregion

        #region Vote methods

        //Vote methods

        // Return enumerable of all votes
        public IEnumerable<Vote> GetVotes()
        {
            return _ctx.Votes.AsEnumerable();
        }

        // Return vote based on ID
        public Vote GetVote(int voteId)
        {
            return _ctx.Votes.Find(voteId);
        }

        // Create a new vote based on given vote
        public Vote CreateVote(Vote vote)
        {
            _ctx.Votes.Add(vote);
            _ctx.SaveChanges();
            return vote;
        }

        // Checks if user already voted on Idea
        // Return true if user did not vote already
        // Return false if user already voted
        public bool CheckUserVote(IdentityUser user, VoteType voteType, Idea idea)
        {
            if (_ctx.Votes.Where(v => v.Idea == idea).Where(v => v.IdentityUser == user)
                .Where(v => v.VoteType == voteType)
                .AsEnumerable().Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Deletes given vote from database
        public void RemoveVote(Vote vote)
        {
            _ctx.Votes.Remove(vote);
            _ctx.SaveChanges();
        }

        #endregion

        #region Like methods

        // Checks if given user has already liked the given reaction
        // Returns false if user has liked reaction
        // Return true if user has not liked reaction
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

        // Creates new like based on given like
        // Returns the new like
        public Like CreateLike(Like like)
        {
            _ctx.Likes.Add(like);
            _ctx.SaveChanges();
            return like;
        }

        // Returns like based on ID
        public Like GetLike(int likeId)
        {
            return _ctx.Likes.Find(likeId);
        }

        // Deletes given like from database
        public void RemoveLike(Like like)
        {
            _ctx.Likes.Remove(like);
            _ctx.SaveChanges();
        }

        #endregion


        #region Tag methods
        
        // Return tag based on ID
        public Tag GetTag(int tagId)
        {
            return _ctx.Tags.Find(tagId);
        }

        // Returns enumerable of all tags
        public IEnumerable<Tag> GetAllTags()
        {
            return _ctx.Tags.AsEnumerable();
        }
        
        // Returns ideatag based on ID
        public IdeaTag GetIdeaTag(int ideaTagId)
        {
            return _ctx.IdeaTags
                .Include(i => i.Idea)
                .Include(i => i.Tag)
                .Single(i => i.IdeaTagId == ideaTagId);
        }
        
        // Creates new ideatag from given ideatag
        public void CreateIdeaTag(IdeaTag ideaTag)
        {
            _ctx.IdeaTags.Add(ideaTag);
            _ctx.SaveChanges();
        }

        // Deletes the given ideatag from database
        public void DeleteIdeaTag(IdeaTag ideaTag)
        {
            _ctx.IdeaTags.Remove(ideaTag);
            _ctx.SaveChanges();
        }
        
        // Updates tag based on given tag
        public void EditTag(Tag tag)
        {
            _ctx.Tags.Update(tag);
            _ctx.SaveChanges();
        }

        // Creates a new tag based on given tag
        public void AddTag(Tag tag)
        {
            _ctx.Tags.Add(tag);
            _ctx.SaveChanges();
        }

        // Deletes the given tag from the database
        public void DeleteTag(Tag tag)
        {
            _ctx.Tags.Remove(tag);
            _ctx.SaveChanges();
        }
        #endregion


        
    }
}