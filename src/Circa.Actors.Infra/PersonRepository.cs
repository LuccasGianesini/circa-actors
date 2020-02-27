using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Circa.Actors.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions.CQR;
using Solari.Titan;
using Solari.Vanth;
using Solari.Vanth.Builders;

namespace Circa.Actors.Infra
{
    public class PersonRepository : CallistoRepository<Person>, IPersonRepository
    {
        private readonly ICommonResponseFactory _commonResponseFactory;
        private readonly ITitanLogger<PersonRepository> _logger;

        public PersonRepository(ICallistoContext context, ICommonResponseFactory commonResponseFactory, ITitanLogger<PersonRepository> logger) : base(context)
        {
            _commonResponseFactory = commonResponseFactory;
            _logger = logger;
        }

        public async Task<CommonResponse<Person>> AddPerson(ICallistoInsert<Person> operation)
        {
            try
            {
                await Insert.One(operation);
                return _commonResponseFactory.CreateResult(operation.Value);
            }
            catch (MongoWriteException writeException)
            {
                _logger.Error(writeException.WriteError.Message, writeException);
                List<CommonDetailedErrorResponse> details = writeException.WriteError
                                                             .Details
                                                             .Elements
                                                             .Select(detailsElement => new CommonDetailedErrorResponseBuilder()
                                                                                       .WithTarget(detailsElement.Name)
                                                                                       .WithMessage(detailsElement.Value.AsString)
                                                                                       .Build())
                                                             .ToList();
                return _commonResponseFactory.CreateError<Person>(builder => builder
                                                                             .WithCode(writeException.WriteError.Code.ToString())
                                                                             .WithMessage(writeException.WriteError.Message)
                                                                             .WithDetail(details).Build());
            }
        }
    }
}