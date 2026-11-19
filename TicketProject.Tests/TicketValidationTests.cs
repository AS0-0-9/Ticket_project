using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Ticket_project.Models;

namespace TicketProject.Tests
{
    public class TicketValidationTests
    {
        private static List<ValidationResult> Validate(Ticket t)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(t, new ValidationContext(t), results, true);
            return results;
        }

        [Fact]
        public void Ticket_EmptyName_IsInvalid()
        {
            var t = new Ticket { Name = "", Description = "d", SprintNumber = 1, PointValue = 1, StatusId = "todo" };
            Assert.Contains(Validate(t), r => r.MemberNames.Contains("Name"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Ticket_BadPointValue_IsInvalid(int points)
        {
            var t = new Ticket { Name = "n", Description = "d", SprintNumber = 1, PointValue = points, StatusId = "todo" };
            Assert.Contains(Validate(t), r => r.MemberNames.Contains("PointValue"));
        }

        [Fact]
        public void Ticket_NameWithLeadingSpace_IsInvalid()
        {
            var t = new Ticket { Name = " Fix bug", Description = "d", SprintNumber = 1, PointValue = 1, StatusId = "todo" };
            Assert.Contains(Validate(t), r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void Ticket_AllFieldsValid_HasNoErrors()
        {
            var t = new Ticket { Name = "Fix bug", Description = "d", SprintNumber = 1, PointValue = 3, StatusId = "todo" };
            Assert.Empty(Validate(t));
        }
    }
}
