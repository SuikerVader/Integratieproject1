using System.Collections.Generic;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IIdeationsRepository
    {
        #region Ideation methods

        IEnumerable<Ideation> GetIdeations(int phaseId);

        IEnumerable<Ideation> GetProjectsIdeations(int projectId);
        IEnumerable<Ideation> GetAllIdeations();
        IEnumerable<Ideation> GetIdeationsByPlatform(int platformId);
        Ideation GetIdeation(int ideationId);
        Ideation EditIdeation(Ideation ideation);
        void RemoveIdeation(Ideation ideation);
        
        #endregion

        #region Idea methods

        IEnumerable<Idea> GetIdeas(int ideationId);
        IEnumerable<Idea> GetAllIdeas(int platformId);
        IEnumerable<Idea> GetIdeasByUser(string currentUserId);
        IEnumerable<Idea> GetAllNonPublishedIdeas();
        Idea GetIdea(int ideaId);
        IEnumerable<Idea> GetReportedIdeas(int projectId);
        Idea CreateIdea(Idea idea);
        void UpdateIdea(Idea idea);
        void PublishIdea(Idea idea);
        void RemoveIdea(Idea idea);

        #endregion

        #region IdeaObject methods

        IEnumerable<IdeaObject> GetIdeaObjects(int ideaId);
        IdeaObject GetIdeaObject(int ideaObjectId);
        
        #region Images

        Image CreateImage(Image image);
        IEnumerable<Image> ReadImagesOfIdea(int ideaId);
        Image GetImage(int imageId);
        void RemoveImage(Image image);
        void EditImage(Image image);
        
        #endregion

        #region VideoAllowed

        Video GetVideo(int videoId);
        void AddVideo(Video video);
        void EditVideo(Video video);
        void RemoveVideo(Video video);
       
        #endregion

        #region TextField

        void EditTextField(TextField textField);
        void AddTextField(TextField textField);
        TextField GetTextField(int textFieldId);
        void RemoveTextField(TextField textField);

        #endregion

        #endregion

        #region Reaction methods

        IEnumerable<Reaction> GetAllReactions(int platformId);
        IEnumerable<Reaction> GetReportedReactions(int projectId);
        IEnumerable<Reaction> GetReactionsOnIdeation(Ideation ideation);
        IEnumerable<Reaction> GetReactionsOnIdea(Idea idea);
        IEnumerable<Reaction> GetIdeaReactions(int id);
        Reaction GetReaction(int reactionId);
        Reaction CreateReaction(Reaction reaction);
        void UpdateReaction(Reaction reaction);
        void RemoveReaction(Reaction reaction);

        #endregion

        #region Vote methods

        IEnumerable<Vote> GetVotes();
        Vote GetVote(int voteId);
        Vote CreateVote(Vote vote);
        bool CheckUserVote(IdentityUser user, VoteType voteType, Idea idea);
        void RemoveVote(Vote vote);
        
        #endregion

        #region Like methods

        bool CheckLike(Reaction reaction, IdentityUser loggedInUser);
        Like CreateLike(Like like);
        Like GetLike(int likeId);
        void RemoveLike(Like like);
        
        #endregion

        #region Tag methods

        Tag GetTag(int tagId);
        IEnumerable<Tag> GetAllTags();
        IdeaTag GetIdeaTag(int ideaTagId);
        void CreateIdeaTag(IdeaTag ideaTag);
        void DeleteIdeaTag(IdeaTag ideaTag);
        void EditTag(Tag tag);
        void AddTag(Tag tag);
        void DeleteTag(Tag tag);
        

        #endregion

    }
}