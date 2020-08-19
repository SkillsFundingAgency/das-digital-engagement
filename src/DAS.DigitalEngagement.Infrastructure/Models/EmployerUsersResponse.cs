using System;
using System.Collections.Generic;
using System.Text;

namespace DAS.DigitalEngagement.Infrastructure.Models
{
    public class EmployerUserResponse<T> where T : class
    {
        public List<T> Data { get; set; }
    }
}
