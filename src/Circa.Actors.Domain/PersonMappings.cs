using System;
using Circa.Actors.Domain.Dtos;

namespace Circa.Actors.Domain
{
    public static class PersonMappings
    {
        public static Person FromInsertDto(InsertPersonDto dto) =>
            new Person(dto.Name, dto.Email, dto.Nickname)
            {
                CreatedAt = DateTimeOffset.Now,
                LastUpdatedAt = DateTimeOffset.Now,
                Nickname = dto.Nickname
            };

        public static Campaign FromAddCampaignDto(AddCampaignDto dto) => new Campaign(dto.CampaignId, dto.Name);
        
        public static Character FromAddCharacterDto(AddCharacterDto dto) => new Character(dto.CharacterId, dto.Name, dto.Level);
    }
}