using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Infrastructure.Models
{
    public class EmployerUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string Email { get; set; }
        public string Href { get; set; }
    }
}
