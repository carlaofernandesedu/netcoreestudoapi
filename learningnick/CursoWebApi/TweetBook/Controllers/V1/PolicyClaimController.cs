using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;

namespace TweetBook.Controllers.V1
{
    [Authorize]
    public class PolicyClaimController : Controller
    {
        
        [HttpGet(ApiRoutes.PolicyClaim.GetByPolicy)]
        [Authorize("PolicyClaimViewer")]
        public IActionResult GetbyPolicy()
        {
            return Ok(new { descricao = "retornando por autorizacao de claim" });
        }

        [HttpGet(ApiRoutes.PolicyClaim.Get)]
        public IActionResult Get()
        {
            return Ok(new { descricao = "retornando sem autorizacao" });
        }

    }
}