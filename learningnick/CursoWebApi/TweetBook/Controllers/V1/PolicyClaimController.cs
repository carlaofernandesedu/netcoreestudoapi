using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Extensions;

namespace TweetBook.Controllers.V1
{
    [Authorize]
    public class PolicyClaimController : Controller
    {
        
        [HttpGet(ApiRoutes.PolicyClaim.GetByPolicy)]
        [Authorize("PolicyClaimViewer")]
        public IActionResult GetbyPolicy()
        {
            return Ok(new { descricao = "retornando autenticado e  autorizado por claim" });
        }

        [HttpGet(ApiRoutes.PolicyClaim.Get)]
        public IActionResult Get()
        {
            var userid = HttpContext.GetUserId();
            return Ok(new { descricao = "retornando autenticado para o usuario:" + userid });
        }

    }
}