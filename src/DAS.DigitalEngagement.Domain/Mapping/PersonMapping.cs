using DAS.DigitalEngagement.Infrastructure.Models;
using DAS.DigitalEngagement.Models;

namespace DAS.DigitalEngagement.Domain.Mapping
{
    public interface IPersonMapper
    {
        Person Map(EmployerUser person);
    }

    public class PersonMapper : IPersonMapper
    {
        public Person Map(EmployerUser user)
        {
            var lead = new Person();

            lead.Email = user.Email;
            lead.FirstName = user.FirstName;
            lead.LastName = user.LastName;
            lead.EmployerUserId = user.Id;

            return lead;
        }
    }
}
