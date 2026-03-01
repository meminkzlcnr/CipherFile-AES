using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== KESİN ÇÖZÜM ŞİFRELEME ===");
        Console.WriteLine("1 - Şifrele\n2 - Çöz");
        string secim = Console.ReadLine();

        Console.Write("Dosyayı buraya sürükle ve Enter'a bas: ");
        string path = Console.ReadLine().Trim().Replace("\"", "");

        if (!File.Exists(path))
        {
            Console.WriteLine("Hata: Dosya bulunamadı!");
            return;
        }

        Console.Write("Şifre belirle: ");
        string pass = Console.ReadLine();

        // Çıktı dosyasını otomatik belirleyelim (Karışıklığı önlemek için)
        string outPath = secim == "1" ? path + ".enc" : path.Replace(".enc", "") + "_cozulden.txt";

        try
        {
            if (secim == "1") CryptoHelper.Encrypt(path, outPath, pass);
            else CryptoHelper.Decrypt(path, outPath, pass);

            Console.WriteLine($"\nİŞLEM TAMAM!");
            Console.WriteLine($"Oluşan Dosya: {outPath}");
            Console.WriteLine($"Dosya Boyutu: {new FileInfo(outPath).Length} byte");
        }
        catch (Exception ex)
        {
            Console.WriteLine("HATA: " + ex.Message);
        }
        Console.ReadKey();
    }
}