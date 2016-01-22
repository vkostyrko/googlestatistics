using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using Admixer.AdEx.Integration.Web.Models;


namespace Admixer.AdEx.Integration.Web.DataLayer
{
    public static class DataWorker
    {
        static public bool UpdateAdx(List<AdExGoogleStats> data)
        {
            try
            {
               if (data != null || data.Count !=0)
                {
                    using (var ctx = new AdExContext())
                    {
                        foreach (var item in data)
                        {
                            if (ctx.AdExStats.Any(x=>x.Id == item.Id && x.Date==item.Date && x.Size == item.Size))
                            {
                                ctx.Entry(item).State = EntityState.Modified;
                            }
                            else
                            {
                                ctx.AdExStats.Add(item);
                            }
                            ctx.SaveChanges();
                        }
                        Logger.Write("DataUpdated", "DataUpdated");
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Write("ex_DataUpdated", ex.Message);
                return false;
            }
            return false;
        }

    }
}
