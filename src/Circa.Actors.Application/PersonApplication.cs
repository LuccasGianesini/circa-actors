using System.Threading.Tasks;
using Circa.Actors.Domain;
using Circa.Actors.Domain.Dtos;
using Circa.Actors.Domain.Operations;
using Circa.Actors.Infra;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;
using Solari.Titan;
using Solari.Vanth;

namespace Circa.Actors.Application
{
    public class PersonApplication : IPersonApplication
    {
        private readonly IPersonRepository _personRepository;
        private readonly PersonOperations _personOperations;
        private readonly ITitanLogger<PersonApplication> _logger;

        public PersonApplication(IPersonRepository personRepository, PersonOperations personOperations, ITitanLogger<PersonApplication> logger)
        {
            _personRepository = personRepository;
            _personOperations = personOperations;
            _logger = logger;
        }

        public async Task<CommonResponse<ObjectId>> Insert(InsertPersonDto dto)
        {
            _logger.Information("Inserting a new person");
            CommonResponse<Person> result = await _personRepository.AddPerson(_personOperations.Insert(dto));
            return result.ToNewGenericType(result.HasErrors ? CallistoConstants.ObjectIdDefaultValue : result.Result.Id, true);
        }
    }
}