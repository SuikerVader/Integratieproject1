using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Projects;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositorys;

namespace Integratieproject1.BL.Managers
{
    public class ProjectsManager
    {
        private ProjectsRepository projectsRepository;
        private CityOfIdeasDbContext ctx;

        public ProjectsManager()
        {
            ctx = Program.uow.ctx;
            this.projectsRepository = new ProjectsRepository(ctx);
        }

        public Project GetProject(int projectId)
        {
            return projectsRepository.GetProject(projectId);
        }


        public Platform GetPlatform(int platformId)
        {
            return projectsRepository.GetPlatform(platformId);
        }
    }
}