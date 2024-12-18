using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusEventListener.BgServcie.Infrastructure
{
    public class DependencyRegistration
    {
        internal static void ConfigureProductionDependencies(HostBuilderContext context, IServiceCollection collection)
        {
            throw new NotImplementedException();
        }

        internal static void ConfigureStaticDependencies(HostBuilderContext context, IServiceCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}
