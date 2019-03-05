using Integratieproject1.Domain.Projects;

namespace Integratieproject1.BL.Interfaces
{
    public interface IProjectsManager
    {
        Platform GetPlatform(int platformId);
        void CreatePlatform(Platform platform);
        Project GetProject(int projectId);
        void CreateProject(Project project);
        Phase GetPhase(int phaseId);
        void CreatePhase(Phase phase);
    }
}