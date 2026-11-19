using Ticket_project.Controllers;
using Ticket_project.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace TicketProject.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Add_ValidTicket_SavesAndRedirects()
        {
            var repo = new FakeTicketRepository();
            var controller = new HomeController(repo);

            var result = controller.Add(new Ticket { Name = "Test", Description = "Desc", StatusId = "todo" });

            Assert.True(repo.AddCalled);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public void Add_InvalidModel_DoesNotSaveAndReturnsView()
        {
            var repo = new FakeTicketRepository();
            var controller = new HomeController(repo);
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Add(new Ticket());

            Assert.False(repo.AddCalled);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("todo", "inprogress")]
        [InlineData("inprogress", "qa")]
        [InlineData("qa", "done")]
        public void MoveNext_AdvancesStatus(string start, string expected)
        {
            var repo = new FakeTicketRepository();
            repo.Tickets.Add(new Ticket { Id = 1, StatusId = start });
            var controller = new HomeController(repo);

            controller.MoveNext(null, 1);

            Assert.Equal(expected, repo.Tickets[0].StatusId);
            Assert.True(repo.UpdateCalled);
        }

        [Fact]
        public void MoveNext_AlreadyDone_StaysDone()
        {
            var repo = new FakeTicketRepository();
            repo.Tickets.Add(new Ticket { Id = 1, StatusId = "done" });
            var controller = new HomeController(repo);

            controller.MoveNext(null, 1);

            Assert.Equal("done", repo.Tickets[0].StatusId);
            Assert.False(repo.UpdateCalled);
        }

        [Fact]
        public void MoveNext_UnknownTicket_DoesNotThrow()
        {
            var controller = new HomeController(new FakeTicketRepository());

            var result = controller.MoveNext(null, 999);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void DeleteDone_RemovesOnlyDoneTickets()
        {
            var repo = new FakeTicketRepository();
            repo.Tickets.Add(new Ticket { Id = 1, StatusId = "done" });
            repo.Tickets.Add(new Ticket { Id = 2, StatusId = "todo" });
            var controller = new HomeController(repo);

            controller.DeleteDone(null);

            Assert.Single(repo.Tickets);
            Assert.Equal("todo", repo.Tickets[0].StatusId);
        }

        [Fact]
        public void Index_NoFilter_ReturnsAllTickets()
        {
            var repo = new FakeTicketRepository();
            repo.Tickets.Add(new Ticket { Id = 1, SprintNumber = 1, StatusId = "todo" });
            repo.Tickets.Add(new Ticket { Id = 2, SprintNumber = 2, StatusId = "done" });
            var controller = new HomeController(repo);

            var result = controller.Index(null) as ViewResult;
            var model = Assert.IsType<List<Ticket>>(result!.Model, exactMatch: false);

            Assert.Equal(2, model.Count);
        }
    }
}
