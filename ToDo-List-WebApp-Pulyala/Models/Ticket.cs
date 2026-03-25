namespace ToDo_List_WebApp_Pulyala.Models
{
    public class Ticket
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
        public string Description { get; set; }
        public int SprintNumber { get; set; }
        public int PointValue { get; set; }
        public string Status { get; set; } // TO Do, In Progress, QA, Done
    }
}
