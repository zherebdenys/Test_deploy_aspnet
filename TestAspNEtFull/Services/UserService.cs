using System;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Repositories;

namespace TestAspNEtFull.Services;

public interface IUserService
{
    Task CreateAsync(User todoItem);
    Task<List<User>> GetAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<User> GetAsync(string id);
    Task UpdateAsync(User todoItem);
    Task DeleteAsync(string id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateAsync(User user) => await _userRepository.CreateAsync(user);
    public async Task DeleteAsync(string id) => await _userRepository.DeleteAsync(id);
    public async Task<List<User>> GetAsync() => await _userRepository.GetAsync();
    public async Task<User?> GetByEmailAsync(string email) => await _userRepository.GetByEmailAsync(email);
    public async Task<User> GetAsync(string id) => await _userRepository.GetAsync(id);
    public async Task UpdateAsync(User user) => await _userRepository.UpdateAsync(user);
}
