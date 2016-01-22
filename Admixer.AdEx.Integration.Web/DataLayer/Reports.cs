using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Admixer.AdEx.Integration.Web.Models;
using System.Data;


namespace Admixer.AdEx.Integration.Web.DataLayer
{
    //TODO: LOgger
    public class MonthByDaysSitesReport : IAdxReport
    {
        protected string connection;
        public DateRange DateRange { get; set; }
        public long TagId { get; set; }
        public MonthByDaysSitesReport(string connection)
        {
            this.connection = connection;

        }
       
        public List<AdexReportObject> Generate()
        {
            string command = @"select TagName,cast(Date as date),sum(Requests) as Requests,sum(MatchedRequests) as MatchReq,
                              cast((cast(sum(MatchedRequests) as float)/CAST(sum(Requests) as float)*100) as decimal(18,2)) as Coverage,    
                                sum(Clicks) as Click,cast((CAST(sum(Clicks) as float)/sum(MatchedRequests) *100) as decimal(18,2)) as CTR,
                                sum(eCPM) as eCPM,sum(Revenue) as Revenue from AdExGoogleStats
                                where Date>=@DateFrom and Date<=@DateTo
                                group by TagName,Date
                                order by Tagname";
            List<AdexReportObject> adxreports = new List<AdexReportObject>();
            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateRange.DateFrom;
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateRange.DateTo;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdexReportObject report = new AdexReportObject()
                        {
                            TagName = rdr.GetString(0),
                            Date = rdr.GetDateTime(1),
                            Requests = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2),
                            MatchedRequests = rdr.IsDBNull(3) ? 0 : rdr.GetInt64(3),
                            Coverage = rdr.IsDBNull(4) ? 0 : rdr.GetDecimal(4),
                            Clicks = rdr.IsDBNull(5) ? 0 : rdr.GetInt32(5),
                            CTR = rdr.IsDBNull(6) ? 0 : rdr.GetDecimal(6),
                            eCPM = rdr.IsDBNull(7) ? 0 : rdr.GetDecimal(7),
                            Revenue = rdr.IsDBNull(8) ? 0 : rdr.GetDecimal(8)
                        };
                        adxreports.Add(report);
                    }
                    if (rdr.IsClosed)
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
            return adxreports;
        }

        public  void SaveToExcel(List<AdexReportObject> report)
        {
            throw new NotImplementedException();
        }
    }
    public class ConcreteSiteMonthDateReport : MonthByDaysSitesReport, IAdxReport
    {
        //public DateRange DateRange { get; set; }
        //public long TagId { get; set; }
        public ConcreteSiteMonthDateReport(string connection) : base(connection)
        {

        }
        private string command = @"select cast(Date as date),sum(Requests) as Requests,sum(MatchedRequests) as MatchReq,
                              cast((cast(sum(MatchedRequests) as float)/CAST(sum(Requests) as float)*100) as decimal(18,2)) as Coverage,    
                                sum(Clicks) as Click,cast((CAST(sum(Clicks) as float)/sum(MatchedRequests) *100) as decimal(18,2)) as CTR,
                                sum(eCPM) as eCPM,sum(Revenue) as Revenue from AdExGoogleStats
                                where Date>=@DateFrom and Date<=@DateTo and Id=@TagId
                                group by TagName,date
								order by TagName";
        public new List<AdexReportObject> Generate()
        {
            List<AdexReportObject> adxreports = new List<AdexReportObject>();

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateRange.DateFrom;
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateRange.DateTo;
                    cmd.Parameters.Add("@TagId", SqlDbType.BigInt).Value = TagId;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdexReportObject report = new AdexReportObject()
                        {

                            Date = rdr.GetDateTime(0),
                            Requests = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                            MatchedRequests = rdr.IsDBNull(2) ? 0 : rdr.GetInt64(2),
                            Coverage = rdr.IsDBNull(3) ? 0 : rdr.GetDecimal(3),
                            Clicks = rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4),
                            CTR = rdr.IsDBNull(5) ? 0 : rdr.GetDecimal(5),
                            eCPM = rdr.IsDBNull(6) ? 0 : rdr.GetDecimal(6),
                            Revenue = rdr.IsDBNull(7) ? 0 : rdr.GetDecimal(7)
                        };
                        adxreports.Add(report);
                    }
                    if (rdr.IsClosed)
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
            return adxreports;
        }

    }
    public class MonthByDatesReport : MonthByDaysSitesReport, IAdxReport
    {
        //public DateRange DateRange { get; set; }
        //public long TagId { get; set; }

        public MonthByDatesReport(string connection) : base(connection)
        {

        }
        private string command = @"select cast(Date as date),sum(Requests) as Requests,sum(MatchedRequests) as MatchReq,
                              cast((cast(sum(MatchedRequests) as float)/CAST(sum(Requests) as float)*100) as decimal(18,2)) as Coverage,    
                                sum(Clicks) as Click,cast((CAST(sum(Clicks) as float)/sum(MatchedRequests) *100) as decimal(18,2)) as CTR,
                                sum(eCPM) as eCPM,sum(Revenue) as Revenue from AdExGoogleStats
                                where Date>=@DateFrom and Date<=@DateTo
                                group by Date
                                order by Date";
        public new List<AdexReportObject> Generate()
        {
            List<AdexReportObject> adxreports = new List<AdexReportObject>();

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateRange.DateFrom;
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateRange.DateTo;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdexReportObject report = new AdexReportObject()
                        {

                            Date = rdr.GetDateTime(0),
                            Requests = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                            MatchedRequests = rdr.IsDBNull(2) ? 0 : rdr.GetInt64(2),
                            Coverage = rdr.IsDBNull(3) ? 0 : rdr.GetDecimal(3),
                            Clicks = rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4),
                            CTR = rdr.IsDBNull(5) ? 0 : rdr.GetDecimal(5),
                            eCPM = rdr.IsDBNull(6) ? 0 : rdr.GetDecimal(6),
                            Revenue = rdr.IsDBNull(7) ? 0 : rdr.GetDecimal(7)
                        };
                        adxreports.Add(report);
                    }
                    if (rdr.IsClosed)
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
            return adxreports;
        }

    }


    public class MonthByFormatsBySitesReport : MonthByDaysSitesReport, IAdxReport
    {
        //public DateRange DateRange { get; set; }
        //public long TagId { get; set; }
        private string command = @"select TagName,Size,sum(Requests) as Requests,sum(MatchedRequests) as MatchReq,
                              cast((cast(sum(MatchedRequests) as float)/CAST(sum(Requests) as float)*100) as decimal(18,2)) as Coverage,    
                                sum(Clicks) as Click,cast((CAST(sum(Clicks) as float)/sum(MatchedRequests) *100) as decimal(18,2)) as CTR,
                                sum(eCPM) as eCPM,sum(Revenue) as Revenue from AdExGoogleStats
                                where Date>=@DateFrom and Date<=@DateTo
                                group by TagName,Size
                                order by Tagname,Size";
        public MonthByFormatsBySitesReport(string connection) : base(connection)
        {

        }
        public new List<AdexReportObject> Generate()
        {
            List<AdexReportObject> adxreports = new List<AdexReportObject>();

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateRange.DateFrom;
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateRange.DateTo;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdexReportObject report = new AdexReportObject()
                        {

                            TagName = rdr.GetString(0),
                            Size = rdr.GetString(1),
                            Requests = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2),
                            MatchedRequests = rdr.IsDBNull(3) ? 0 : rdr.GetInt64(3),
                            Coverage = rdr.IsDBNull(4) ? 0 : rdr.GetDecimal(4),
                            Clicks = rdr.IsDBNull(5) ? 0 : rdr.GetInt32(5),
                            CTR = rdr.IsDBNull(6) ? 0 : rdr.GetDecimal(6),
                            eCPM = rdr.IsDBNull(7) ? 0 : rdr.GetDecimal(7),
                            Revenue = rdr.IsDBNull(8) ? 0 : rdr.GetDecimal(8)
                        };
                        adxreports.Add(report);
                    }
                    if (rdr.IsClosed)
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
            return adxreports;
        }

    }
    public class ConcreteSiteByFormatsByMonthReport : MonthByDaysSitesReport, IAdxReport
    {
        //public DateRange DateRange { get; set; }
        //public long TagId { get; set; }
        public ConcreteSiteByFormatsByMonthReport(string connection) : base(connection)
        {

        }
        private string command = @"select Size,sum(Requests) as Requests,sum(MatchedRequests) as MatchReq,
                              cast((cast(sum(MatchedRequests) as float)/CAST(sum(Requests) as float)*100) as decimal(18,2)) as Coverage,    
                                sum(Clicks) as Click,cast((CAST(sum(Clicks) as float)/sum(MatchedRequests) *100) as decimal(18,2)) as CTR,
                                sum(eCPM) as eCPM,sum(Revenue) as Revenue from AdExGoogleStats
                                where Date>=@DateFrom and Date<=@DateTo and id =@TagId
                                group by Size
                                order by Size";
        public new List<AdexReportObject> Generate()
        {
            List<AdexReportObject> adxreports = new List<AdexReportObject>();

            using (var conn = new SqlConnection(connection))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = DateRange.DateFrom;
                    cmd.Parameters.Add("@DateTo", SqlDbType.Date).Value = DateRange.DateTo;
                    cmd.Parameters.Add("@TagId", SqlDbType.BigInt).Value = TagId;
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AdexReportObject report = new AdexReportObject()
                        {

                            
                            Size = rdr.GetString(0),
                            Requests = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1),
                            MatchedRequests = rdr.IsDBNull(2) ? 0 : rdr.GetInt64(2),
                            Coverage = rdr.IsDBNull(3) ? 0 : rdr.GetDecimal(3),
                            Clicks = rdr.IsDBNull(4) ? 0 : rdr.GetInt32(4),
                            CTR = rdr.IsDBNull(5) ? 0 : rdr.GetDecimal(5),
                            eCPM = rdr.IsDBNull(6) ? 0 : rdr.GetDecimal(6),
                            Revenue = rdr.IsDBNull(7) ? 0 : rdr.GetDecimal(7)
                        };
                        adxreports.Add(report);
                    }
                    if (rdr.IsClosed)
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
            return adxreports;
        }

    }
}