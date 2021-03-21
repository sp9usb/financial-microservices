using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using System;

namespace Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(new Container.Installer());

            using (var scope = container.BeginScope())
            {
                var loader = container.Resolve<StooqSymbolLoader>();
                loader.StoreAllSymbolsOnDatabase().Wait();
            }
        }
    }
}
