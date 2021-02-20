using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Attributes;
using PuppeteerSharp;

namespace WebCrawlerFoundation.Demos
{
    public class Clean163Email : TaskBase
    {
        protected override string Url { get; } = "https://mail.163.com/";

        public Clean163Email(Browser browser) : base(browser)
        {
        }

        public async Task Run()
        {
            var page = await GetLastingPage();
            var response = await page.GoToAsync(Url);

            if (response.Ok)
            {
                var frame = page.Frames.First(s=>s.Name.Contains("x-URS-iframe"));
                
                //等待手工操作
                var user = await frame.WaitForSelectorAsync("input[data-placeholder='邮箱帐号或手机号码']");
                await user.TypeAsync("");

                var password = await frame.WaitForSelectorAsync("input[data-placeholder='输入密码']");
                await password.TypeAsync("");

                await frame.ClickAsync("#dologin");

                var element = await page.WaitForXPathAsync("//*[a='清理邮箱']");
                var cleanBtn = await element.XPathAsync("a[1]");
                await cleanBtn[0].ClickAsync();

                await Task.Delay(3000);
                var frame2 = page.Frames.First(s => s.Name.Contains("frmoutlink"));
                
                await frame2.ClickAsync("#clearTypeDate");
                      
                await frame2.ClickAsync("#dateCleanCustom");
                      
                await frame2.TypeAsync("#customYearStartIpt", "1990");
                await frame2.TypeAsync("#customMonthStartIpt", "1");
                await frame2.TypeAsync("#customDayStartIpt", "1");
                await frame2.TypeAsync("#customYearEndIpt", "2021");
                await frame2.TypeAsync("#customMonthEndIpt", "2");
                await frame2.TypeAsync("#customDayEndIpt", "18");

                for (int i = 0; i < 100000; i++)
                {
                    var b1 = await frame2.WaitForXPathAsync("//*/div[span='开始扫描']");
                    await b1.ClickAsync();

                    await Task.Delay(5000);

                    var deleteBtn = await frame2.WaitForXPathAsync("//div[span='彻底删除']");
                    await deleteBtn.ClickAsync();

                    var confirmBtn = await page.WaitForXPathAsync("//div[span='确 定']");
                    await confirmBtn.ClickAsync();

                    var confirm2Btn = await page.WaitForXPathAsync("//div[span='确 定']");
                    await confirm2Btn.ClickAsync();
                }
            }
        }
    }
}