using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.IoT;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class IoTRepository : IIoTRepository
    {
        private readonly CityOfIdeasDbContext _ctx;

        public IoTRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _ctx = unitOfWork.Ctx;
        }


        #region Gets
        public IEnumerable<IoTSetup> GetIoTSetups()
        {
            return _ctx.IoTSetups.AsEnumerable();
        }

        public IoTSetup GetIoTSetup(string ioTSetupId)
        {
            return _ctx.IoTSetups
                .Include(i => i.Question)
                .Include(i => i.Idea)
                .Include(i => i.Position)
                .Single(i => i.Code == ioTSetupId);
        }
        public IoTSetup GetIoTSetupByIdea(int id)
        {
            return _ctx.IoTSetups.First(ioTSetup => ioTSetup.Idea.IdeaId == id);
        }
        public IEnumerable<IoTSetup> GetAllIoTSetupsForPlatform(int platformId)
        {
            return _ctx.IoTSetups
                .Where(i => i.Idea.Ideation.Phase.Project.Platform.PlatformId == platformId ||
                            i.Question.Survey.Phase.Project.Platform.PlatformId == platformId)
                .Include(i => i.Position)
                .Include(i => i.Question).ThenInclude(q => q.Survey).ThenInclude(s => s.Phase).ThenInclude(p=>p.Project).ThenInclude(p=>p.Platform)
                .Include(i => i.Idea).ThenInclude(i => i.Ideation).ThenInclude(s => s.Phase).ThenInclude(p=>p.Project).ThenInclude(p=>p.Platform)
                .AsEnumerable();
        }

        public IEnumerable<IoTSetup> GetAllIoTSetupsForProject(int id)
        {
            return _ctx.IoTSetups
                .Where(i => i.Idea.Ideation.Phase.Project.ProjectId == id ||
                            i.Question.Survey.Phase.Project.ProjectId == id)
                .Include(i => i.Position)
                .Include(i => i.Question).ThenInclude(q => q.Survey)
                .Include(i => i.Idea)
                .AsEnumerable();
        }

        public IEnumerable<IoTSetup> GetAllIoTSetupsForIdeation(int id)
        {
            return _ctx.IoTSetups
                .Where(i => i.Idea.Ideation.IdeationId == id)
                .Include(i => i.Position)
                .Include(i => i.Question).ThenInclude(q => q.Survey)
                .Include(i => i.Idea)
                .AsEnumerable();
        }

        public IEnumerable<IoTSetup> GetAllIoTSetupsForIdea(int id)
        {
            return _ctx.IoTSetups
                            .Where(i => i.Idea.IdeaId == id)
                            .Include(i => i.Position)
                            .Include(i => i.Question).ThenInclude(q => q.Survey)
                            .Include(i => i.Idea)
                            .AsEnumerable();
        }
        public IEnumerable<IoTSetup> GetAllIoTSetupsForQuestion(int id)
        {
            return _ctx.IoTSetups
                .Where(i => i.Question.QuestionId == id)
                .Include(i => i.Position)
                .Include(i => i.Question).ThenInclude(q => q.Survey)
                .Include(i => i.Idea)
                .AsEnumerable();
        }
        

        #endregion

        #region CUD
        public IoTSetup CreateIoTSetup(IoTSetup ioTSetup)
        {
            _ctx.IoTSetups.Add(ioTSetup);
            _ctx.SaveChanges();
            return ioTSetup;
        }

        public void RemoveIoTSetup(IoTSetup ioTSetup)
        {
            _ctx.IoTSetups.Remove(ioTSetup);
            _ctx.SaveChanges();
        }

       

        public void UpdateIoTSetup(IoTSetup original)
        {
            _ctx.IoTSetups.Update(original);
            _ctx.SaveChanges();
        }

        #endregion

        
    }
}