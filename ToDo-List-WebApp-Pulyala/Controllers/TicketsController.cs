using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo_List_WebApp_Pulyala.Models;
using ToDo_List_WebApp_Pulyala.Enums;
using ToDo_List_WebApp_Pulyala.Services;

namespace ToDo_List_WebApp_Pulyala.Controllers
{
    public class TicketsController : Controller
    {
        //private readonly ToDoContext _context;

        private readonly IToDoListService _toDoListService;

        //public TicketsController(ToDoContext context)
        //{
        //    _context = context;
        //}

        public TicketsController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string statusFilter)
        {
            TicketStatus? parsedStatus = null;

            //Parses the string into the Enum
            if (Enum.TryParse(statusFilter, out TicketStatus result)) 
            {
               parsedStatus = result;
            }

            var tickets = await _toDoListService.GetActiveTicketsAsync(parsedStatus);

            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _toDoListService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewBag.Statuses = new List<string> { "To Do", "In Progress", "QA", "Done" }; // List of statuses
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SprintNumber,PointValue,Status")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _toDoListService.CreateTicketAsync(ticket);
                return RedirectToAction("Index", "Home"); // REDIRECT: Redirects to Agile Board instead of Backlog List
            }


            ViewBag.Statuses = new List<string> { "To Do", "In Progress", "QA", "Done" };
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _toDoListService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Statuses = new List<string> { "To Do", "In Progress", "QA", "Done" };

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SprintNumber,PointValue,Status")] Ticket ticket)
        {
            // Validation: Ensures ticket ID is the same.

            if (id != ticket.Id)
            {
                return NotFound();
            }

            // Validation: Ensures ticket data meets model requirements before saving
            if (ModelState.IsValid)
            {
                try
                {
                    await _toDoListService.UpdateTicketAsync(ticket);
                }
                // Handles concurrency issues (e.g. ticket deleted by another user while editing)
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw if database error is different.
                    }
                }
                return RedirectToAction("Index", "Home"); // REDIRECT: Redirects to Agile Board instead of Backlog List
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _toDoListService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _toDoListService.DeleteTicketAsync(id);
            return RedirectToAction("Index", "Home"); // REDIRECT: Redirects to Agile Board instead of Backlog List
        }

        private async Task<bool> TicketExists(int id)
        {
            return await _toDoListService.TicketExistsAsync(id);
        }
    }
}