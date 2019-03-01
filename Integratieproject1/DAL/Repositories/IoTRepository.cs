using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Models.IoT;

namespace Integratieproject1.DAL.Repositorys
{
    public class IoTRepository
    {
        private CityOfIdeasDbContext ctx = null;
        public IoTRepository( CityOfIdeasDbContext dbContext)
        {
            ctx = dbContext;
            //CityOfIdeasDbInitializer.Initialize(ctx, false);
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
    }
}