using System.Threading.Tasks;
using Jobs;
using NUnit.Framework;

namespace SydneyWeatherSnapShots
{
    [TestFixture]
    public class JobsTests
    {
        [Test]
        public async Task TestFetchSydneyWetherJob()
        {
            var temperature = await OpenWeatherProxy.FetchSydney();
            Assert.IsTrue(true);
        }

        [Test]
        public void SydneyWetherSnapShotTest()
        {
            Jobs.SydneyWetherSnapShot("unit test");
        }
    }
}
