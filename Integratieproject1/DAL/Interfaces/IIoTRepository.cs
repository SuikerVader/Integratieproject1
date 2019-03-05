using System.Collections.Generic;
using Integratieproject1.Domain.IoT;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IIoTRepository
    {
        IEnumerable<IoTSetup> GetIoTSetups();
        IoTSetup GetIoTSetup(int ioTSetupId);
        IoTSetup CreateIoTSetup(IoTSetup ioTSetup);
    }
}