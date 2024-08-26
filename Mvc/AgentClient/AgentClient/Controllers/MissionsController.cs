using AgentClient.Dto;
using AgentClient.Servise;
using AgentClient.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgentClient.Controllers
{
    public class MissionsController(IMissionsServis missionsServis) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var IsAllMissions = await missionsServis.GetAllMissionsFormServerAsync();
            if (IsAllMissions != null)
            {
                return View(IsAllMissions);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int id)
        {
            var IsAllMissions = await missionsServis.GetAllMissionsFormServerAsync();
            if (IsAllMissions != null)
            {
                return View(IsAllMissions.FirstOrDefault(x => x.Id == id));
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
