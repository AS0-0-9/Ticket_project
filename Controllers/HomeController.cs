using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticket_project.Models;

namespace Ticket_project.Controllers
{
    public class HomeController : Controller
    {
        private TicketContext context;
        public HomeController(TicketContext ctx) => context = ctx;

        private static readonly string[] Workflow = { "todo", "inprogress", "qa", "done" };

        public IActionResult Index(string? id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;
            ViewBag.Statuses = context.Statuses.ToList();
            ViewBag.Sprints = context.Tickets
                .Select(t => t.SprintNumber).Distinct().OrderBy(s => s).ToList();

            IQueryable<Ticket> query = context.Tickets.Include(t => t.Status);

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
            ViewBag.Statuses = context.Statuses.ToList();
            return View(new Ticket { StatusId = "todo", SprintNumber = 1, PointValue = 1 });
        }

        [HttpPost]
        public IActionResult Add(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                context.Tickets.Add(ticket);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Statuses = context.Statuses.ToList();
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
            var ticket = context.Tickets.Find(ticketId);
            if (ticket != null)
            {
                int i = Array.IndexOf(Workflow, ticket.StatusId);
                if (i >= 0 && i < Workflow.Length - 1)
                {
                    ticket.StatusId = Workflow[i + 1];
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index", new { id });
        }

        [HttpPost]
        public IActionResult DeleteDone([FromRoute] string? id)
        {
            var toDelete = context.Tickets.Where(t => t.StatusId == "done").ToList();
            context.Tickets.RemoveRange(toDelete);
            context.SaveChanges();
            return RedirectToAction("Index", new { id });
        }
    }
}