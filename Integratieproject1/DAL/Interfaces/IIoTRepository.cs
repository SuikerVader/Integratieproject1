using System.Collections.Generic;
using Integratieproject1.Domain.IoT;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IIoTRepository
    {
      
        #region Gets

        IEnumerable<IoTSetup> GetIoTSetups();
        IoTSetup GetIoTSetup(string ioTSetupId);
        IoTSetup GetIoTSetupByIdea(int id);
        IEnumerable<IoTSetup> GetAllIoTSetupsForPlatform(int platformId);
        IEnumerable<IoTSetup> GetAllIoTSetupsForProject(int id);
        IEnumerable<IoTSetup> GetAllIoTSetupsForIdeation(int id);
        IEnumerable<IoTSetup> GetAllIoTSetupsForIdea(int id);
        IEnumerable<IoTSetup> GetAllIoTSetupsForQuestion(int id);

        #endregion

        #region CUD

        IoTSetup CreateIoTSetup(IoTSetup ioTSetup);
        void RemoveIoTSetup(IoTSetup ioTSetup);
        void UpdateIoTSetup(IoTSetup original);

        #endregion

    }
}