using Microsoft.AspNetCore.Mvc;

namespace AgentClient.Controllers
{
    public class AgentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
