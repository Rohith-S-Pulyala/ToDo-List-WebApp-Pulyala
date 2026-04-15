using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.EntityFrameworkCore;
using ToDo_List_WebApp_Pulyala.Models;
using ToDo_List_WebApp_Pulyala.Enums;

namespace ToDo_List_WebApp_Pulyala.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext (DbContextOptions<ToDoContext> options) : base(options) { }

        // DATABASE SET: Ticket Data to be set.
        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Constraints for validation
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Seed data to be set on the table.
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket 
                { 
                    Id = 1, 
                    Name = "Setup Project Structure", 
                    Description = "Create the MVC app and folders", 
                    SprintNumber = 1, 
                    PointValue = 3,
                    Status = TicketStatus.Done
                },
                new Ticket
                {
                    Id = 2,
                    Name = "Implement CRUD",
                    Description = "Build Create/Read/Update/Delete logic",
                    SprintNumber = 1,
                    PointValue = 5,
                    Status = TicketStatus.InProgress
                },
                new Ticket 
                {
                    Id = 3,
                    Name = "Fix Header CSS",
                    Description = "Fix header CSS",
                    SprintNumber = 2,
                    PointValue = 1,
                    Status = TicketStatus.ToDo
                }
            );
        }
    }
}
