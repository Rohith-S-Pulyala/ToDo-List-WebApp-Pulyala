using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDo_List_WebApp_Pulyala.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SprintNumber = table.Column<int>(type: "int", nullable: false),
                    PointValue = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Description", "Name", "PointValue", "SprintNumber", "Status" },
                values: new object[,]
                {
                    { 1, "Create the MVC app and folders", "Setup Project Structure", 3, 1, 3 },
                    { 2, "Build Create/Read/Update/Delete logic", "Implement CRUD", 5, 1, 1 },
                    { 3, "Fix header CSS", "Fix Header CSS", 1, 2, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
