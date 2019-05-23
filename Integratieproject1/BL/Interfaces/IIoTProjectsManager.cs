using System.Collections.Generic;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.BL.Interfaces
{
    public interface IIoTProjectsManager
    {
        void DeleteIoTSetup(string ioTSetup);
        void CreateIoT(Position position, Idea idea, Question question);
        void CreateIoTSetup(IoTSetup ioTSetup, int id, string type);
        string GenerateIoTUrl(string url);
        void RegisterComplexVote(int id, int supportLv, int amount);
        List<IoTSetup> GetAllIoTSetups();
        IoTSetup GetIoT(string iotId);
        List<IoTSetup> GetAllIoTSetupsForPlatform(int platformId);
        List<IoTSetup> GetAllIoTSetupsForProject(int id);
        List<IoTSetup> GetAllIoTSetupsForIdeation(int id);
        List<IoTSetup> GetAllIoTSetupsForIdea(int id);
        List<IoTSetup> GetAllIoTSetupsForQuestion(int id);
        void EditIoTSetup(IoTSetup ioTSetup, string iotId);
    }
}