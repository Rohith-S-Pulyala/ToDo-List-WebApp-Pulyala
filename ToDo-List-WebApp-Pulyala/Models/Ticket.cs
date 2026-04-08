using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo_List_WebApp_Pulyala.Attributes;
using ToDo_List_WebApp_Pulyala.Enums;

namespace ToDo_List_WebApp_Pulyala.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(100, MinimumLength = 5)] // Makes sure field is not null, minimum 5 characteristics.
        public string Name { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Please provide a more detailed description.")] // Constraint annotation for 10 characters minimum
        public string Description { get; set; }

        [Required]
        [Range(0, 999, ErrorMessage = "Sprint Number must be Non-Negative.")]
        public int SprintNumber { get; set; }

        // Validation to make sure Administrator inputs all numbers.
        [Required]
        [FibonacciPoints(ErrorMessage = "Point value must be a valid Fibonacci number (1, 2, 3, 5, 8, 13, 21).")]
        public int PointValue { get; set; }

        // Validation not necessary, dropdown menu has a fixed set of options, none to add.
        public TicketStatus Status { get; set; } = TicketStatus.ToDo; // To Do, In Progress, QA, Done
    }
}
