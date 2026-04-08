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


            StatusId = filters.Length > 0 ? filters[0].ToLower() : "all";
            // Ensures that if the segment is empty, it defaults to "all"
            SprintId = filters.Length > 0 ? filters[1].ToLower() : "all";
        }

        // Read-Only properties to simplify conditional checks in the Controller/View
        public bool HasStatus => !string.Equals(StatusId, "all", StringComparison.OrdinalIgnoreCase);
        public bool HasSprint => !string.Equals(SprintId, "all", StringComparison.OrdinalIgnoreCase);
    }
}
