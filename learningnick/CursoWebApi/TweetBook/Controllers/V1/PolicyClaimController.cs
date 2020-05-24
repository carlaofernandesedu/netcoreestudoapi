using Microsoft.AspNetCore.Mvc;

namespace TweetBook.Controllers.V1
{
    public class PolicyClaimController : Controller
    {
        
        public IActionResult GetbyPolicy()
        {
            return Ok(new { descricao = "retornando por autorizacao de claim" });
        }

        public IActionResult Get()
        {
            return Ok(new { descricao = "retornando sem autorizacao" });
        }

    }
}