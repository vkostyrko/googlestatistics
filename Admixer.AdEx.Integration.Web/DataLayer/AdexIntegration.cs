using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.AdExchangeSeller.v1_1.Data;
using Google.Apis.AdExchangeSeller.v1_1;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Util.Store;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Admixer.AdEx.Integration.Web.Models;
using System.Globalization;

namespace Admixer.AdEx.Integration.Web.DataLayer
{
    class AdexIntegration
    {

        private List<string> Dimensions { get; set; }
        private List<string> Metrics { get; set; }
        public string StarDate { get; set; }
        public string EndDate { get; set; }

        private UserCredential OauthCredentials;
        public AdexIntegration()
        {
            using (var stream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "YourCredentials.json.json"), FileMode.Open, FileAccess.Read))
            {
                OauthCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, new[] { AdExchangeSellerService.Scope.AdexchangeSeller },
                "user", CancellationToken.None, new FileDataStore("AdexchangeSeller.Auth.Store")).Result;

            }
            Dimensions = new List<string>()
            {
            "DATE",
            "AD_TAG_CODE",
            "AD_TAG_NAME",
            "AD_UNIT_SIZE_CODE"
            };
            Metrics = new List<string>()
            {
                "MATCHED_AD_REQUESTS",
                "CLICKS",
                "MATCHED_AD_REQUESTS_CTR",
                "MATCHED_AD_REQUESTS_RPM",
                "EARNINGS",
                "AD_UNIT_SIZE_CODE",
                "AD_REQUESTS"
            };


        }
        public AdexIntegration(string jsonpath)
        {
            using (var stream = new FileStream(jsonpath, FileMode.Open, FileAccess.Read))
            {
                OauthCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                 GoogleClientSecrets.Load(stream).Secrets,
                 new[] { AdExchangeSellerService.Scope.AdexchangeSeller },
                  "user", CancellationToken.None, new FileDataStore("AdexchangeSeller.Auth.Store10")).Result;

            }


            Dimensions = new List<string>()
            {
            "DATE",
            "AD_TAG_CODE",
            "AD_TAG_NAME",
            "AD_UNIT_SIZE_CODE"
            };
            Metrics = new List<string>()
            {
                "MATCHED_AD_REQUESTS",
                "CLICKS",
                "MATCHED_AD_REQUESTS_CTR",
                "MATCHED_AD_REQUESTS_RPM",
                "EARNINGS",
                "AD_UNIT_SIZE_CODE",
                "AD_REQUESTS"
            };

        }
        public AdexIntegration(List<string> Dimensions, List<string> Metrics)
        {
            using (var stream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "YourCredentials.json.json"), FileMode.Open, FileAccess.Read))
            {
                OauthCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, new[] { AdExchangeSellerService.Scope.AdexchangeSeller },
        "user", CancellationToken.None, new FileDataStore("AdexchangeSeller.Auth.Store10")).Result;

            }
            this.Dimensions = Dimensions;
            this.Metrics = Metrics;
        }
        public AdexIntegration(string jsonpath, List<string> Dimensions, List<string> Metrics)
        {
            using (var stream = new FileStream(jsonpath, FileMode.Open, FileAccess.Read))
            {
                OauthCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync
                  (
                  GoogleClientSecrets.Load(stream).Secrets,
                  new[] { AdExchangeSellerService.Scope.AdexchangeSeller },
                  "user", CancellationToken.None, new FileDataStore("AdexchangeSeller.Auth.Store10")
                  ).Result;

            }
            this.Dimensions = Dimensions;
            this.Metrics = Metrics;

        }
        public List<AdExGoogleStats> GetAdxData()
        {
            List<AdExGoogleStats> stats = new List<AdExGoogleStats>();
            try
            {
                AdExchangeSellerService service_adex = new AdExchangeSellerService(new AdExchangeSellerService.Initializer()
                {
                    HttpClientInitializer = OauthCredentials,
                    ApplicationName = "Admixer Exchange",
                });
                ReportsResource.GenerateRequest gr = new ReportsResource.GenerateRequest(service_adex, StarDate, EndDate);
                gr.Metric = Metrics;
                gr.Dimension = Dimensions;
                gr.Sort = "DATE";
                gr.Locale = "en_US";
                Report report = gr.Execute();
                //stat = report.Rows.Cast<AdExGoogleStat>().ToList();
                foreach (var item in report.Rows)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(item.ElementAt(1))) continue;
                        AdExGoogleStats statobject = new AdExGoogleStats()
                        {
                            Date = DateTime.Parse(item.ElementAt(0)),
                            Id = Int64.Parse(item.ElementAt(1)),
                            TagName = item.ElementAt(2),
                            MatchedRequests = Int64.Parse(item.ElementAt(4)),
                            Clicks = Int32.Parse(item.ElementAt(5)),
                            CTR = decimal.Parse(item.ElementAt(6)),
                            eCPM = decimal.Parse(item.ElementAt(7)),
                            Revenue = decimal.Parse(item.ElementAt(8)),
                            Size = item.ElementAt(3),
                            Width = Int32.Parse(item.ElementAt(3).Substring(0, item.ElementAt(3).IndexOf('x'))),
                            Height = Int32.Parse(item.ElementAt(3).Substring(item.ElementAt(3).IndexOf('x') + 1, item.ElementAt(3).Length - (item.ElementAt(3).IndexOf('x') + 1))),
                            Requests = Int32.Parse(item.ElementAt(9))
                        };
                        stats.Add(statobject);
                    }
                    catch (Exception ex)
                    {
                        Logger.Write("ex_GetAdxData", ex.Message);
                    }
                }


            }

            catch (Exception ex)
            {
                Logger.Write("ex_GetAdxData", ex.Message);
                stats = null;
            }
            Logger.Write("GetAdxData", "Adx Data received");
            return stats;
        }
        
    }

}
