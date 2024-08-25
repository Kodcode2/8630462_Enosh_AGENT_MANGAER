using Microsoft.AspNetCore.Mvc;

namespace AgentClient.Controllers
{
    public class MissionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
