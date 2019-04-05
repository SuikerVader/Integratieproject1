using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public void IoTVote(int id)
        {
            ClaimsPrincipal currentUser = ClaimsPrincipal.Current;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
           ideationsManager.CreateVote(ideaId:id,voteType:VoteType.IOT, userId:currentUserID);
        }
        
        //wordt gebruikt voor IoT-opstellingen die meerdere knoppen bevatten (1-4) in mate van hoe eens ze het zijn
        [HttpPost("Vote/{id}/{supportLv}")]
        public async Task IoTVote(int id, int supportLv)
        {
            ioTManager.RegisterComplexVote(id, supportLv);
        }
    }
}