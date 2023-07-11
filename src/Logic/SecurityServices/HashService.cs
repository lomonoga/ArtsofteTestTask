using System.Security.Cryptography;
using System.Text;
using Logic.Interfaces;

namespace Logic.SecurityServices;

public class HashService : IHashService
{
    private static readonly byte[] salt = Encoding.UTF8.GetBytes("? ? ? Live get high walk @ @ @");

    public string EncryptPassword(string password)
    {
        var byteResult = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, 9099);
        return Convert.ToBase64String(byteResult.GetBytes(32));
    }
}