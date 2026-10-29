namespace Ticket_project.Models
{
    public class Filters
    {
        public Filters(string? filterstring)
        {
            FilterString = filterstring ?? "all-all";
            string[] parts = FilterString.Split('-');
            Sprint = parts[0];
            StatusId = parts.Length > 1 ? parts[1] : "all";
        }

        public string FilterString { get; }
        public string Sprint { get; }
        public string StatusId { get; }

        public bool HasSprint => Sprint.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";
    }
}