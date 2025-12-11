using Microsoft.AspNetCore.Mvc;

namespace PokemonCenter.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "ok" });
}
