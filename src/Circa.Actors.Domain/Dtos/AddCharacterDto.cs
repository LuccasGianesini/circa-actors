using MongoDB.Bson;

namespace Circa.Actors.Domain.Dtos
{
    public class AddCharacterDto
    {
        public ObjectId CharacterId { get; set; }
        public string Name { get; set; }
        public short Level { get; set; }
        
    }
}