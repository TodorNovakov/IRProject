using Abot.Crawler;
using Abot.Poco;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectIRBgMamma.Infrasctructure
{
    public static class CrawlData
    {
        public static IWebCrawler GetDefaultWebCrawler()
        {
            return new PoliteWebCrawler();
        }

        public static IWebCrawler GetManuallyConfiguredWebCrawler()
        {
            //Create a config object manually
            CrawlConfiguration config = new CrawlConfiguration();
            config.CrawlTimeoutSeconds = 0;
            config.DownloadableContentTypes = "text/html, text/plain";
            config.IsExternalPageCrawlingEnabled = false;
            config.IsExternalPageLinksCrawlingEnabled = false;
            config.IsRespectRobotsDotTextEnabled = false;
            config.IsUriRecrawlingEnabled = false;
            config.MaxConcurrentThreads = 10;
            config.MaxPagesToCrawl = 10;
            config.MaxPagesToCrawlPerDomain = 0;
            config.MinCrawlDelayPerDomainMilliSeconds = 1000;

            //Add you own values without modifying Abot's source code.
            //These are accessible in CrawlContext.CrawlConfuration.ConfigurationException object throughout the crawl
            config.ConfigurationExtensions.Add("Somekey1", "SomeValue1");
            config.ConfigurationExtensions.Add("Somekey2", "SomeValue2");

            //Initialize the crawler with custom configuration created above.
            //This override the app.config file values
            return new PoliteWebCrawler(config, null, null, null, null, null, null, null, null);
        }

        public static IWebCrawler GetCustomBehaviorUsingLambdaWebCrawler()
        {
            IWebCrawler crawler = GetDefaultWebCrawler();

            //Register a lambda expression that will make Abot not crawl any url that has the word "ghost" in it.
            //For example http://a.com/ghost, would not get crawled if the link were found during the crawl.
            //If you set the log4net log level to "DEBUG" you will see a log message when any page is not allowed to be crawled.
            //NOTE: This is lambda is run after the regular ICrawlDecsionMaker.ShouldCrawlPage method is run.
            crawler.ShouldCrawlPage((pageToCrawl, crawlContext) =>
            {
                if (pageToCrawl.Uri.AbsoluteUri.Contains("topic=") || pageToCrawl.Uri.AbsoluteUri.Contains("board=376") || pageToCrawl.Uri.AbsoluteUri.Contains("board=376."))
                    return new CrawlDecision { Allow = true };

                return new CrawlDecision { Allow = false, Reason = "Scared of ghosts" };
            });

            //Register a lambda expression that will tell Abot to not download the page content for any page after 5th.
            //Abot will still make the http request but will not read the raw content from the stream
            //NOTE: This lambda is run after the regular ICrawlDecsionMaker.ShouldDownloadPageContent method is run
            //crawler.ShouldDownloadPageContent((crawledPage, crawlContext) =>
            //{
            //    if (crawlContext.CrawledCount >= 5)
            //        return new CrawlDecision { Allow = false, Reason = "We already downloaded the raw page content for 5 pages" };

            //    return new CrawlDecision { Allow = true };
            //});

            //Register a lambda expression that will tell Abot to not crawl links on any page that is not internal to the root uri.
            //NOTE: This lambda is run after the regular ICrawlDecsionMaker.ShouldCrawlPageLinks method is run
            crawler.ShouldCrawlPageLinks((crawledPage, crawlContext) =>
            {
                if (!crawledPage.IsInternal)
                    return new CrawlDecision { Allow = false, Reason = "We dont crawl links of external pages" };

                if (crawledPage.Uri.AbsoluteUri.Contains("board=376.") || crawledPage.Uri.AbsoluteUri.Contains("topic=") || crawledPage.Uri.AbsoluteUri.Contains("board=376"))
                    return new CrawlDecision { Allow = true };
                return new CrawlDecision { Allow = false, Reason = "We dont crawl links of external pages" };
            });

            return crawler;
        }

        public static Uri GetSiteToCrawl(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                throw new ApplicationException("Site url to crawl is as a required parameter");

            return new Uri(args);
        }

        public static void PrintDisclaimer()
        {
            PrintAttentionText("The demo is configured to only crawl a total of 10 pages and will wait 1 second in between http requests. This is to avoid getting you blocked by your isp or the sites you are trying to crawl. You can change these values in the app.config or Abot.Console.exe.config file.");
        }

        public static void PrintAttentionText(string text)
        {
            ConsoleColor originalColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(text);
            System.Console.ForegroundColor = originalColor;
        }

        public static void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            //Process data
            var crawledPage = e.PageToCrawl;
        }

        public static void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            //Process data
            CrawledPage crawledPage = e.CrawledPage;
            var html = crawledPage.Content.Text;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string className = "post pb_article";
            var findclasses = crawledPage.HtmlDocument.DocumentNode.Descendants().Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(className)).ToList();

            if(findclasses.Count > 0)
            {
                foreach (var node in findclasses)
                {
                    string value = node.InnerText;
                }
            }

            var a = ExtractText(html);
        }

        public static void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            //Process data
        }

        public static void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            //Process data
        }

        public static string ExtractText(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();

            foreach (var item in doc.DocumentNode.DescendantsAndSelf())
            {
                if (item.NodeType == HtmlNodeType.Text)
                {
                    if (item.InnerText.Trim() != "")
                    {
                        chunks.Add(item.InnerText.Trim());
                    }
                }
            }
            return String.Join(" ", chunks);
        }
    }
}