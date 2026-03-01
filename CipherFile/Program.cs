using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== AES DOSYA ŞİFRELEME ===");
        Console.WriteLine("1 - Şifrele");
        Console.WriteLine("2 - Çöz");
        Console.Write("Seçim: ");
        string secim = Console.ReadLine();

        Console.Write("Dosyayı buraya sürükle ve Enter'a bas: ");
        string path = Console.ReadLine().Trim().Replace("\"", "");

        if (!File.Exists(path))
        {
            Console.WriteLine("Hata: Dosya bulunamadı!");
            return;
        }

        Console.Write("Şifre gir: ");
        string pass = Console.ReadLine();

        string outPath;

        try
        {
            if (secim == "1")
            {
                // Şifreleme
                outPath = path + ".enc";
                CryptoHelper.Encrypt(path, outPath, pass);
            }
            else if (secim == "2")
            {
                // Çözme için güvenlik kontrolü
                if (!path.EndsWith(".enc"))
                {
                    Console.WriteLine("HATA: Çözme için .enc uzantılı dosya seçmelisin!");
                    return;
                }

                outPath = path.Substring(0, path.Length - 4) + "_cozuldu.txt";
                CryptoHelper.Decrypt(path, outPath, pass);
            }
            else
            {
                Console.WriteLine("Geçersiz seçim!");
                return;
            }

            Console.WriteLine("\nİŞLEM TAMAMLANDI!");
            Console.WriteLine($"Oluşan Dosya: {outPath}");
            Console.WriteLine($"Dosya Boyutu: {new FileInfo(outPath).Length} byte");
        }
        catch (Exception ex)
        {
            Console.WriteLine("HATA: " + ex.Message);
        }

        Console.WriteLine("\nÇıkmak için bir tuşa bas...");
        Console.ReadKey();
    }
}