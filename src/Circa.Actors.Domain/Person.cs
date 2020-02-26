using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Solari.Callisto.Abstractions;

namespace Circa.Actors.Domain
{
    public class Person : IDocumentRoot
    {
        public Person(string name, string email, string nickname)
        {
            Name = name;
            Email = email;
            Nickname = nickname;
        }

        /// <summary>
        /// The name of the person.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The email of the person.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The nickname of the person.
        /// </summary>
        public string Nickname { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public DateTimeOffset LastLogin { get; set; }
        public ObjectId Id { get; set; }
        
        /// <summary>
        /// List of campaigns the person is a Dm
        /// </summary>
        public List<Campaign> CampaignsAsDungeonMaster { get; set; } = new List<Campaign>();
        
        /// <summary>
        /// List of campaigns the person is a player
        /// </summary>
        public List<Campaign> CampaignsAsPlayer { get; set; } = new List<Campaign>();
        
        /// <summary>
        /// List of the persons playable characters
        /// </summary>
        public List<Character> PlayableCharacters { get; set; } = new List<Character>();
        
        /// <summary>
        /// List of the persons playable characters
        /// </summary>
        public List<Character> NonPlayableCharacters { get; set; } = new List<Character>();
    }
}