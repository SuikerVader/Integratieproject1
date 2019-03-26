using System.Collections.Generic;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IProjectsRepository
    {
        IEnumerable<Platform> GetPlatforms();
        Platform GetPlatform(int platformId);
        Platform CreatePlatform(Platform platform);
        IEnumerable<Project> GetProjects(int platformId);
        Project GetProject(int projectId);
        Project CreateProject(Project project);
        IEnumerable<Phase> GetPhases(int projectId);
        Phase GetPhase(int phaseId);
        Phase CreatePhase(Phase phase);
    }
}