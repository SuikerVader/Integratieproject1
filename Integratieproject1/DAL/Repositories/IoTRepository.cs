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
        public IoTRepository( UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _ctx = unitOfWork.Ctx;
        }
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
        public IoTSetup GetIoTSetupByIdea(int id)
        {
            return _ctx.IoTSetups.First(ioTSetup => ioTSetup.Idea.IdeaId == id);
        }

        public void UpdateIoTSetup(IoTSetup original)
        {
            _ctx.IoTSetups.Update(original);
            _ctx.SaveChanges();
        }
    }
}