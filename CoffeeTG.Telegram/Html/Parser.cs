using CoffeeTG.Telegram.Model;
using CoffeeTG.Telegram.Request;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CoffeeTG.Telegram.Html
{
    internal class Parser
    {
        public async static void Run(HttpClient httpClient, TelegramBotClient botClient)
        {
            Settings settings = new();
            while (true)
            {
                if ((23 == DateTime.Now.Hour) &&
                    (55 == DateTime.Now.Minute))
                {
                    var response = await AutorizationUnicum(settings.Root.User.Login,
                            settings.Root.User.Password, settings.Root.User.Httpauthreqtype, httpClient);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine($"Autorization unicum successfyly!");
                    }
                    else
                    {
                        Console.WriteLine($"Error connetcion: {response.StatusCode}");
                    }

                    var result = await ParseSalesAnalysis(httpClient, @"https://online.unicum.ru/n/sgraph.html?V0300009BE20200002760");
                    await botClient.SendTextMessageAsync(-1001396271437, result);
                }
                await Task.Delay(60000);
            }
        }

        public static async Task<string> ParseEvents(HttpClient httpClient, string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                string html = null;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    html = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(html))
                    {
                        HtmlDocument doc = new();
                        doc.LoadHtml(html);

                        var events = doc.DocumentNode.SelectNodes("/html/body/table[3]/tr");

                        if (events != null && events.Count > 0)
                        {
                            foreach (var item in events)
                            {
                                var date = item.SelectSingleNode("/td/font");
                                Console.WriteLine(date.InnerText);
                            }
                        }
                        else
                            return "Нет данных, произошла ошибка парсинга.";
                    }
                }
                else
                    return $"Ошибка подлючения бота к сайту. Статус ошибки: {response.StatusCode}";

            }
            catch (Exception)
            { }

            return null;
        }

        public static async Task<string> ParseSalesAnalysis(HttpClient httpClient, string url)
        {
            try
            {
                Dictionary<string, string> payload = new()
                {
                    {"day", $"{DateTime.Now.Day}"},
                    {"month", $"{DateTime.Now.Month}"},
                    {"year", $"{DateTime.Now.Year}"},
                    {"day1", $"{DateTime.Now.Day}"},
                    {"month1", $"{DateTime.Now.Month}"},
                    {"year1", $"{DateTime.Now.Year}"}
                };

                PostRequest postRequest = new(payload, url, httpClient);

                var response = await postRequest.Operation();
                string html = null;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    html = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(html))
                    {
                        HtmlDocument doc = new();
                        doc.LoadHtml(html);

                        var espresso = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[2]/td[9]");
                        var doubleEspresso = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[3]/td[9]");
                        var americano = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[4]/td[9]");
                        var chcolate = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[5]/td[9]");
                        var hotWater = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[6]/td[9]");
                        var cappuccino = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[7]/td[9]");
                        var mochachino = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[8]/td[9]");
                        var coffeeWithMilk = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[9]/td[9]");
                        var latte = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[10]/td[9]");
                        var vanillaCappuccino = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[11]/td[9]");
                        var total = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[12]/td[9]");

                        return $"❗️Ежедневный отчет на {DateTime.Now}❗️\n" +
                               $"=============================================\n" +
                               $"☕Кофе эспрессо: {espresso.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Двойной эспрессо: {doubleEspresso.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Кофе американо: {americano.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Горячий шоколад: {chcolate.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Горячая вода: {hotWater.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Кофе капучино: {cappuccino.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Кофе мокачино: {mochachino.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Кофе с молоком: {coffeeWithMilk.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Латте макиато: {latte.InnerText.Replace("&nbsp;", "")}\n" +
                               $"☕Ванильный капучино: {vanillaCappuccino.InnerText.Replace("&nbsp;", "")}\n" +
                               $"=============================================\n"+
                               $"Итого: {total.InnerText.Replace("&nbsp;", "")}";
                    }
                    else
                        Console.WriteLine($"Html is null!");
                }
                else
                    Console.WriteLine($"Error request! Status code: {response.StatusCode}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknow exception: {ex.Message}");
            }
            return null;
        }

        static async Task<HttpResponseMessage> AutorizationUnicum(string login, string password, string httpauthreqtype, HttpClient httpClient)
        {
            Dictionary<string, string> payload = new()
            {
                {"httpauthreqtype", httpauthreqtype},
                {"Login", login},
                {"Password", password}
            };

            PostRequest postRequest = new(payload, @"https://online.unicum.ru/n/user/recovery.html", httpClient);
            var response = await postRequest.Operation();

            return response;
        }
    }


}


