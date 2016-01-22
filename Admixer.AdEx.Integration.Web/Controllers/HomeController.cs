using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Admixer.AdEx.Integration.Web.BusinessLayer;
using Admixer.AdEx.Integration.Web.Models;

namespace Admixer.AdEx.Integration.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "YourCredentials.json");
            DateTime LastDateUpdated = DateTime.Parse("2016-01-01");
            using (var ctx = new AdExContext())
            {
                var date= ctx.AdExStats.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault();
                if(date != default(DateTime))
                {
                    LastDateUpdated = date;
                }
            }
            //Get and update data from Google ADX
            IntegrationAPI api = new IntegrationAPI(config);
            var googledata=api.FetchAdexchangeData(LastDateUpdated.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
            api.UpdateData(googledata);
            //Will get from UI:    DateRange, TagID
            DateRange range = new DateRange();
            range.DateFrom = DateTime.Parse("2016-01-01");
            range.DateTo = DateTime.Parse("2016-01-12");
            long Tagid = 4642798920;
            api.GenerateReport(ReportType.ConcreteSiteByFormatsByMonth, range, Tagid);
            return View();
        }
    }
}