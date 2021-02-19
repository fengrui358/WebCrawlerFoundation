# WebCrawlerFoundation

- 使用`Page`的`WaitForSelectorAsync`方法等待指定选择器的 dom 元素出现
- 使用`Page`的`WaitForXPathAsync`方法等待指定 xpath 命中的 dom 元素出现，`[]`中括号可使用简单的匹配来匹配目标文本，这一点比`css`选择器强大，如：

``` csharp
//*[@id=\"filterbox1\"]/div/div/div[a='锦城公园校区']"
```
