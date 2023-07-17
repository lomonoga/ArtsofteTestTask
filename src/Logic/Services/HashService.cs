using System.Security.Cryptography;
using System.Text;
using Logic.Interfaces;

namespace Logic.Services;

public class HashService : IHashService
{
    private static readonly byte[] salt = Encoding.UTF8.GetBytes("? ? ? Live get high walk @ @ @");

    /// <summary>
    /// Encrypts the user's password
    /// </summary>
    /// <param name="password">Unencrypted password of the user</param>
    /// <returns>Encrypted password of the user</returns>
    public string EncryptPassword(string password)
    {
        var byteResult = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, 9099);
        return Convert.ToBase64String(byteResult.GetBytes(32));
    }
}