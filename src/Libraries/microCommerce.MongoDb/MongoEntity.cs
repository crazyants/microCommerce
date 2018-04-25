using microCommerce.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace microCommerce.MongoDb
{
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = false)]
    public abstract class MongoEntity : BaseEntityTypeId<string>
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public override string Id { get; set; }
    }
}