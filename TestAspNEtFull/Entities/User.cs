using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestAspNEtFull.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("passwordHash")]
    public string Password { get; set; } = string.Empty;
}
