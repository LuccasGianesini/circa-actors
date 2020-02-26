using MongoDB.Bson;

namespace Circa.Actors.Domain.Dtos
{
    public class AddCampaignDto
    {
        public ObjectId CampaignId { get; set; }
        public string Name { get; set; }
        
    }
}