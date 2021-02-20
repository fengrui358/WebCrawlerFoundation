using System.Threading.Tasks;
using PuppeteerSharp;

namespace WebCrawlerFoundation
{
    public abstract class TaskBase
    {
        private Page _page;

        /// <summary>
        /// 目标地址
        /// </summary>
        protected virtual string Url { get; }

        /// <summary>
        /// 浏览器
        /// </summary>
        protected Browser Browser { get; }

        protected TaskBase(Browser browser)
        {
            Browser = browser;
        }

        protected async Task<Page> GetNewPage()
        {
            return await Browser.NewPageAsync();
        }

        protected async Task<Page> GetLastingPage()
        {
            _page ??= await Browser.NewPageAsync();
            return _page;
        }

        protected async void Close()
        {
            var pages = await Browser.PagesAsync();
            foreach (var page in pages)
            {
                await page.CloseAsync();
            }
        }
    }
}