using Microsoft.EntityFrameworkCore;

namespace Ticket_project.Models
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketContext _context;

        public TicketRepository(TicketContext context)
        {
            _context = context;
        }

        public List<Ticket> GetAll()
        {
            return _context.Tickets
                .Include(t => t.Status)
                .ToList();
        }

        public Ticket? GetById(int id)
        {
            return _context.Tickets
                .Include(t => t.Status)
                .FirstOrDefault(t => t.Id == id);
        }

        public void Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void Update(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }

        public List<Status> GetStatuses()
        {
            return _context.Statuses.ToList();
        }
        public void DeleteByStatus(string statusId)
        {
            var toDelete = _context.Tickets.Where(t => t.StatusId == statusId).ToList();
            _context.Tickets.RemoveRange(toDelete);
            _context.SaveChanges();
        }
    }
}