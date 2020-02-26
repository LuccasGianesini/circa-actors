using System;
using Circa.Actors.Domain.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Circa.Actors.Domain.Operations
{
    public class PersonOperations
    {
        private readonly ICallistoOperationFactory _factory;

        public PersonOperations(ICallistoOperationFactory factory) { _factory = factory; }

        public ICallistoInsert<Person> Insert(InsertPersonDto dto) => _factory.CreateInsert("insert-person", PersonMappings.FromInsertDto(dto));

        public ICallistoUpdate<Person> Update(ObjectId id, UpdatePersonDto dto)
        {
            return _factory.CreateUpdateById("update-person",
                                             id,
                                             Builders<Person>.Update
                                                             .Set(a => a.Name, dto.Name)
                                                             .Set(a => a.Email, dto.Email)
                                                             .Set(a => a.Nickname, dto.Nickname)
                                                             .Set(a => a.LastUpdatedAt, DateTimeOffset.Now));
        }
    }
}