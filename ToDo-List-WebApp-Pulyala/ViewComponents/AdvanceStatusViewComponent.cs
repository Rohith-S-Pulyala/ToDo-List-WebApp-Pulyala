using Microsoft.AspNetCore.Mvc;
using ToDo_List_WebApp_Pulyala.Models;

namespace ToDo_List_WebApp_Pulyala.ViewComponents
{
    public class AdvanceStatusViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Ticket ticket) {
            
            
            return View(ticket);
        }
    }
}
