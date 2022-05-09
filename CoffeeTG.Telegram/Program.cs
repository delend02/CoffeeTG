using CoffeeTG.Telegram.Html;
using CoffeeTG.Telegram.Model;
using System;
using System.Net.Http;
using System.Text;
using Telegram.Bot;


Settings settings = new();

HttpClient client = new(); 

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var botClient = new TelegramBotClient($"{settings.Root.СonnectionString.TelegramBot}");

Parser.Run(client, botClient);

Console.WriteLine("Для завершения работы телеграм бота нажмите на любую кнопку");
Console.ReadKey();


