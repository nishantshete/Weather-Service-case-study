using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WeatherService.Controllers
{
    public class FileUploadController : ApiController
    {
        //Created Ilog object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public string FileUpload()
        {
            try
            {
                //Save File to local folder
                var request = HttpContext.Current.Request;
                using (var fs = new System.IO.FileStream($@"{ ConfigurationManager.AppSettings["FileUploadFolder"].ToString()}/{request.Headers["filename"]}", System.IO.FileMode.Create))
                {
                    request.InputStream.CopyTo(fs);
                }

                //Validate file for columns
                
                return "uploaded";
            }
           
            catch (Exception ex)
            {
                log.Error("Error ocurred in FileUpload Method", ex);
                throw ex; 
            }
        }
       
        
    }
}