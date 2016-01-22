using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admixer.AdEx.Integration.Web.DataLayer;
using Admixer.AdEx.Integration.Web.Models;


namespace Admixer.AdEx.Integration.Web.BusinessLayer
{
    class IntegrationAPI
    {
        AdexIntegration integration;
        ReportFactory reportfactory;
        public IntegrationAPI()
        {
            integration = new AdexIntegration();
        }
        public IntegrationAPI(string configpath)
        {
            integration = new AdexIntegration(configpath);
        }
        
        public List<AdExGoogleStats> FetchAdexchangeData(string DateFrom,string DateTo)
        {
            integration.StarDate = DateFrom;
            integration.EndDate = DateTo;
            return integration.GetAdxData();
        }
        public bool UpdateData(List<AdExGoogleStats> data)
        {
            bool IsUpdated;
            IsUpdated = DataWorker.UpdateAdx(data);
            return IsUpdated;
        }
        public List<AdexReportObject> GenerateReport(ReportType Type,DateRange range , long TagID)
        {   
            reportfactory = new ReportFactory(Type);
            var report = reportfactory.GetReportFactory();
            report.DateRange = range;
            if(TagID != 0)
            {
                report.TagId = TagID;
            }
           
            return report.Generate();
        }
        //TODO: write sheetData

    }
}
