using System.ComponentModel.DataAnnotations;

namespace ToDo_List_WebApp_Pulyala.Enums
{
    public enum TicketStatus
    {
        //[Display(Name = "To Do")]
        ToDo,

        //[Display(Name = "In Progress")]
        InProgress,

        //[Display(Name = "QA")]
        QA,

        //[Display(Name = "Done")]
        Done
    }
}
