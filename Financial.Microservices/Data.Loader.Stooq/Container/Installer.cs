using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Data.Loader.Stooq.Container
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDataLoader<StooqData>>().ImplementedBy<StooqDataLoader>().LifestyleScoped()
            );
        }
    }
}
