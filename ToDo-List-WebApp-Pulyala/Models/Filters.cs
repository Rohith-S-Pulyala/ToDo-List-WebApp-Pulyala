namespace ToDo_List_WebApp_Pulyala.Models
{
    public class Filters
    {
        public string FilterString { get; }
        public string StatusId { get; set; }
        public string SprintId { get; set; }

        // Constructor: Parses a single URL string into separate Status and Sprint properties.
        // Defaults to "all-all" if the string is null or empty.
        public Filters(string filterstring)
        {
            FilterString = filterstring ?? "all-all";
            string[] filters = FilterString.Split('-') ;
            StatusId = filters[0];
            SprintId = filters.Length > 1 ? filters[1]: "all";
        }

        // Read-Only properties to simplify conditional checks in the Controller/View
        public bool HasStatus => StatusId.ToLower() != "all";
        public bool HasSprint => SprintId.ToLower() != "all";
    }
}
