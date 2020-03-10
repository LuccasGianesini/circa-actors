using System.Threading.Tasks;
using Circa.Actors.Domain;
using Circa.Actors.Domain.Dtos;
using Circa.Actors.Domain.Operations;
using Circa.Actors.Infra;
using MongoDB.Bson;
using MongoDB.Driver;
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

        public async Task<CommonResponse<string>> Insert(InsertPersonDto dto)
        {
            _logger.Information("Inserting a new person", enrich => enrich.WithProperty("person.name", dto.Name));
            if (!Helper.IsValidEmail(dto.Email))
            {
                return new CommonResponse<string>().AddError(builder => builder
                                                                        .WithCode("002x2")
                                                                        .WithMessage("Invalid email address")
                                                                        .WithErrorType(CommonErrorType.ValidationError)
                                                                        .Build());
            }

            CommonResponse<Person> result = await _personRepository.AddPerson(_personOperations.Insert(dto));
            if (result.HasErrors)
            {
                //TODO Log error messages.
                _logger.Error("There was a error while inserting the document", enrich => enrich.WithProperty("error.message", "message"));
            }

            _logger.Information("Inserted new person on circa-actors database", enrich => enrich.WithProperty("document.id", result.Result.Id));
            return result.ToNewGenericType(result.HasErrors ? "Error creating a new person" : "Person created", true);
        }
    }
}