using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Amber.Flywheel;
using Amber.Protobuf;

namespace WeatherStationReader
{
    public class WeatherData : TableEntity
    {
        public string StationId { get; set; }
        public DateTime Time { get; set; }
        public string weather { get; set; }
        public double Temprature { get; set; }
        public string RelativeHumidity { get; set; }
        public string WindDirection { get; set; }
        public string WindDegrees { get; set; }
        public string WindMPH { get; set; }
        public string WindGustMPH { get; set; }
        public string Pressure { get; set; }
        public string PressureTrend { get; set; }
        public string DewPoint { get; set; }
        public string HeatIndex { get; set; }
        public string FeelsLikeTemp { get; set; }
        public string VisibilityDistance { get; set; }
        public string SolarRadiation { get; set; }
        public string UV { get; set; }
        public string PrecipitationInHr { get; set; }
        public string PrecipitationToday { get; set; }
        public string WindChill { get; set; }

        public WeatherData()
        {
            PartitionKey = "";
            RowKey = "";
        }
        public void SetTableKeys()
        {
            PartitionKey = this.StationId;
            RowKey = ShimUtils.ConvertDateTimeToRowKey(this.Time);
        }
    }


}
