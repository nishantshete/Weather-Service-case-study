using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using System.Configuration;
using log4net;

namespace WeatherService.Controllers
{
    public class WeatherServiceController : ApiController
    {
        //Created Ilog object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This Api gets called upon successful upload of File
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet]
       
       public async Task<string>GetWeatherData(string filename)
        {
            HttpClient httpclient = null;
            try
            {
                //call to validate file first
                ValidateFile(filename);
                //get the data from CSV filr into datatable
                DataTable dtData = GetDataFromCsvFile(filename);

                if (dtData != null && dtData.Rows.Count > 0)
                {
                    //Use http client to make a http request to open weather API
                    using (httpclient = new HttpClient())
                    {
                        
                        foreach (DataRow dr in dtData.Rows)
                        {
                            // make http call for each city id
                            using (Stream stream = await httpclient.GetStreamAsync($"{ConfigurationManager.AppSettings["WeatherApiUrl"].ToString()}?id={Convert.ToString(dr["City_ID"]).Trim()}&appid={ConfigurationManager.AppSettings["AppKey"].ToString()}&units={ConfigurationManager.AppSettings["unit"].ToString()}"))
                            {
                                using (StreamReader streamReader = new StreamReader(stream))
                                {
                                    //logic to save each city file with today's date and time into CSV file type 
                                    string city = Convert.ToString(dr["City_Name"].ToString()).Trim();

                                    WeatherEntities WE = JsonConvert.DeserializeObject<WeatherEntities>(streamReader.ReadToEnd());

                                    string saveFileName = WE.name + "_" + DateTime.Now.ToString("MMMM-dd-yyyy_Hmmssfff") + ".csv";

                                    using (var writer = new StreamWriter($@"{ConfigurationManager.AppSettings["Outfolder"].ToString()}\{saveFileName}"))
                                    {
                                        using (var csv = new CsvWriter(writer))
                                        {
                                            //saving only Main info like temp,pressure,humidty,temp_min,temp_max
                                            List<Main> lsMain = new List<Main>();
                                            lsMain.Add(WE.main);
                                            csv.WriteRecords(lsMain);
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new InvalidDataException(); 
                }
                
                return "File Processed!";
            }
            catch (CsvHelperException ex)
            {
                log.Error("Error ocurred in GetWeather Data Method", ex);
                throw new CsvHelperException(ex.ReadingContext,"file validation failed!! Invalid file");
            }
            catch(InvalidDataException ex)
            {
                log.Error("Error ocurred in GetWeather Data Method", ex);

                throw new InvalidDataException("File has no data");
            }
            catch(FileNotFoundException ex)
            {
                log.Error("Error ocurred in GetWeather Data Method", ex);

                throw new FileNotFoundException("File not found");
            }
            catch (Exception ex)
            {
                log.Error("Error ocurred in GetWeather Data Method", ex);
                
                throw new Exception("Error occurred while processing file");
            }
            
        }
        /// <summary>
        /// Method to fetch data from CSV
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DataTable GetDataFromCsvFile(string fileName)
        {
            try
            {
                DataTable dtData = new DataTable();
                using (var reader = new StreamReader($@"{ConfigurationManager.AppSettings["FileUploadFolder"].ToString()}\{fileName}"))
                {
                    using (var csv = new CsvReader(reader))
                    {
                        // Loading CSV file into Datatable.
                        using (var Cdr = new CsvDataReader(csv))
                        {
                            dtData.Load(Cdr);
                        }
                    }
                }
                return dtData;
            }

            catch(Exception ex)
            {
                log.Error("Error ocurred in GetDataFromCsvFile Method", ex);
                throw ex;
            }
        }
        private void ValidateFile(string FileName)
        {
            try
            {
                using (var reader = new StreamReader($@"{ ConfigurationManager.AppSettings["FileUploadFolder"].ToString()}/{FileName}"))
                {
                    using (var csv = new CsvReader(reader))
                    {
                        csv.Configuration.RegisterClassMap<CityDetailsMapper>();
                        //this line will throw error if file is not valid
                        csv.GetRecords<CityDetails>().ToList();
                    }
                }
            }
            catch (CsvHelperException ex)
            {
                log.Error("Error ocurred in ValidateFile  Method", ex);
                throw new CsvHelperException(ex.ReadingContext,"file validation failed!! Invalid file");
            }
            catch (FileNotFoundException ex)
            {
                log.Error("Error ocurred in ValidateFile Method", ex);

                throw new FileNotFoundException("File not found");
            }
            catch (Exception ex)
            {
                log.Error("Error ocurred in ValidateFile Method", ex);
                throw ex;
            }
        }
    }
}