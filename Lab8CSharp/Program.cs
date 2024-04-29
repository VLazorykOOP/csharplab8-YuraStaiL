using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Task1()
    {
        string inputFile = "input1.txt";
        string outputFile = "output1.txt";
        int count = 0;
        List<string> finded = new List<string>();
        try
        {
            string[] lines = File.ReadAllLines(inputFile);
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                foreach (string line in lines)
                {
                    // Розділяю рядок на слова за допомогою пробілів та розділових знаків
                    string[] words = Regex.Split(line, @"[\s,]+");

                    foreach (string word in words)
                    {
                        Console.WriteLine(word);
                        if (word.Contains(".com"))
                        {
                            finded.Add(word);
                            writer.WriteLine(word);
                            count++;
                        }
                    }
                }
                writer.Close();
            }

            Console.WriteLine($"Знайдено {count} підтекстів з доменом .com.");
            foreach (string word in finded)
            {
                Console.WriteLine("\t- " + word);
            }
            Console.WriteLine("Операція завершена успішно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void Task2()
    {
        string inputFile = "input2.txt";
        string outputFile = "output2.txt"; 

        try
        {
            string text = File.ReadAllText(inputFile);

            // Регулярний вираз для виявлення українських слів, що починаються з голосної літери
            string pattern = @"\b[аеиіїоуюяєАЕИІЇОУЮЯЄ][\p{L}']*\b";
            MatchCollection matches = Regex.Matches(text, pattern);
            Console.WriteLine("Знайдені українські слова, які починаються з голосної літери:");
            foreach (Match match in matches)
            {
                Console.WriteLine($"\t- {match.Value}");
            }

            string result = Regex.Replace(text, pattern, string.Empty);
            File.WriteAllText(outputFile, result);
            Console.WriteLine($"Знайдені слова було видалено з тексту і збережено у файлі: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static string RemoveSubsequent(string word)
    {
        if (string.IsNullOrEmpty(word) || word.Length < 2)
            return word;

        char firstLetter = word[0];
        string remainingLetters = word.Substring(1);

        // Вилучаємо всі наступні входження першої літери
        string processedWord = firstLetter + remainingLetters.Replace(firstLetter.ToString(), "");

        return processedWord;
    }

    static void Task3()
    {
        string inputText = "Текст із словами, які потрібно обробити. припливли, приклад словосполучення!";

        // Розділяю текст на слова за допомогою пробілів та розділових знаків
        string[] words = inputText.Split(new char[] { ' ', ',', '.', '!', '?', ':', ';' }, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"Початковий текст: {inputText}");
        Console.Write("Оброблений текст:");
        foreach (string word in words)
        {
            string processedWord = RemoveSubsequent(word);
            Console.Write(processedWord + " ");
        }

        Console.WriteLine();
    }

    static void Task4()
    {
        int[] sequence = { 10, -5, 8, 0, 20, -3, 15, -7, 25 };
        string fileName = "positive_numbers.bin";

        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (int number in sequence)
                {
                    if (number > 0)
                    {
                        writer.Write(number);
                    }
                }
            }

            Console.WriteLine("Додатні числа було записано у файл: " + fileName);
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                Console.WriteLine("Вміст файлу " + fileName + ":");
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int number = reader.ReadInt32();
                    Console.WriteLine(number);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Сталася помилка: " + ex.Message);
        }
    }

    static void Task5()
    {
        try
        {
            string basePath = @"D:\temp\";
            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }

            string faryna1Path = Path.Combine(basePath, "Faryna1");
            string faryna2Path = Path.Combine(basePath, "Faryna2");

            Directory.CreateDirectory(faryna1Path);
            Directory.CreateDirectory(faryna2Path);

            string t1Path = Path.Combine(faryna1Path, "t1.txt");
            File.WriteAllText(t1Path, "Щербенко Ігор Іванович, 1993 року народження, місце проживання м. Чернівці");

            string t2Path = Path.Combine(faryna1Path, "t2.txt");
            File.WriteAllText(t2Path, "Комар Сергій Федорович, 1998 року народження, місце проживання м. Рівне");

            string t3Path = Path.Combine(faryna2Path, "t3.txt");
            File.Copy(t1Path, t3Path, true); // Копіюю вміст файлу t1.txt в t3.txt

            // Додаю вміст файлу t2.txt в кінець файлу t3.txt
            File.AppendAllText(t3Path, Environment.NewLine + File.ReadAllText(t2Path));

            Console.WriteLine("Створені файли:");
            Console.WriteLine($"- {t1Path}");
            Console.WriteLine($"- {t2Path}");
            Console.WriteLine($"- {t3Path}");
            foreach (string path in new string[3] { t1Path, t2Path, t3Path })
            {
                FileInfo fileInfo = new FileInfo(path);
                Console.WriteLine($"Назва файлу: {fileInfo.Name}");
                Console.WriteLine($"Час створення: {fileInfo.CreationTime}");
                Console.WriteLine($"Розмір: {fileInfo.Length}");
            }
            
 
            string newT2Path = Path.Combine(faryna2Path, Path.GetFileName(t2Path));
            File.Move(t2Path, newT2Path);

            string copyT1Path = Path.Combine(faryna2Path, Path.GetFileName(t1Path));
            File.Copy(t1Path, copyT1Path);

            Directory.Move(faryna2Path, Path.Combine(basePath, "ALL"));
            Directory.Delete(faryna1Path, true);

            string[] allFiles = Directory.GetFiles(Path.Combine(basePath, "ALL"));
            Console.WriteLine("\n\nПовна інформація про файли у папці ALL:");
            foreach (string file in allFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                Console.WriteLine($"Назва файлу: {fileInfo.Name}");
                Console.WriteLine($"Час створення: {fileInfo.CreationTime}");
                Console.WriteLine($"Розмір: {fileInfo.Length}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
     
    }

    static void Main(string[] args)
    {
        Console.WriteLine("\n\n\tTASK 1");
        Task1();

        Console.WriteLine("\n\n\tTASK 2");
        Task2();

        Console.WriteLine("\n\n\tTASK 3");
        Task3();

        Console.WriteLine("\n\n\tTASK 4");
        Task4();

        Console.WriteLine("\n\n\tTASK 5");
        Task5();
    }
}
