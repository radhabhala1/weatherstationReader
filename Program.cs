using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using AzureInterface;
using System.Globalization;

namespace WeatherStationReader
{
    class Program
    {
        static string WUKey = "";
        const string RecordTable = "WeatherData";
        static void Main(string[] args)
        {
            try
            {
                string weatherRequest = CreateRequest("KCASANAR3.json");
                string response = MakeRequest(weatherRequest);
                WeatherData wd = ConvertJson(response);
                UploadToAzure(wd);
                Console.WriteLine("uploaded: " + wd.Time);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }

        }

        //Create the request URL
        public static string CreateRequest(string queryString)
        {
            string UrlRequest = "http://api.wunderground.com/api/" +
                                           WUKey +
                                           "/conditions/q/pws:" + queryString;
            return (UrlRequest);
        }

        public static String MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(requestUrl);
                WebResponse myResp = myReq.GetResponse();
                using (StreamReader reader = new StreamReader(myResp.GetResponseStream()))
                {
                    string jsonString = reader.ReadToEnd();
                    dynamic results = JsonConvert.DeserializeObject<dynamic>(jsonString);

                    return jsonString;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        public static WeatherData ConvertJson(string response)
        {
            WeatherData wd = new WeatherData();
            dynamic results = JsonConvert.DeserializeObject<dynamic>(response);
            wd.StationId = results.current_observation.station_id;
            CultureInfo provider = CultureInfo.InvariantCulture;
            wd.Time = DateTime.ParseExact(results.current_observation.observation_time_rfc822.ToString(), "ddd, dd MMM yyyy HH:mm:ss zzz", provider);
            wd.weather = results.current_observation.weather;
            wd.Temprature = results.current_observation.temp_f;
            wd.RelativeHumidity = results.current_observation.relative_humidity;
            wd.WindDirection = results.current_observation.wind_dir;
            wd.WindDegrees = results.current_observation.wind_degrees;
            wd.WindMPH = results.current_observation.wind_mph;
            wd.WindGustMPH = results.current_observation.wind_gust_mph;
            wd.Pressure = results.current_observation.pressure_mb;
            wd.PressureTrend = results.current_observation.pressure_trend;
            wd.DewPoint = results.current_observation.dewpoint_f;
            wd.HeatIndex = results.current_observation.heat_index_f;
            wd.FeelsLikeTemp = results.current_observation.feelslike_f;
            wd.VisibilityDistance = results.current_observation.visibility_mi;
            wd.SolarRadiation = results.current_observation.solarradiation;
            wd.UV = results.current_observation.UV;
            wd.PrecipitationInHr = results.current_observation.precip_1hr_in;
            wd.PrecipitationToday = results.current_observation.precip_today_in;
            wd.WindChill = results.current_observation.windchill_f;
            wd.SetTableKeys();
            return wd;

        }

        public static void UploadToAzure(WeatherData wd)
        {
            string SASKey = "";

            TableUploader<WeatherData> uploader = new TableUploader<WeatherData>(SASKey);
            uploader.Upload(wd);
        }
        
    }
}



