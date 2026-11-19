using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ticket_project.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Name cannot start or end with spaces.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Please enter a description.")]
        [RegularExpression(@"^\S[\s\S]*\S$|^\S$", ErrorMessage = "Description cannot start or end with spaces.")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Please enter a sprint number.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sprint number must be 1 or higher.")]
        public int SprintNumber { get; set; }

        [Required(ErrorMessage = "Please enter a point value.")]
        [Range(1, int.MaxValue, ErrorMessage = "Point value must be 1 or higher.")]
        public int PointValue { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        public string StatusId { get; set; } = "";

        [ValidateNever]
        public Status Status { get; set; } = null!;
    }
}