using System;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;

namespace Circa.Actors.Domain
{
    public class Campaign : IDocumentNode
    {
        public ObjectId CampaignId { get; set; }
        public string Name { get; set; }

        public Campaign(ObjectId campaignId, string name)
        {
            CampaignId = campaignId;
            Name = name;
        }
    }
}