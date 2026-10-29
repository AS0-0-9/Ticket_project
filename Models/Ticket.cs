using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ticket_project.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = "";

        [Range(1, 100, ErrorMessage = "Sprint number must be 1 or higher.")]
        public int SprintNumber { get; set; }

        [Range(1, 100, ErrorMessage = "Point value must be 1 or higher.")]
        public int PointValue { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        public string StatusId { get; set; } = "";

        [ValidateNever]
        public Status Status { get; set; } = null!;
    }
}