using System;
using Hangfire;

namespace HangfireServer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(System.Action));

            const string connectionString = @"Data Source=localhost\sqlexpress;Initial Catalog=HangFireDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

            using (var server = new BackgroundJobServer())
            {
                log.Info("Hangfire Server started. Started processing jobs. Press any key to exit...");

                Console.ReadKey();
            }
        }
    }
}
