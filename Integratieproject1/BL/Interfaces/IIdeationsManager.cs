using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.BL.Interfaces
{
    public interface IIdeationsManager
    {
        Ideation GetIdeation(int ideationId);
        void CreateIdeation(Ideation ideation);
        void CreateIdea(Idea idea);
    }
}