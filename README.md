# WebCrawlerFoundation

## 使用句法

- 使用`Page`的`WaitForSelectorAsync`方法等待指定选择器的 dom 元素出现
- 使用`Page`的`WaitForXPathAsync`方法等待指定 xpath 命中的 dom 元素出现，`[]`中括号可使用简单的匹配来匹配目标文本，这一点比`css`选择器强大，如：

```csharp
//*[@id=\"filterbox1\"]/div/div/div[a='锦城公园校区']"
```

- 对 Url 进行编码，`System.Web.HttpUtility.UrlEncode(url);`
- 使用`ElementHandle`的`TypeAsync`方法输入文本

## 验证

经常在 chrome 浏览器中验证元素选择是否正确

- 使用 `$("your_css_selector")`验证`css`选择器是否正确
- 使用 `$x("your_xpath_selector")`
- 清空 `console`快捷键`ctrl+L`
