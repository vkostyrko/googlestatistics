using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Admixer.AdEx.Integration.Web.Models
{
    public class AdExGoogleStats
    {
        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }
        [Key]
        [Column(Order = 2)]
        public long Id { get; set; }
        public string TagName { get; set; }
        public long? MatchedRequests { get; set; }
        public int? Clicks { get; set; }
        public decimal? CTR { get; set; }
        public decimal? eCPM { get; set; }
        public decimal? Revenue { get; set; }
        [Key]
        [Column(Order = 3)]
        public string Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Requests { get; set; }

    }
    public class AdExContext : DbContext
    {
        public AdExContext() : base(ConfigurationManager.ConnectionStrings["Local"].ConnectionString)
        {

        }
        public DbSet<AdExGoogleStats> AdExStats { get; set; }

    }
}