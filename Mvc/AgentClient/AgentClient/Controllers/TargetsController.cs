using Microsoft.AspNetCore.Mvc;

namespace AgentClient.Controllers
{
    public class TargetsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
