using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Circa.Actors.Domain.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Circa.Actors.Domain.Operations
{
    public class CharacterOperations
    {
        private readonly ICallistoOperationFactory _factory;

        private const string Playable = "playable";
        private const string NonPlayable = "non-playable";

        public CharacterOperations(ICallistoOperationFactory factory) { _factory = factory; }

        public ICallistoUpdate<Person> AddPlayableCharacter(ObjectId id, AddCharacterDto dto) => AddChar(Playable, a => a.PlayableCharacters, id, dto);

        public ICallistoUpdate<Person> RemovePlayableCharacter(ObjectId id, ObjectId charId) => RemoveChar(Playable, a => a.PlayableCharacters, id, charId);

        public ICallistoUpdate<Person> AddNonPlayableCharacter(ObjectId id, AddCharacterDto dto) => AddChar(NonPlayable, a => a.NonPlayableCharacters, id, dto);

        public ICallistoUpdate<Person> RemoveNonPlayableCharacter(ObjectId id, ObjectId charId) =>
            RemoveChar(NonPlayable, a => a.NonPlayableCharacters, id, charId);

        private ICallistoUpdate<Person> AddChar(string opName, Expression<Func<Person, IEnumerable<Character>>> field, ObjectId id, AddCharacterDto dto)
        {
            Character character = PersonMappings.FromAddCharacterDto(dto);
            return _factory.CreateUpdateById($"add-{opName}-character",
                                             id,
                                             Builders<Person>.Update.Push(field, character));
        }

        private ICallistoUpdate<Person> RemoveChar(string opName, Expression<Func<Person, IEnumerable<Character>>> field, ObjectId id, ObjectId charId)
        {
            FilterDefinition<Character> arrayFilter = Builders<Character>.Filter.Eq(a => a.CharacterId, charId);
            return _factory.CreateUpdateById($"remove-{opName}-character", id,
                                             Builders<Person>.Update.PullFilter(field, arrayFilter));
        }
    }
}