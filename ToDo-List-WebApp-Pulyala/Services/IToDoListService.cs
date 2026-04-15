using ToDo_List_WebApp_Pulyala.Enums;
using ToDo_List_WebApp_Pulyala.Models;

namespace ToDo_List_WebApp_Pulyala.Services
{
    public interface IToDoListService
    {
        Task<List<Ticket>> GetActiveTicketsAsync(TicketStatus? status = null);
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task CreateTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<bool> TicketExistsAsync(int id);
        Task<List<Ticket>> GetFilteredTicketsAsync(Filters filters);
        Task<List<int>> GetUniqueSprintNumbersAsync();
        Task ToggleTicketStatusAsync(int id);
    }
}
