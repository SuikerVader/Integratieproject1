using System.Collections.Generic;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IProjectsRepository
    {
        IEnumerable<Platform> GetPlatforms();
        Platform GetPlatform(int platformId);
        Platform CreatePlatform(Platform platform);
        IEnumerable<Project> GetProjects();
        Project GetProject(int projectId);
        Project CreateProject(Project project);
        IEnumerable<Phase> GetPhases();
        Phase GetPhase(int phaseId);
        Phase CreatePhase(Phase phase);
    }
}