using System;
using MongoDB.Driver;

namespace TestAspNEtFull.Providers;

public class MobgoDBClient
{
    private static IMongoDatabase _db;
    private static MobgoDBClient _instance;

    public static MobgoDBClient Instance
    {
        get => _instance ?? new MobgoDBClient();
    }

    private MobgoDBClient()
    {
        var connectionString = "mongodb+srv://zherebdenyshpk_db_user:K4bP3i4aRkh96DSa@cluster0.cux4gv7.mongodb.net/?appName=Cluster0"; // або URI від MongoDB Atlas
        var client = new MongoClient(connectionString);
        _db = client.GetDatabase("Test1");

    }

    public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);
}

