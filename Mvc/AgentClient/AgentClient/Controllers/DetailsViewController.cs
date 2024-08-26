using AgentClient.Servise;
using Microsoft.AspNetCore.Mvc;

namespace AgentClient.Controllers
{
    // Controller for information about Missions
    public class DetailsViewController(IDetailsViewServis detailsViewServis) : Controller
    {
        // Returns a list of all Missions
        public async Task<IActionResult> Index()
        {
            // Sending the request to the Servis to receive a list of Missions
            var IsAllMissions = await detailsViewServis.GetAllMissionsFormServerAsync();
            if (IsAllMissions != null)
            {
                return View(IsAllMissions);
            }
            return RedirectToAction("Index", "Home");
        }


        // Returns details about a particular Missions
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
