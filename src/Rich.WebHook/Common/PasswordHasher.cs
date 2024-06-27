using System.Security.Cryptography;
using System.Text;

namespace Rich.WebHook.Common;

/// <summary>
/// 密码哈希
/// </summary>
public static class PasswordHasher
{
    private static readonly char[] Chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*+-".ToCharArray();

    /// <summary>
    /// 生成随机密码
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GeneratePassword(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentException("Password length must be greater than zero.", nameof(length));
        }

        var password = new char[length];
        var random = new Random();

        for (var i = 0; i < length; i++)
        {
            password[i] = Chars[random.Next(Chars.Length)];
        }

        return new string(password);
    }

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
    /// <param name="passWordSecret"></param>
    /// <returns></returns>
    public static bool VerifyPasswordWithSalt(string password, string passWordSecret)
    {
        var (saltString, storedHash) = FromBase64(passWordSecret);

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

    public static string ToBase64(string salt, string hash)
    {
        var passwordSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{salt}|{hash}"));
        return passwordSecret;
    }

    public static (string salt, string hash) FromBase64(string passwordSecret)
    {
        var base64 = Encoding.UTF8.GetString(Convert.FromBase64String(passwordSecret));

        var split = base64.Split("|");
        return (split[0], split[1]);
    }
}