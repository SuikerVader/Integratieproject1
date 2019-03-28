using Integratieproject1.Domain.Projects;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.BL.Interfaces
{
    public interface IProjectsManager
    {
        Platform GetPlatform(int platformId);
        void CreatePlatform(Platform platform);
        Project GetProject(int projectId);
        void CreateProject(Project project, int userId);
        Phase GetPhase(int phaseId);
        Phase CreatePhase(Phase phase, int phaseNr, int projectId);
    }
}