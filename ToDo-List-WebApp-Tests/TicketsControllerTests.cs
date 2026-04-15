using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo_List_WebApp_Pulyala.Controllers;
using ToDo_List_WebApp_Pulyala.Enums;
using ToDo_List_WebApp_Pulyala.Models;
using ToDo_List_WebApp_Pulyala.Services;
using Xunit;

namespace ToDo_List_WebApp_Tests
{
    public class TicketsControllerTests
    {
        private readonly Mock<IToDoListService> _mockService;
        private readonly TicketsController _controller;

        public TicketsControllerTests() {
            _mockService = new Mock<IToDoListService>(); // Mock service setup

            _controller = new TicketsController(_mockService.Object); // Injects Mock into the controller
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithATicket()
        {
            // ARRANGE
            // Creates a fake ticket to return
            var fakeTicket = new Ticket { Id = 1, Name = "Test Ticket" };

            // Tells the Mock: "When someone calls GetTicketByIdAsync(1), return the fake ticket"
            _mockService.Setup(service => service.GetTicketByIdAsync(1))
                        .ReturnsAsync(fakeTicket);

            // ACT
            var result = await _controller.Details(1);

            // ASSERT
            // Verifies the result is a ViewResult (not a 404 or a Redirect)
            var viewResult = Assert.IsType<ViewResult>(result);

            // Verifies the model inside the view is our fake ticket
            var model = Assert.IsAssignableFrom<Ticket>(viewResult.ViewData.Model);
            model.Id.Should().Be(1);
            model.Name.Should().Be("Test Ticket");
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // ARRANGE
            // Tells the mock to return null for ID 99
            _mockService.Setup(service => service.GetTicketByIdAsync(99))
                        .ReturnsAsync((Ticket)null!);

            // ACT
            var result = await _controller.Details(99);

            // ASSERT
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
