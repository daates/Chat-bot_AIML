using System;
using System.IO;
using System.Text;
using AIMLbot;

namespace TopoBotCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Bot myBot = new Bot();

            // Абсолютные пути к папкам
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // bin\Debug\net8.0\
            string configDir = Path.Combine(baseDir, "config");
            string aimlDir = Path.Combine(baseDir, "aiml");
            string settingsPath = Path.Combine(configDir, "Settings.xml");

            Console.WriteLine("=== ДИАГНОСТИКА ПУТЕЙ ===");
            Console.WriteLine($"BaseDirectory: {baseDir}");
            Console.WriteLine($"Config dir:    {configDir}");
            Console.WriteLine($"AIML dir:      {aimlDir}");
            Console.WriteLine($"Settings.xml:  {settingsPath}");
            Console.WriteLine("=========================");

            if (!File.Exists(settingsPath))
            {
                Console.WriteLine("ОШИБКА: Settings.xml не найден по ожидаемому пути.");
                Console.ReadLine();
                return;
            }

            try
            {
                // 1. Сообщаем боту, где лежат конфиги и AIML
                myBot.UpdatedConfigDirectory = configDir;
                myBot.UpdatedAimlDirectory = aimlDir;

                // 2. Грузим настройки (через перегрузку с путём)
                myBot.loadSettings(settingsPath);

                // 3. Загружаем все AIML из папки (через стандартный механизм)
                myBot.isAcceptingUserInput = false;
                myBot.loadAIMLFromFiles();
                myBot.isAcceptingUserInput = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при инициализации бота:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadLine();
                return;
            }

            User myUser = new User("User1", myBot);

            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("БОТ-КАРТОГРАФ ЗАПУЩЕН УСПЕШНО!");
            Console.WriteLine("Напиши 'Привет' или 'Что ты умеешь'");
            Console.WriteLine("========================================");

            while (true)
            {
                Console.Write("Ты: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input.Equals("выход", StringComparison.OrdinalIgnoreCase)) break;

                Request r = new Request(input, myUser, myBot);
                Result res = myBot.Chat(r);

                Console.WriteLine("Бот: " + res.Output);
            }
        }
    }
}