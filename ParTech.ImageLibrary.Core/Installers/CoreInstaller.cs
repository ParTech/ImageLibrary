using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.Workers;

namespace ParTech.ImageLibrary.Core.Installers
{
    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<ImageRepository>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<ObjectRepository>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<OrderRepository>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<UserRepository>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<AccountsWorker>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<LuceneWorker>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient(),
                Castle.MicroKernel.Registration.Classes.FromThisAssembly()
                    .BasedOn<ShoppingCartWorker>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient()
            );

            container.AddFacility<LoggingFacility>(f => f.UseLog4Net());
        }
    }
}
