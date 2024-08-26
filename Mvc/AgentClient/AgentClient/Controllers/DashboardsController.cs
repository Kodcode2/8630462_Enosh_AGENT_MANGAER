using AgentClient.Servise;
using Microsoft.AspNetCore.Mvc;

namespace AgentClient.Controllers
{
    public class DashboardsController(IDashboardsServis dashboardsServis) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> General()
        {
            var vM = await dashboardsServis.SetGeneralInformationVM();
            if (vM != null)
            {
                return View(vM);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> AgentStatus()
        {
            var vM = await dashboardsServis.AgentDetails();
            if (vM != null)
            {
                return View(vM);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> TargetStatus()
        {
            var vM = await dashboardsServis.TargetDetails();
            if (vM != null)
            {
                return View(vM);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Mtrix()
        {
            
                return View();
            
        }
    }
}
