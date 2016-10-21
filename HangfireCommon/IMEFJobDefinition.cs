using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireCommon
{
    public interface IMEFJobDefinition
    {
        void Execute(string jobId);
    }
    public interface IMEFJobDefinitionMetadata
    {
        string JobId { get; }
        string CronExpression { get; }
    }
}
