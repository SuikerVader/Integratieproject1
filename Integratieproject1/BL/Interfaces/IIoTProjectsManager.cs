using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.BL.Interfaces
{
    public interface IIoTProjectsManager
    {
        void DeleteIoTSetup(IoTSetup ioTSetup);
        void CreateIoT(Position position, Idea idea, Question question);
        string GenerateIoTUrl();
        void RegisterComplexVote(int id, int supportLv);
    }
}