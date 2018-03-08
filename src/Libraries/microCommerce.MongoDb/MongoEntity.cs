using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace microCommerce.MongoDb
{
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MongoEntity
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}