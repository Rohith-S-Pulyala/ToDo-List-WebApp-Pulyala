using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDo_List_WebApp_Pulyala.Models;
using ToDo_List_WebApp_Pulyala.Enums;

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

        public HomeController (ToDoContext ctx) => context = ctx; // Dependency Injection to give access to the Database.

        public IActionResult Index(string id)
        {
            // Initializes filter logic.
            var filters = new Filters(id);
            ViewBag.Filters = filters; // Pass filters to the view for dropdowns

            // Loads dynamic data for sidebar dropdowns from DB and Enum
            ViewBag.Sprints = context.Tickets.Select(t => t.SprintNumber).Distinct().OrderBy(s => s).ToList(); 
            ViewBag.Statuses = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList(); // Updated to enum for database readiness.

            // Starts the query
            IQueryable<Ticket> query = context.Tickets;

            // Apply filters if they aren't "all"
            if (filters.HasStatus) {
                // Parses Status back to enum type.
                if (Enum.TryParse(filters.StatusId, true, out TicketStatus statusEnum)) {
                    query = query.Where(t => t.Status == statusEnum);
                }
            }

            if (filters.HasSprint && int.TryParse(filters.SprintId, out int sprintNum)) {
                if (int.TryParse(filters.SprintId, out int sprintId)) {
                    query = query.Where(t => t.SprintNumber == sprintId);
                }
            }

            // Final list to be sent to the view
            var tasks = query.OrderBy(t => t.SprintNumber).ToList();
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
        public IActionResult UpdateStatus(int id, string filter) 
        {
            var ticket = context.Tickets.Find(id);
            if (ticket != null) 
            {
                // Simple logic to update status
                ticket.Status = ticket.Status switch
                {
                    TicketStatus.ToDo => TicketStatus.InProgress,
                    TicketStatus.InProgress => TicketStatus.QA,
                    TicketStatus.QA => TicketStatus.Done,
                    _ => ticket.Status
                };
                context.SaveChanges();
            }

            //Redirect to index for existing filter string so the user stays on the same filtered view they looked at.
            return RedirectToAction("Index", new { ID = filter });
        }
    }
}
