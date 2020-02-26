using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Circa.Actors.Domain.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.CQR;

namespace Circa.Actors.Domain.Operations
{
    public class CampaignOperations
    {
        private readonly ICallistoOperationFactory _factory;
        private const string Dm = "dm";
        private const string Player = "player";
        public CampaignOperations(ICallistoOperationFactory factory) { _factory = factory; }

        public ICallistoUpdate<Person> AddCampaignAsDungeonMaster(ObjectId id, AddCampaignDto dto) => AddCampaign(Dm, a => a.CampaignsAsDungeonMaster, id, dto);

        public ICallistoUpdate<Person> RemoveCampaignAsDm(ObjectId id, ObjectId campaignId) =>
            RemoveCampaign(Dm, a => a.CampaignsAsDungeonMaster, id, campaignId);


        public ICallistoUpdate<Person> AddCampaignAsPlayer(ObjectId id, AddCampaignDto dto) => AddCampaign(Player, a => a.CampaignsAsPlayer, id, dto);

        public ICallistoUpdate<Person> RemoveCampaignAsPlayer(ObjectId id, ObjectId campaignId) =>
            RemoveCampaign(Player, a => a.CampaignsAsPlayer, id, campaignId);


        private ICallistoUpdate<Person> RemoveCampaign(string opName, Expression<Func<Person, IEnumerable<Campaign>>> field, ObjectId id, ObjectId campaignId)
        {
            FilterDefinition<Campaign> arrayFilter = Builders<Campaign>.Filter.Eq(a => a.CampaignId, campaignId);
            return _factory.CreateUpdateById($"remove-campaign-as-{opName}",
                                             id,
                                             Builders<Person>.Update.PullFilter(field, arrayFilter));
        }

        private ICallistoUpdate<Person> AddCampaign(string opName, Expression<Func<Person, IEnumerable<Campaign>>> field, ObjectId id, AddCampaignDto dto)
        {
            Campaign campaign = PersonMappings.FromAddCampaignDto(dto);
            return _factory.CreateUpdateById($"add-campaign-as-{opName}",
                                             id, Builders<Person>.Update.Push(field, campaign));
        }
    }
}