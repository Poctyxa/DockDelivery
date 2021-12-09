using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DockDelivery.Domain.Entities
{
    public abstract class EntityBase
    {
        public string Id { get; set; }
    }
}
