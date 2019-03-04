using System;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.DAL;
using Integratieproject1.DAL.Repositories;

namespace Integratieproject1.BL.Managers
{
    public class IdeationsManager
    {
        private IdeationsRepository ideationsRepository;
        private UnitOfWorkManager unitOfWorkManager;

        public IdeationsManager()
        {
            unitOfWorkManager = new UnitOfWorkManager();
            ideationsRepository = new IdeationsRepository(unitOfWorkManager.UnitOfWork);
        }
        public IdeationsManager( UnitOfWorkManager unitOfWorkManager)
                 {
                     if (unitOfWorkManager == null)
                         throw new ArgumentNullException("unitOfWorkManager");

                     this.unitOfWorkManager = unitOfWorkManager;
                     ideationsRepository = new IdeationsRepository(this.unitOfWorkManager.UnitOfWork);
                 }
        public Ideation GetIdeation(int ideationId)
        {
            return ideationsRepository.GetIdeation(ideationId);
        }

        public void CreateIdeation(Ideation ideation)
        {
            ideationsRepository.CreateIdeation(ideation);
            unitOfWorkManager.Save();
        }

        public void CreateIdea(Idea idea)
        {
            ideationsRepository.CreateIdea(idea);
            unitOfWorkManager.Save();
        }
    }
}