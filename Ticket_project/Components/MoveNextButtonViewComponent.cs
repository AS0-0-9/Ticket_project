using Microsoft.AspNetCore.Mvc;
using Ticket_project.Models;

namespace Ticket_project.ViewComponents
{
    public class MoveNextButtonViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Ticket ticket, string? filterString)
        {
            if (ticket.StatusId == "done")
                return Content(string.Empty);  

            ViewBag.FilterString = filterString;
            return View(ticket);
        }

    }
}
