using Tulip.Data;
using Tulip.Models;
using Tulip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tulip.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITasksServices _tasksServices;
        private readonly ApplicationDbContext _db;

        public ChatController(ILogger<HomeController> logger, ITasksServices tasksServices, ApplicationDbContext db)
        {
            _tasksServices = tasksServices;
            _db = db;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Compose()
        {
            return View();
        }

        private ApplicationUser getCurrentUser()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _db.ApplicationUsers.Find(userId);
        }

    }
}