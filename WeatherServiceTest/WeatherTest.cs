using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Controllers;
using System.IO;
namespace WeatherServiceTest
{
    [TestClass]
   public class WeatherTest
    {

        [TestMethod]
        //test to validate positive test considering file is at the location
        public void GetWeatherDataTestPositive()
        {
            WeatherServiceController WSController = new WeatherServiceController();
            string result =  WSController.GetWeatherData("citylist.csv").Result;
            Assert.AreEqual("File Processed!", result);
            
        }


        [TestMethod]
          ///catching filenotfound exception when file is not there
        public void GetWeatherDataTestFileNotFound()
        {
            try
            {
                WeatherServiceController WSController = new WeatherServiceController();
                string result = WSController.GetWeatherData("citylist1sv.csv").Result;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(FileNotFoundException), ex.InnerException.GetType());
            }
        }
        [TestMethod]
        ///catching filevalidation  exception when invalid file is passed
        public void GetWeatherDataTestfilevalidationException()
        {
            try
            {
                WeatherServiceController WSController = new WeatherServiceController();
                string result = WSController.GetWeatherData("myapp - Copy.csv").Result;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(CsvHelper.CsvHelperException), ex.InnerException.GetType());
            }
        }
        [TestMethod]
        ///catching filevalidation  exception when invalid file is passed
        public void GetWeatherDataTestFileEmptyException()
        {
            try
            {
                WeatherServiceController WSController = new WeatherServiceController();
                string result = WSController.GetWeatherData("citylist - Copy.csv").Result;
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(InvalidDataException), ex.InnerException.GetType());
            }
        }
    }
}
