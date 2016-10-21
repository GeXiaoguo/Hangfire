using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangfireCommon;

namespace PluginJobs
{
    [Export(typeof(HangfireCommon.IMEFJobDefinition))]
    [ExportMetadata("JobId", "MEFPluginJob_Minutely")]
    [ExportMetadata("CronExpression", "* * * * *")]
    public class SampleJob : IMEFJobDefinition
    {
        public void Execute(string jobId)
        {
            SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot(jobId);
        }
    }
}
