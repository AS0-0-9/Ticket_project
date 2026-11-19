using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Ticket_project.Models;

namespace TicketProject.Tests
{
    public class FakeTicketRepository : ITicketRepository
    {
        public List<Ticket> Tickets = new();
        public List<Status> Statuses = new()
    {
        new Status { StatusId = "todo", Name = "To Do" },
        new Status { StatusId = "inprogress", Name = "In Progress" },
        new Status { StatusId = "qa", Name = "QA" },
        new Status { StatusId = "done", Name = "Done" }
    };
        public bool AddCalled;
        public bool UpdateCalled;

        public List<Ticket> GetAll() => Tickets;
        public Ticket? GetById(int id) => Tickets.FirstOrDefault(t => t.Id == id);
        public void Add(Ticket ticket) { Tickets.Add(ticket); AddCalled = true; }
        public void Update(Ticket ticket) { UpdateCalled = true; }
        public List<Status> GetStatuses() => Statuses;
        public void DeleteByStatus(string statusId) =>
            Tickets.RemoveAll(t => t.StatusId == statusId);
    }
}
