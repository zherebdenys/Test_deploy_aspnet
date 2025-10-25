using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestAspNEtFull.Entities;

public class TodoItemEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("is_complete")]
    public bool IsComplete { get; set; }
}

