using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using FrHello.NetLib.Core.Net;
using PuppeteerSharp;

namespace WebCrawlerFoundation.Helpers
{
    public static class PuppeteerHelper
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool _isInit;
        
        /// <summary>
        /// 获取一个浏览器
        /// </summary>
        /// <returns></returns>
        public static async Task<Browser> GetBrowser()
        {
            var config = Config.LoadConfig();
            
            var launchOptions = new LaunchOptions
            {
                Headless = config.HeadLess
            };

            if (config.RemoteChromePort != 0 && NetHelper.CheckLocalPort(9222))
            {
                try
                {
                    return await Puppeteer.ConnectAsync(new ConnectOptions { BrowserURL = "http://localhost:9222" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            await TryInit();

            var browser = await Puppeteer.LaunchAsync(launchOptions);
            return browser;
        }
        
        /// <summary>
        /// 截屏保存在当前目录下
        /// </summary>
        /// <param name="page">页面</param>
        /// <returns></returns>
        public static async Task ScreenshotAsync(this Page page)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            await page.ScreenshotAsync(Path.Combine(dir, $"{DateTime.Now:yyyyMMddhhmmss}.png"));
        }

        /// <summary>
        /// 截屏保存在当前目录下
        /// </summary>
        /// <param name="elementHandle">界面元素</param>
        /// <returns></returns>
        public static async Task ScreenshotAsync(this ElementHandle elementHandle)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            await elementHandle.ScreenshotAsync(Path.Combine(dir, $"{DateTime.Now:yyyyMMddhhmmss}.png"));
        }

        /// <summary>
        /// 获取page中的document
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task<IDocument> GetDocument(Page page)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var html = await page.GetContentAsync();
            var document = await context.OpenAsync(q => q.Content(html));

            return document;
        }

        /// <summary>
        /// 根据文件名获取javascript
        /// </summary>
        /// <param name="javaScriptName"></param>
        /// <returns></returns>
        public static async Task<string> GetJavaScript(string javaScriptName)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsScripts");
            if (Directory.Exists(dir))
            {
                var files = Directory.GetFiles(dir, javaScriptName, SearchOption.AllDirectories);
                if (files.Any())
                {
                    return await File.ReadAllTextAsync(files[0], Encoding.UTF8);
                }
            }

            throw new InvalidOperationException($"未找到 {javaScriptName}");
        }

        /// <summary>
        /// 用XPath表达式查找
        /// </summary>
        /// <param name="page"></param>
        /// <param name="xpath"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<ElementHandle> WaitForXPathAsyncWithOutException(this Page page, string xpath, int timeout = 100)
        {
            try
            {
                var result = await page.WaitForXPathAsync(xpath, new WaitForSelectorOptions
                {
                    Timeout = timeout
                });

                return result;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception e)
            {
            }

            return null;
        }

        /// <summary>
        /// 用Selector表达式查找
        /// </summary>
        /// <param name="page"></param>
        /// <param name="selector"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<ElementHandle> WaitForSelectorAsyncWithOutException(this Page page, string selector, int timeout = 100)
        {
            try
            {
                var result = await page.WaitForSelectorAsync(selector, new WaitForSelectorOptions
                {
                    Timeout = timeout
                });

                return result;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception e)
            {
            }

            return null;
        }

        /// <summary>
        /// 注入JQuery框架
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task InjectJQuery(this Page page)
        {
            await InjectJavascript(page, "https://code.jquery.com/jquery-3.6.0.min.js");
        }

        public static async Task InjectJavascript(this Page page, string url)
        {
            var script =
                $"var head = document.getElementsByTagName('head')[0];var script = document.createElement('script');script.type = 'text/javascript';script.src = '{url}';head.appendChild(script);";

            await page.EvaluateExpressionAsync(script);
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        private static async Task TryInit()
        {
            if (!_isInit)
            {
                ConsoleHelper.Console($"查找chromium版本");
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

                _isInit = true;
            }
        }
    }
}