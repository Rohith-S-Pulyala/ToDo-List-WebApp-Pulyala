using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDo_List_WebApp_Pulyala.Models;
using ToDo_List_WebApp_Pulyala.Enums;
using ToDo_List_WebApp_Pulyala.Services;
using System.Threading.Tasks;

/*
 * PROJECT: Agile Ticketing System (MVP)
 * CORE FEATURES:
 * - Automated Filtering via hyphenated URL segments
 * - Single-click status advancement (To Do -> In Progress -> QA -> Done).
 * - Separation of concerns between Dashboard (Home) and Backlog (Tickets).
 */

namespace ToDo_List_WebApp_Pulyala.Controllers
{
    public class HomeController : Controller
    {
        //private ToDoContext context;
        private readonly IToDoListService _toDoListService;
        private readonly ILogger<HomeController> _logger;

        //public HomeController (ToDoContext ctx) => context = ctx; 

        public HomeController(IToDoListService toDoListService, ILogger<HomeController> logger) // Dependency Injection to give access to the Database.
        {
            _toDoListService = toDoListService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string id)
        {
            // Initializes filter logic.
            var filters = new Filters(id);
            ViewBag.Filters = filters; // Pass filters to the view for dropdowns


            ViewBag.Sprints = await _toDoListService.GetUniqueSprintNumbersAsync();

            // Loads dynamic data for sidebar dropdown from DB and Enum
            ViewBag.Statuses = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList(); // Updated to enum for database readiness.

            // Asks the service for filtered list
            var tasks = await _toDoListService.GetFilteredTicketsAsync(filters);

            return View(tasks);
        }

        [HttpPost]
        public IActionResult Filter(string status, string sprint) 
        {
            // Lowercase values and uncertainty in "all"
            string statusPart = status?.ToLower() ?? "all";
            string sprintPart = sprint?.ToLower() ?? "all";

            // Builds ID string like "todo-1"
            string id = $"{statusPart}-{sprintPart}";
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost] // Transitions a ticket to the next logical Agile status
        public async Task <IActionResult> UpdateStatus(int id, string filter) 
        {
            await _toDoListService.ToggleTicketStatusAsync(id);

            //Redirect to index for existing filter string so the user stays on the same filtered view they looked at.
            return RedirectToAction("Index", new { ID = filter });
        }
    }
}
