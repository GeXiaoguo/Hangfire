using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Hangfire;
using HangfireCommon;
using Microsoft.Owin.Hosting;
using Owin;

using JsonConfig;

namespace HangfireClient
{
    class Program
    {
        static void StartHangfireHashboard()
        {
            const string dashBoardAddress = "http://FDAUSYD-FQ8G862:12345";
            try
            {
                using (WebApp.Start<Startup>(url: dashBoardAddress))
                {
                    Console.WriteLine($"Hangfire Client/Dashboard started. Dashboard at {dashBoardAddress}. Press any key to exit.");
                    Console.ReadKey();
                }
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException is HttpListenerException)
                {
                    //dashboard already started at the same address. quit silently.
                }
                else
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// read RecuringJob definitions from default.conf and add those jobs to Hangfire
        /// </summary>
        private static void AddExternalExeJobs()
        {
            foreach (dynamic job in Config.Default.HangfireJobs)
            {
                string jobId = job.JobId;
                string exeServerLocation = job.Execute;
                string arguments = job.Arguments;
                int exitCode = job.ExpectedExitCode;
                string cron = job.Cron;

                Console.WriteLine($"adding JobId:{jobId} ExeServerLocation:{exeServerLocation} Cron:{cron}");

                RecurringJob.AddOrUpdate(jobId,
                    () => ExternalExeJobAdapter.StartExecutable(exeServerLocation, arguments, exitCode),
                    (string)job.Cron);
            }
        }

        private static void AddPluginDllJobs()
        {
            var mefAdaptor = new MEFPluginJobAdaptor();
            foreach (var jobDefinition in mefAdaptor.JobDefinitions)
            {
                RecurringJob.AddOrUpdate(jobDefinition.Metadata.JobId, () => jobDefinition.Value.Execute(jobDefinition.Metadata.JobId), jobDefinition.Metadata.CronExpression);
            }
        }

        private static void AddDependentDllJobs()
        {
            RecurringJob.AddOrUpdate("DLL_Minutely", () => SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot("DLL_Minutely"), Cron.Minutely);
            RecurringJob.AddOrUpdate("DLL_Every10Minute_StartingAt4minute", () => SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot("DLL_Every10Minute_StartingAt4minute"), "4/10 * * * *");
            RecurringJob.AddOrUpdate("DLL_Midnight_of_ThursdayToSunday", () => SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot("DLL_Midnight_of_ThursdayToSunday"), "59 23 * * THU-SUN");
        }

        static void Main(string[] args)
        {
            const string connectionString =
    @"Data Source=localhost\sqlexpress;Initial Catalog=HangFireDB;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

            //directly reference job definitions and add them
            AddDependentDllJobs();

            //define external standalone jobs
            AddExternalExeJobs();

            //define plug-in jobs
            AddPluginDllJobs();

            StartHangfireHashboard();
        }
    }
}
