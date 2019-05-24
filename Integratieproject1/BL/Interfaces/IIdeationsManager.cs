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
        IList<Ideation> GetIdeationsByPlatform(int platformId);
        IList<Ideation> GetAllIdeations();
        IList<Ideation> GetAllIdeationsBySort(string sortOrder);
        void CreateIdeation(Ideation ideation, int phaseId);
        Ideation EditIdeation(Ideation ideation, int ideationId);
        void DeleteIdeation(int ideationId);

        #endregion

        #region Idea

        Idea GetIdea(int ideaId);
        IList<Idea> GetIdeas(int ideationId);
        IList<Idea> GetAllIdeas(int platformId); 
        IEnumerable<Idea> GetIdeasByUser(string currentUserId); 
        IEnumerable<Idea> GetAllNonPublishedIdeas(string sortOrder); 
        IList<Idea> GetOtherIdeas(int ideationId);
        IList<Idea> GetReportedIdeas(int projectId);
        Idea CreateNewIdea(int ideationId, string userId);
        void PublishIdea(int ideaId);
        void EditIdea(Idea idea, int ideaId);
        void ChangeIdea(Idea idea);
        void DeleteIdea(int ideaId);
        void AddPosition(Position position, int ideaId);
        void DeleteLocationFromIdea(int ideaId, int positionId);

        #endregion

        #region IdeaObject

        IdeaObject GetIdeaObject(int ideaObjectId);
        List<IdeaObject> GetIdeaObjects(int ideaId);
        void EditIdeaObject(IdeaObject ideaObject);
        void OrderNrChange(int ideaObjectId, string changer, int ideaId);

        #region TextFields

        void AddTextField(TextField textField, int ideaId);
        void EditTextField(TextField textField, int textFieldId);
        void DeleteTextField(int textFieldId);
        TextField GetTextField(int textFieldId);
        
        #endregion
        
        #region VideoAllowed

        void AddVideo(Video video, int ideaId);
        void DeleteVideo(int videoId);
        Video GetVideo(int videoId);

        #endregion
        
        #region Images

        void CreateImage(string name, string path, int ideaId);
        IEnumerable<Image> GetImages(int ideaId);
        Image GetImage(int imageId);
        void DeleteImage(int imageId);
        
        #endregion
        
        #endregion

        #region Like

        Like GetLike(int likeId);
        void DeleteLike(int likeId);

        #endregion

        #region Reaction

        Reaction GetReaction(int reactionId);
        IList<Reaction> GetAllReactions(int platformId);
        IList<Reaction> GetIdeaReactions(int id);
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
        List<Tag> GetAllTagsBySort(string sortOrder);
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