using System;
using System.Security.Cryptography;
using System.Text;

namespace TestAspNEtFull.Helpers;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool Verify(string password, string hash);
}

// Простий сервіс для хешування паролів за допомогою SHA256, САМА ПРОСТА РЕАЛІЗАЦІЯ, В РЕАЛЬНИХ ПРОЕКТАХ ВИКОРИСТОВУЙТЕ БІБЛІОТЕКИ НАПРИКЛАД BCrypt
public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool Verify(string password, string hash)
    {
        var computed = HashPassword(password);
        return computed == hash;
    }
}



