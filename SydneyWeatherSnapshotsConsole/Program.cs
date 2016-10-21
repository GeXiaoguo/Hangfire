namespace SydneyWeatherSnapshotsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string jobId = args.Length >= 1 ? args[0] : "from_console";
            SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot(jobId);
        }
    }
}
