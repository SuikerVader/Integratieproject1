using System;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Integratieproject1.UI.Controllers
{
    [Route("api/IoTApiController")]
    [ApiController]
    public class IoTApiController
    {
        private IdeationsManager ideationsManager;
        private IoTManager ioTManager;

        public IoTApiController()
        {
            this.ideationsManager = new IdeationsManager();
        }

        [HttpPost("Vote/{id}")]
        public async Task IoTVote(int id)
        {
           ideationsManager.CreateVote(ideaId:id,voteType:VoteType.IOT);
        }
        
        //wordt gebruikt voor IoT-opstellingen die meerdere knoppen bevatten (1-4) in mate van hoe eens ze het zijn
        [HttpPost("Vote/{id}/{supportLv}")]
        public async Task IoTVote(int id, int supportLv)
        {
            ioTManager.RegisterComplexVote(id, supportLv);
        }
    }
}