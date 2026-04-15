using Microsoft.EntityFrameworkCore;
using ToDo_List_WebApp_Pulyala.Enums;
using ToDo_List_WebApp_Pulyala.Models;

namespace ToDo_List_WebApp_Pulyala.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly ToDoContext _context;
        public ToDoListService(ToDoContext context) {
            _context = context;
        }

        public async Task<List<Ticket>> GetActiveTicketsAsync(TicketStatus? status = null) {
            IQueryable<Ticket> query = _context.Tickets;

            if (status.HasValue) {
                query = query.Where(t => t.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id) {
            return await _context.Tickets.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateTicketAsync(Ticket ticket) {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null) 
            {
                _context.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TicketExistsAsync(int id) {
            return await _context.Tickets.AnyAsync(e => e.Id == id);
        }

        public async Task<List<Ticket>> GetFilteredTicketsAsync(Filters filters) {
            IQueryable<Ticket> query = _context.Tickets;

            if (filters.HasStatus && Enum.TryParse(filters.StatusId, true, out TicketStatus statusEnum)) {
                query = query.Where(t => t.Status == statusEnum);
            }

            if (filters.HasSprint && int.TryParse(filters.SprintId, out int sprintId)) {
                query = query.Where(t => t.SprintNumber == sprintId);
            }

            return await query.OrderBy(t => t.SprintNumber).ToListAsync();
        }

        public async Task<List<int>> GetUniqueSprintNumbersAsync() {
            return await _context.Tickets
                .Select(t => t.SprintNumber)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
        }

        public async Task ToggleTicketStatusAsync(int id) {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null) {
                ticket.Status = ticket.Status switch
                {
                    TicketStatus.ToDo => TicketStatus.InProgress,
                    TicketStatus.InProgress => TicketStatus.QA,
                    TicketStatus.QA => TicketStatus.Done,
                    _ => ticket.Status
                };

                await _context.SaveChangesAsync();
            }
        }
    }
}
