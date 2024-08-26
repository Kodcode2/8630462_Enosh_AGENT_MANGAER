using AgentClient.Servise;
using Microsoft.AspNetCore.Mvc;

namespace AgentClient.Controllers
{
    public class DetailsViewController(IDetailsViewServis detailsViewServis) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var IsAllMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            if (IsAllMissions != null)
            {
                return View(IsAllMissions);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int id)
        {
            var IsAllMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            if (IsAllMissions != null)
            {
                return View(IsAllMissions.FirstOrDefault(x => x.Id == id));
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
