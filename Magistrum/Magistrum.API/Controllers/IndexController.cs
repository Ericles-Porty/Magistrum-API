using Microsoft.AspNetCore.Mvc;

namespace Magistrum.API.Controllers;
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("")]
public class IndexController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Redirect("/swagger");
    }
}