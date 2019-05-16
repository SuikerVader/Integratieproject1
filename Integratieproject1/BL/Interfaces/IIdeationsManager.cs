using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using System.Collections;
using System.Collections.Generic;

namespace Integratieproject1.BL.Interfaces
{
    public interface IIdeationsManager
    {
        #region Ideation

        Ideation GetIdeation(int ideationId);
        IList<Ideation> GetProjectIdeations(int projectId);
        IList<Ideation> GetIdeations(int phaseId);
        IList<Ideation> GetIdeationsByPlatform(int phaseId);
        IList<Ideation> GetAllIdeations();
        void CreateIdeation(Ideation ideation, int phaseId);
        Ideation EditIdeation(Ideation ideation, int ideationId);
        void DeleteIdeation(int ideationId);

        #endregion

        #region Idea

        Idea GetIdea(int ideaId);
        IList<Idea> GetIdeas(int ideationId);
        IList<Idea> GetAllIdeas(int platformId); 
        IList<Idea> GetOtherIdeas(int ideationId);
        IList<Idea> GetReportedIdeas(int projectId);
        Idea CreateIdea(Idea idea);
        Idea CreateNewIdea(int ideationId, string userId);
        void EditIdea(Idea idea, int ideaId);
        void ChangeIdea(Idea idea);
        void DeleteIdea(int ideaId);
        Idea PostIdea(ArrayList parameters, int ideationId, string userId);
        void AddPosition(Position position, int ideaId);

        #endregion

        #region IdeaObject

        IdeaObject GetIdeaObject(int ideaObjectId);
        List<IdeaObject> GetIdeaObjects(int ideaId);
        void EditIdeaObject(IdeaObject ideaObject);
        void OrderNrChange(int ideaObjectId, string changer, int ideaId);

        #endregion

        #region Like

        Like GetLike(int likeId);
        void DeleteLike(int likeId);

        #endregion

        #region Reaction

        Reaction GetReaction(int reactionId);
        IList<Reaction> GetAllReactions(int platformId);
        IList<Reaction> GetReportedReactions(int projectId);
        void PostReaction(ArrayList parameters, int id, string userId, string element);
        void LikeReaction(int reactionId, string userId);
        void DeleteReaction(int reactionId);

        #endregion

        #region Vote

        Vote GetVote(int voteId);
        void CreateVote(int ideaId, VoteType voteType, string userId);
        void DeleteVote(int voteId);
        bool CheckVote(string userId, VoteType voteType, int ideaId);

        #endregion

        #region Tag

        Tag GetTag(int tagId);
        IdeaTag GetIdeaTag(int ideaTagId);
        List<Tag> GetTags(int ideaId);
        List<Tag> GetAllTags();
        void AddTag(Tag tag);
        void CreateIdeaTag(int ideaId, int tagId);
        void EditTag(Tag tag, int tagId);
        void DeleteTag(int tagId);
        void DeleteIdeaTag(int ideaTagId);

        #endregion

        #region Posts

        void DeletePost(int id, string type);
        void ReportPost(int id, string type);
        void PostCorrect(int id, string type);

        #endregion
    }
}