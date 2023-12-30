using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestRapidPay.Models;
using TestRapidPay.Services;

namespace TestRapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthenticationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User users)
        {
            var result_authentication = await _authorizationService.ReturnToken(users);
            if (result_authentication == null)
                return Unauthorized();

            return Ok(result_authentication);

        }
    }
}
