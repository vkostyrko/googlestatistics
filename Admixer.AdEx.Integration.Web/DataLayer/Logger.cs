using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admixer.AdEx.Integration.Web.Models
{
    public class Logger
    {
        static void CreateFolder()
        {
            string subPath = "Logs"; // your code goes here

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + subPath))  // if it doesn't exist, create
                Directory.CreateDirectory(subPath);
        }

        public static void Write(string logname, string msg)
        {
            CreateFolder();
            string filename = string.Format(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\log_{1}_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"), logname);
            StreamWriter file = new StreamWriter(filename, true);
            file.WriteLine(msg + "     Time:" + DateTime.Now);
            file.Close();

        }
        public static void Write(string logname, Exception ex)
        {
            CreateFolder();
            string filename = string.Format(AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Ex_log_{1}_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd"), logname);
            StreamWriter file = new StreamWriter(filename, true);
            file.WriteLine(ex.Message + "     Time:" + DateTime.Now);
            file.Close();
        }
    }
}
