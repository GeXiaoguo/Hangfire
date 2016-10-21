using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireCommon
{
    public class MEFPluginJobAdaptor
    {
        [ImportMany(typeof(IMEFJobDefinition))]
        public IEnumerable<Lazy<IMEFJobDefinition, IMEFJobDefinitionMetadata>> JobDefinitions;

        public MEFPluginJobAdaptor()
        {
            CompositionContainer _container;
            var aggregateCatalog = new AggregateCatalog();
            aggregateCatalog.Catalogs.Add(new DirectoryCatalog(@"C:\TestProjects\Hangfire\HangfireClient\bin\plugins"));
            _container = new CompositionContainer(aggregateCatalog);

            //resolving Import points for this
            _container.ComposeParts(this);
        }
    }
}
