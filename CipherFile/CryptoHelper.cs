using System;
using System.IO;
using System.Security.Cryptography;

public static class CryptoHelper
{
    private const int KeySize = 32; // 256 bit
    private const int IvSize = 16;  // 128 bit
    private const int Iterations = 100000;

    public static void EncryptFile(string inputFile, string outputFile, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        using var aes = Aes.Create();

        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        aes.Key = key.GetBytes(KeySize);
        aes.GenerateIV();

        using FileStream fsOutput = new FileStream(outputFile, FileMode.Create);

        fsOutput.Write(salt, 0, salt.Length);
        fsOutput.Write(aes.IV, 0, aes.IV.Length);

        using CryptoStream cryptoStream = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using FileStream fsInput = new FileStream(inputFile, FileMode.Open);

        fsInput.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock(); // ÖNEMLİ
    }

    public static void DecryptFile(string inputFile, string outputFile, string password)
    {
        using FileStream fsInput = new FileStream(inputFile, FileMode.Open);

        byte[] salt = new byte[16];
        fsInput.Read(salt, 0, 16);

        byte[] iv = new byte[16];
        fsInput.Read(iv, 0, 16);

        using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        using var aes = Aes.Create();

        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        aes.Key = key.GetBytes(KeySize);
        aes.IV = iv;

        using CryptoStream cryptoStream = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using FileStream fsOutput = new FileStream(outputFile, FileMode.Create);

        cryptoStream.CopyTo(fsOutput);
    }
}