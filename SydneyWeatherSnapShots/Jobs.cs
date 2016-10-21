using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobs;

namespace SydneyWeatherSnapShots
{
    public static class Jobs
    {
        public static void SydneyWetherSnapShot(string jobId)
        {
            var sydneyWeather = OpenWeatherProxy.FetchSydney()
                .GetAwaiter()
                .GetResult();

            const string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=WeatherDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                var temperature = sydneyWeather.main.temp;
                var reportTime = DateTimeOffset.FromUnixTimeSeconds(sydneyWeather.dt);
                cmd.CommandText = "insert into [dbo].[SydneyWeather] (JobId, Temperature, dt_reported_utc, dt_logged_utc) values(@JobId, @temperature, @reportTime, @loggingTime)";
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@temperature", temperature);
                cmd.Parameters.AddWithValue("@reportTime", reportTime);
                cmd.Parameters.AddWithValue("@JobId", jobId);
                cmd.Parameters.AddWithValue("@loggingTime", DateTime.UtcNow);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                cmd.ExecuteNonQuery();
            }
        }
    }
}
