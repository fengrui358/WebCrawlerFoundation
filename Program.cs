using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using WebCrawlerFoundation.Demos;
using WebCrawlerFoundation.Helpers;

namespace WebCrawlerFoundation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var headless =
                ((JObject) JsonConvert.DeserializeObject(
                    File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"))))
                ?.GetValue("HeadLess")?.Value<bool>();

            var browser = await PuppeteerHelper.GetBrowser(new LaunchOptions {Headless = headless ?? true});
            
            //google 翻译demo
            var googleDemo = new GoogleTranslationDemo(browser);
            var str1 = await googleDemo.ToEnglish("你好吗？");
            ConsoleHelper.Console(str1);

            var str2 = await googleDemo.ToChinese("Hello world!");
            ConsoleHelper.Console(str2);

            Console.ReadLine();
        }
    }
}