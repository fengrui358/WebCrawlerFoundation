using System;
using System.Threading.Tasks;
using Exceptionless;
using WebCrawlerFoundation.Demos;
using WebCrawlerFoundation.Helpers;

namespace WebCrawlerFoundation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ExceptionlessClient.Default.Startup("");
            
            var browser = await PuppeteerHelper.GetBrowser();

            ////google 翻译demo
            var googleDemo = new GoogleTranslationDemo(browser);
            var str1 = await googleDemo.ToEnglish("你好吗？");
            ConsoleHelper.Console(str1);

            //var str2 = await googleDemo.ToChinese("Hello world!");
            //ConsoleHelper.Console(str2);

            //var email = new Clean163Email(browser);
            //await email.Run();

            Console.ReadLine();
        }
    }
}