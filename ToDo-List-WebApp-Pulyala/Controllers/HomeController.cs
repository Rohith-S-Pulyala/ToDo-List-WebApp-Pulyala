using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDo_List_WebApp_Pulyala.Models;

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
        private ToDoContext context;

        public HomeController (ToDoContext ctx) => context = ctx;

        // Handles main Agile Board view with hyphenated filter strings (e.g., "todo-1")
        public IActionResult Index(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters; // Pass filters to the view for dropdowns

            // Loads dynamic data for sidebar dropdowns
            ViewBag.Sprints = context.Tickets.Select(t => t.SprintNumber).Distinct().OrderBy(s => s).ToList();
            ViewBag.Statuses = new List<string> { "To Do", "In Progress", "QA", "Done" };

            IQueryable<Ticket> query = context.Tickets;

            if (filters.HasStatus) {
                query = query.Where(t => t.Status.ToLower() == filters.StatusId.ToLower());
            }

            if (filters.HasSprint) {
                if (int.TryParse(filters.SprintId, out int sprintId)) {
                    query = query.Where(t => t.SprintNumber == sprintId);
                }
            }

            var tasks = query.OrderBy(t => t.SprintNumber).ToList();
            return View(tasks);
        }

        [HttpPost]
        public IActionResult Filter(string status, string sprint) 
        {
            // Builds ID string like "todo-1"
            string id = $"{status}-{sprint}";
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost] // Transitions a ticket to the next logical Agile status
        public IActionResult UpdateStatus(int id, string filter) 
        {
            var ticket = context.Tickets.Find(id);
            if (ticket != null) 
            {
                // Simple logic to update status
                ticket.Status = ticket.Status switch
                {
                    "To Do" => "In Progress",
                    "In Progress" => "QA",
                    "QA" => "Done",
                    _ => ticket.Status
                };
                context.SaveChanges();
            }

            //Redirect to index for existing filter string so the user stays on the same filtered view they looked at.
            return RedirectToAction("Index", new { ID = filter });
        }
    }
}
