using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("1 - Dosya Şifrele");
        Console.WriteLine("2 - Dosya Çöz");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        Console.Write("Dosya yolu: ");
        string input = Console.ReadLine();

        Console.Write("Çıktı dosya adı: ");
        string output = Console.ReadLine();

        Console.Write("Şifre: ");
        string password = Console.ReadLine();

        try
        {
            if (secim == "1")
            {
                CryptoHelper.EncryptFile(input, output, password);
                Console.WriteLine("Dosya başarıyla şifrelendi.");
            }
            else if (secim == "2")
            {
                CryptoHelper.DecryptFile(input, output, password);
                Console.WriteLine("Dosya başarıyla çözüldü.");
            }
            else
            {
                Console.WriteLine("Geçersiz seçim.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata oluştu: " + ex.Message);
        }

        Console.WriteLine("İşlem tamamlandı.");
    }
}