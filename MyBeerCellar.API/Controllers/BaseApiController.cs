using Microsoft.AspNetCore.Mvc;

namespace MyBeerCellar.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {

    }
}