using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NEW_Mfile_Project.Models
{
    public class CreateLogFiles
    {
       
        protected static readonly ILog log = LogManager.GetLogger(typeof(CreateLogFiles));

        public static void ErrorLog(string sErrMsg)
        {
            string fileName = DateTime.Now.Date.ToShortDateString().Replace('/', '-');
            string path = @"C:\Try\logger\bin\Debug\";

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            path = path + fileName + ".txt";
            //Check if the file exists
            if (!File.Exists(path))
            {
                // Create the file and use streamWriter to write text to it.
                //If the file existence is not check, this will overwrite said file.
                //Use the using block so the file can close and vairable disposed correctly
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(DateTime.Now.ToString() + " " + sErrMsg);
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString() + " " + sErrMsg);
                }
            }
            
        }


        public static string Test_image = @"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";
    }
}