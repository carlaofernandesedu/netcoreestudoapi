using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _service;

        public IdentityController(IIdentityService service)
        {
            _service = service;
        }   

        [HttpPost(template: ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _service.RegisterAsync(request.Email,request.Password);
            
            if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse{Errors = authResponse.Errors});

            return Ok(new AuthSuccessResponse{ Token= authResponse.Token});
        }

        [HttpPost(template: ApiRoutes.Identity.Login)]
        
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _service.LoginAsync(request.Email,request.Password);
            
            if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse{Errors = authResponse.Errors});

            return Ok(new AuthSuccessResponse{ Token= authResponse.Token});
        }

    [HttpPost(template: ApiRoutes.Identity.Roles)]
        public async Task<IActionResult> Roles()
        {
            var roles = await _service.CreateRoles();
        
            return Ok(roles);
        }
    }
}