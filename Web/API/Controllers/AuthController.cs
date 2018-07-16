using Microsoft.AspNetCore.Mvc;
using FMASolutionsCore.BusinessServices.BusinessCore.Auth;
using Newtonsoft.Json;
using FMASolutionsCore.Web.API.Models;

namespace FMASolutionsCore.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("/[controller]/TryUserAuth")]
        public string TryUserAuth([FromBody] string data) =>
            JsonConvert.SerializeObject(new UserAuth(AuthRequest.DecodeFromFormattedBase64(data)).PerformAuth());

    }
}