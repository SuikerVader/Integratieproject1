using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.IoT;

namespace Integratieproject1.DAL.Repositories
{
    public class IoTRepository : IIoTRepository
    {
        private readonly CityOfIdeasDbContext ctx = null;
        public IoTRepository( UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            ctx = unitOfWork.ctx;
        }
        public IEnumerable<IoTSetup> GetIoTSetups()
        {
            return ctx.IoTSetups.AsEnumerable();
        }
        public IoTSetup GetIoTSetup(int ioTSetupId)
        {
            return ctx.IoTSetups.Find(ioTSetupId);
        }
        public IoTSetup CreateIoTSetup(IoTSetup ioTSetup)
        {
            ctx.IoTSetups.Add(ioTSetup);
            ctx.SaveChanges();
            return ioTSetup;
        }

        public void RemoveIoTSetup(IoTSetup ioTSetup)
        {
            ctx.IoTSetups.Remove(ioTSetup);
            ctx.SaveChanges();
        }
        public IoTSetup GetIoTSetupByIdea(int id)
        {
            return ctx.IoTSetups.First(ioTSetup => ioTSetup.Idea.IdeaId == id);
        }
    }
}