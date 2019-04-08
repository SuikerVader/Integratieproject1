using Integratieproject1.Domain.Projects;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.BL.Interfaces
{
    public interface IProjectsManager
    {
        Platform GetPlatform(int platformId);
        void CreatePlatform(Platform platform);
        Project GetProject(int projectId);
        void CreateProject(Project project, string id);
        IdentityUser GetUser(string id);
        Phase GetPhase(int phaseId);
        Phase CreatePhase(Phase phase, int phaseNr, int projectId);
    }
}