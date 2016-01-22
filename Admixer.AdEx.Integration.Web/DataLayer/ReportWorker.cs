using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Admixer.AdEx.Integration.Web.Models;

namespace Admixer.AdEx.Integration.Web.DataLayer
{
    //TODO: IReport,BaseReport(abstract), and childReports (inheritance)


    public class ReportFactory
    {
        private ReportType reportType;
        public ReportType ReportType
        {
            set
            {
                reportType = value;
            }
        }
        public ReportFactory()
        {

        }
        public ReportFactory(ReportType type)
        {
            this.reportType = type;
        }
        public IAdxReport GetReportFactory()
        {
            string connection = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
            switch (reportType)
            {
                case ReportType.MonthByDatesBySites:
                    return new MonthByDaysSitesReport(connection);

                case ReportType.ConcreteSiteMonthDate:
                    return new ConcreteSiteMonthDateReport(connection);

                case ReportType.MonthByDates:
                    return new MonthByDatesReport(connection);

                case ReportType.MonthByFormatsBySites:
                    return new MonthByFormatsBySitesReport(connection);

                case ReportType.ConcreteSiteByFormatsByMonth:
                    return new ConcreteSiteByFormatsByMonthReport(connection);

                default:
                    return new MonthByDaysSitesReport(connection);
            }
        }
    }
 }
