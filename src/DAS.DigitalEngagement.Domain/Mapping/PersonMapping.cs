using System;
using System.Collections.Generic;
using System.Text;
using DAS.DigitalEngagement.Models;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.Mapping.Marketo
{
    public interface IPersonMapper
    {
        Person Map(Person person);
    }

    public class PersonMapper : IPersonMapper
    {
        public Person Map(Person person)
        {
            var lead = new Person();

            lead.Email = person.Email;
            lead.FirstName = person.FirstName;
            lead.LastName = person.LastName;

            return lead;
        }
    }
}
