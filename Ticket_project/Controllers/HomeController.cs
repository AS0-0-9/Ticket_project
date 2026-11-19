using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket_project.Models;

namespace Ticket_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITicketRepository _tickets;

        public HomeController(ITicketRepository tickets)
        {
            _tickets = tickets;
        }

        private static readonly string[] Workflow = { "todo", "inprogress", "qa", "done" };

        public IActionResult Index(string? id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Statuses = _tickets.GetStatuses();

            var allTickets = _tickets.GetAll();

            ViewBag.Sprints = allTickets
                .Select(t => t.SprintNumber)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            IEnumerable<Ticket> query = allTickets;

            if (filters.HasSprint && int.TryParse(filters.Sprint, out int sprint))
                query = query.Where(t => t.SprintNumber == sprint);

            if (filters.HasStatus)
                query = query.Where(t => t.StatusId == filters.StatusId);

            var tickets = query.OrderBy(t => t.SprintNumber).ThenBy(t => t.Id).ToList();
            return View(tickets);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Statuses = _tickets.GetStatuses();
            return View(new Ticket { StatusId = "todo", SprintNumber = 1, PointValue = 1 });
        }

        [HttpPost]
        public IActionResult Add(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _tickets.Add(ticket);
                return RedirectToAction("Index");
            }
            ViewBag.Statuses = _tickets.GetStatuses();
            return View(ticket);
        }

        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join("-", filter);
            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        public IActionResult MoveNext([FromRoute] string? id, int ticketId)
        {
            var ticket = _tickets.GetById(ticketId);
            if (ticket != null)
            {
                int i = Array.IndexOf(Workflow, ticket.StatusId);
                if (i >= 0 && i < Workflow.Length - 1)
                {
                    ticket.StatusId = Workflow[i + 1];
                    _tickets.Update(ticket);
                }
            }
            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        public IActionResult DeleteDone([FromRoute] string? id)
        {
            _tickets.DeleteByStatus("done");
            return RedirectToAction("Index", new { id });
        }
    }
}