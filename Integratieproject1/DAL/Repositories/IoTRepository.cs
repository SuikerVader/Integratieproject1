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
        public IoTSetup GetIoTSetup(int ioTSetupId)
        {
            return _ctx.IoTSetups.Find(ioTSetupId);
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
    }
}