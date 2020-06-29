using System;
using System.Collections.Generic;
using System.Text;
using DAS.DigitalEngagement.Models;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Domain.Mapping.Marketo
{
    public interface INewLeadMapper
    {
        NewLead Map(Person person);
    }

    public class NewLeadMapper : INewLeadMapper
    {
        public NewLead Map(Person person)
        {
            var lead = new NewLead();

            lead.Email = person.Email;
            lead.FirstName = person.FirstName;
            lead.LastName = person.LastName;

            return lead;
        }
    }
}
