using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectIRBgMamma.Infrasctructure;

namespace ProjectIRBgMamma.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            log4net.Config.XmlConfigurator.Configure();
            CrawlData.PrintDisclaimer();

            Uri uriToCrawl = CrawlData.GetSiteToCrawl("http://www.bg-mamma.com/index.php?board=376");

            IWebCrawler crawler;

            //Uncomment only one of the following to see that instance in action
            crawler = CrawlData.GetCustomBehaviorUsingLambdaWebCrawler();
            //crawler = GetManuallyConfiguredWebCrawler();
            //crawler = GetCustomBehaviorUsingLambdaWebCrawler();

            //Subscribe to any of these asynchronous events, there are also sychronous versions of each.
            //This is where you process data about specific events of the crawl
            crawler.PageCrawlStartingAsync += CrawlData.crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += CrawlData.crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += CrawlData.crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += CrawlData.crawler_PageLinksCrawlDisallowed;

            //Start the crawl
            //This is a synchronous call
            CrawlResult result = crawler.Crawl(uriToCrawl);

            //Now go view the log.txt file that is in the same directory as this executable. It has
            //all the statements that you were trying to read in the console window :).
            //Not enough data being logged? Change the app.config file's log4net log level from "INFO" TO "DEBUG"

            CrawlData.PrintDisclaimer();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
