using System;
using System.IO;
using Newtonsoft.Json;

namespace WebCrawlerFoundation
{
    public class Config
    {
        public bool HeadLess { get; set; }
        
        public int RemoteChromePort { get; set; }

        public static Config LoadConfig()
        {
            var config = JsonConvert.DeserializeObject<Config>(
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json")));

            return config;
        }
    }
}
