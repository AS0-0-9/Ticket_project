using Ticket_project.Models;

namespace Ticket_project.Models
{
    public interface ITicketRepository
    {
        List<Ticket> GetAll();
        Ticket? GetById(int id);
        void Add(Ticket ticket);
        void Update(Ticket ticket);
        List<Status> GetStatuses();
        void DeleteByStatus(string statusId);
    }
}
