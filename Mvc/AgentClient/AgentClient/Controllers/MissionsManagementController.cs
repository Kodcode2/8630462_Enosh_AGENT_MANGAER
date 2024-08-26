﻿using AgentClient.Dto;
using AgentClient.Servise;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AgentClient.Controllers
{
    public class MissionsManagementController(IMissionsServis missionsServis) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var ListMissions = await missionsServis.CreateListMissionsManagementVM();
            if (ListMissions != null)
            {
                return View(ListMissions);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> StartTheMission(int id)
        {
            var isChange = await missionsServis.StatusChangeById(id, "mitzvah");
            if (isChange)
            {
                return RedirectToAction("Index", "Missions");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
