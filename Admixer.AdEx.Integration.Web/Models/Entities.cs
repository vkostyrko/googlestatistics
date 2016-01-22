using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admixer.AdEx.Integration.Web.Models
{
    public interface IAdxReport
    {
        void SaveToExcel(List<AdexReportObject> report);
        List<AdexReportObject> Generate();
        DateRange DateRange { get; set; }
        long TagId { get; set; }
    }
    public enum ReportType
    {
        MonthByDatesBySites = 1,
        ConcreteSiteMonthDate = 2,
        MonthByDates = 3,
        MonthByFormatsBySites = 4,
        ConcreteSiteByFormatsByMonth = 5


    }
    public class AdexReportObject
    {
        public DateTime Date { get; set; }

        public string Id { get; set; }
        public string TagName { get; set; }
        public long MatchedRequests { get; set; }
        public int Clicks { get; set; }
        public decimal CTR { get; set; }
        public decimal eCPM { get; set; }
        public decimal Revenue { get; set; }
        public decimal Coverage { get; set; }

        public string Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Requests { get; set; }

    }
    public class DateRange
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public DateTime Date { get; set; }

        public int Month { get; set; }

        public short Week { get; set; }

        public int Year { get; set; }

        public int RangeIndex { get; set; }

        public bool IsFixedPeriod
        {
            get
            {
                return RangeIndex > 0;
            }
        }
    }
  
}