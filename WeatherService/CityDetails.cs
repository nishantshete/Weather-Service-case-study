using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper;
using CsvHelper.Configuration;

namespace WeatherService
{
    public class CityDetails
    {
        public string City_Name { get; set; }
        public string City_ID { get; set; }

    }
    public class CityDetailsMapper: ClassMap<CityDetails>
    {
        public CityDetailsMapper()
        {
            Map(m => m.City_Name).Name("City_Name");
            Map(m => m.City_ID).Name("City_ID");
        }
    }

}