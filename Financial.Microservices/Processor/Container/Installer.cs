using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Processor.Container
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(new Data.Loader.Stooq.Container.Installer());
            container.Register(
                Component.For<IRestClient>().UsingFactoryMethod(kernel => new RestClient("https://stooq.pl")).LifestyleScoped()
            );
            container.Register(
                Component.For<Func<string, Method, IRestRequest>>().UsingFactoryMethod(
                    kernel => new Func<string, Method, IRestRequest>(
                        (resource, method) => new RestRequest(resource, method)
                    )
                ).LifestyleScoped()
            );
            container.Register(
                Component.For<StooqSymbolLoader>().LifestyleScoped()
            );
            container.Register(
                Component.For<UnitOfWork>().UsingFactoryMethod(kernel => new UnitOfWork("stooq-database.db")).LifestyleScoped()
            );
            container.Register(
                Component.For<Func<IList<string>>>().UsingFactoryMethod(kernel =>
                {
                    var allSymbols = File.ReadAllLines("symbols.txt").Select(s => $"{s.Trim()}.US").Distinct().ToList();
                    return new Func<IList<string>>(() => allSymbols);
                }).LifestyleScoped()
            );
        }
    }
}
