using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAS.DigitalEngagement.Application.Repositories;
using DAS.DigitalEngagement.Domain.DataCollection;
using DAS.DigitalEngagement.Domain.Import;
using DAS.DigitalEngagement.Domain.Mapping;
using DAS.DigitalEngagement.Domain.Services;
using DAS.DigitalEngagement.Models.BulkImport;
using Microsoft.Extensions.Logging;

namespace DAS.DigitalEngagement.Application.Import.Handlers
{
    public class ImportEmployerUsersHandler : IImportEmployerUsersHandler
    {
        private readonly IBulkImportService _bulkImportService;
        private readonly IEmployerUsersRepository _employerUsersRepository;
        private readonly IPersonMapper _personMapper;
        private readonly ILogger<ImportEmployerUsersHandler> _logger;

        public ImportEmployerUsersHandler(IBulkImportService bulkImportService, ILogger<ImportEmployerUsersHandler> logger, IEmployerUsersRepository employerUsersRepository, IPersonMapper personMapper)
        {
            _bulkImportService = bulkImportService;
            _logger = logger;
            _employerUsersRepository = employerUsersRepository;
            _personMapper = personMapper;
        }

        public async Task<BulkImportStatus> Handle()
        {
            _logger.LogInformation($"about to handle person import");
            
            var empUsers = await _employerUsersRepository.GetAllUsers();

            var contacts = empUsers.Select(_personMapper.Map).ToList();
            
            return await _bulkImportService.ImportPeople(contacts);
        }


    }
}
