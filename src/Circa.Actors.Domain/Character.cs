using System;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;

namespace Circa.Actors.Domain
{
    public class Character : IDocumentNode
    {
        public Character(ObjectId characterId, string name, short level)
        {
            CharacterId = characterId;
            Name = name;
            Level = level;
        }

        public ObjectId CharacterId { get; set; }
        public string Name { get; set; }
        public short Level { get; set; }
    }
}