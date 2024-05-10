using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        [Route("/secret")]
        [Authorize]
        public string Index()
        {
            var claims = User.Claims.ToList();
            return "secret message from ApiOne";
        }
    }
}
