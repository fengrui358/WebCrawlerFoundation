using System;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
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
        public static async Task<Browser> GetBrowser(LaunchOptions launchOptions = null)
        {
            launchOptions ??= new LaunchOptions
            {
                Headless = true
            };

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