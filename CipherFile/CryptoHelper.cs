using System;
using System.IO;
using System.Security.Cryptography;

public static class CryptoHelper
{
    private const int Iterations = 10000; // Hız için düşürüldü

    public static void Encrypt(string input, string output, string pass)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] data = File.ReadAllBytes(input); // Dosyayı komple RAM'e al

        using (var derive = new Rfc2898DeriveBytes(pass, salt, Iterations, HashAlgorithmName.SHA256))
        using (var aes = Aes.Create())
        {
            aes.Key = derive.GetBytes(32);
            aes.GenerateIV();

            using (var ms = new MemoryStream())
            {
                // Dosya yapısı: SALT(16) + IV(16) + DATA
                ms.Write(salt, 0, 16);
                ms.Write(aes.IV, 0, 16);

                using (var encryptor = aes.CreateEncryptor())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
                // EN KRİTİK NOKTA: Dosyayı en son tek seferde yazıyoruz
                File.WriteAllBytes(output, ms.ToArray());
            }
        }
    }

    public static void Decrypt(string input, string output, string pass)
    {
        byte[] allBytes = File.ReadAllBytes(input);
        byte[] salt = new byte[16];
        byte[] iv = new byte[16];
        byte[] encrypted = new byte[allBytes.Length - 32];

        Array.Copy(allBytes, 0, salt, 0, 16);
        Array.Copy(allBytes, 16, iv, 0, 16);
        Array.Copy(allBytes, 32, encrypted, 0, encrypted.Length);

        using (var derive = new Rfc2898DeriveBytes(pass, salt, Iterations, HashAlgorithmName.SHA256))
        using (var aes = Aes.Create())
        {
            aes.Key = derive.GetBytes(32);
            aes.IV = iv;

            using (var msInput = new MemoryStream(encrypted))
            using (var msOutput = new MemoryStream())
            using (var decryptor = aes.CreateDecryptor())
            using (var cs = new CryptoStream(msInput, decryptor, CryptoStreamMode.Read))
            {
                cs.CopyTo(msOutput);
                File.WriteAllBytes(output, msOutput.ToArray());
            }
        }
    }
}