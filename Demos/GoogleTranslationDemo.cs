using System;
using System.Linq;
using System.Threading.Tasks;
using PuppeteerSharp;
using WebCrawlerFoundation.Helpers;

namespace WebCrawlerFoundation.Demos
{
    public class GoogleTranslationDemo
    {
        private readonly string _urlTemplate = "https://translate.google.cn/?sl=auto&tl={0}&text={1}&op=translate";
        private readonly Browser _browser;
        private Page _page;

        public GoogleTranslationDemo(Browser browser)
        {
            _browser = browser;
        }

        public async Task<string> ToChinese(string str)
        {
            return await Translate("zh-CN", str);
        }

        public async Task<string> ToEnglish(string str)
        {
            return await Translate("en", str);
        }

        private async Task<string> Translate(string toLanguage, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            _page ??= await _browser.NewPageAsync();

            str = System.Web.HttpUtility.UrlEncode(str);
            var url = string.Format(_urlTemplate, toLanguage, str);

            try
            {
                var response = await _page.GoToAsync(url);
                if (response.Ok)
                {
                    await _page.WaitForSelectorAsync(
                        "#yDmH0d > c-wiz > div > div.WFnNle > c-wiz > div.OlSOob > c-wiz > div.ccvoYb > div.AxqVh > div.OPPzxe > c-wiz.P6w8m.BDJ8fb > div.dePhmb > div > div.J0lOec > span.VIiyi > span > span");

                    var document = await PuppeteerHelper.GetDocument(_page);
                    var element = document.Body.QuerySelector(
                        "#yDmH0d > c-wiz > div > div.WFnNle > c-wiz > div.OlSOob > c-wiz > div.ccvoYb > div.AxqVh > div.OPPzxe > c-wiz.P6w8m.BDJ8fb > div.dePhmb > div > div.J0lOec > span.VIiyi > span > span");

                    return element.InnerHtml;
                }
            }
            catch (Exception e)
            {
                ConsoleHelper.Console(e);
            }

            return string.Empty;
        }
    }
}
