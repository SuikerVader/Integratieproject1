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
        string GenerateIoTUrl();
        void RegisterComplexVote(int id, int supportLv);
    }
}