using System.Collections.Generic;
using DAS.DigitalEngagement.Models.DataCollection;
using Das.Marketo.RestApiClient.Configuration;
using Das.Marketo.RestApiClient.Models;

namespace DAS.DigitalEngagement.Application.DataCollection.Mapping
{
    public interface IUserDataMapping
    {
        PushLeadToMarketoRequest MapFromUserData(UserData user,
            RegisterInterestProgramConfiguration programConfiguration);
    }

    public class UserDataMapping : IUserDataMapping
    {
        public PushLeadToMarketoRequest MapFromUserData(UserData user,
            RegisterInterestProgramConfiguration programConfiguration)
        {

            var newLeadRequest = new PushLeadToMarketoRequest();

            newLeadRequest.ProgramName = programConfiguration.ProgramName;
            newLeadRequest.Source = programConfiguration.Source;
            newLeadRequest.Reason = user.RouteId == "1" ? programConfiguration.CitizenReason : programConfiguration.EmployerReason;
            newLeadRequest.LookupField = programConfiguration.LookupField;

            newLeadRequest.Input = new List<NewLead>();

            var newLead = new NewLead()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IncludeInUR = user.IncludeInUR,
                UkEmployerSize = user.UkEmployerSize,
                MktoCompanyNotes=user.MktoCompanyNotes,
                PrimaryIndustry=user.PrimaryIndustry,
                PrimaryLocation=user.PrimaryLocation,
                PersonOrigin = user.PersonOrigin,
                AppsgovSignUpDate = user.AppsgovSignUpDate,
            };

            newLeadRequest.Input.Add(newLead);

            return newLeadRequest;
        }
    }
}