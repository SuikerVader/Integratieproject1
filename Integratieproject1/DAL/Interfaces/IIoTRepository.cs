using System.Collections.Generic;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IIoTRepository
    {
        IEnumerable<IoTSetup> GetIoTSetups();
        IoTSetup GetIoTSetup(int ioTSetupId);
        IoTSetup CreateIoTSetup(IoTSetup ioTSetup);
        void RemoveIoTSetup(IoTSetup ioTSetup);
        IoTSetup GetIoTSetupByIdea(int id);
        IoTSetup GetIoTSetupByQuestion(int id);
        bool IoTExistsFromIdea(Idea idea);
        bool IoTExistsFromQuestion(Question question);
    }
}