using Heart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AITweaker.Services
{
    public class WeatherForecastService
    {
        public static string[] Summaries;

        private static HealthInfo[] Datas;

        public Task<HealthInfo[]> GetForecastAsync(DateTime startDate)
        {
            if (Datas == null)
            {
                Datas = HealthInfo.DeserialiseData(@"C:\Users\Frédéric\source\repos\Heart\Heart\Dataset\heart.csv", ';').ToArray();
                Summaries = Datas[0].Header().Split("\t");
            }

            return Task.FromResult(Datas);
        }
    }
}
