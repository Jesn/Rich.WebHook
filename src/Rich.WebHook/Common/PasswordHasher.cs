using System.Security.Cryptography;
using System.Text;

namespace Rich.WebHook.Common;

/// <summary>
/// 密码哈希
/// </summary>
public class PasswordHasher
{
    /// <summary>
    /// 创建密码哈希
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static (string, string) HashPasswordWithSalt(string password)
    {
        using var sha256 = SHA256.Create();
        // 生成盐
        var salt = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // 将密码和盐转换为字节数组
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPassword = new byte[salt.Length + passwordBytes.Length];
        Array.Copy(salt, saltedPassword, salt.Length);
        Array.Copy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

        // 计算哈希值
        var hash = sha256.ComputeHash(saltedPassword);

        // 将盐和哈希值转换为字符串
        var saltString = Convert.ToBase64String(salt);
        var hashString = Convert.ToBase64String(hash);

        return (saltString, hashString);
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    /// <param name="password"></param>
    /// <param name="saltString"></param>
    /// <param name="storedHash"></param>
    /// <returns></returns>
    public static bool VerifyPasswordWithSalt(string password, string saltString, string storedHash)
    {
        using SHA256 sha256 = SHA256.Create();
        // 将盐从字符串转换为字节数组
        var salt = Convert.FromBase64String(saltString);

        // 将密码和盐合并
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var saltedPassword = new byte[salt.Length + passwordBytes.Length];
        Array.Copy(salt, saltedPassword, salt.Length);
        Array.Copy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

        // 计算哈希值
        var hash = sha256.ComputeHash(saltedPassword);

        // 将计算出的哈希值与存储的哈希值进行比较
        return Convert.ToBase64String(hash) == storedHash;
    }
}