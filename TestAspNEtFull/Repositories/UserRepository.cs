using System;
using MongoDB.Driver;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Providers;

namespace TestAspNEtFull.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<List<User>> GetAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<User> GetAsync(string id);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
}

public class UserRepository : IUserRepository
{
    private IMongoCollection<User> _collection;

    public UserRepository()
    {
        _collection = MobgoDBClient.Instance.GetCollection<User>("Users");
    }

    // Read
    public async Task<User> GetAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task<User?> GetByEmailAsync(string email)
        => await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    public async Task<List<User>> GetAsync() => await _collection.Find(x => true).ToListAsync();

    // Create, Update, Delete
    public async Task CreateAsync(User user) => await _collection.InsertOneAsync(user);
    public async Task DeleteAsync(string id) => await _collection.DeleteOneAsync(x => x.Id == id);
    public async Task UpdateAsync(User user) => await _collection.ReplaceOneAsync(x => x.Id == user.Id, user);
}

