using System.Web.Http.Controllers;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using DemoProject.API.Calculators;
using DemoProject.API.Configurations;
using DemoProject.API.Processors;
using DemoProject.API.Repositories;

namespace DemoProject.API.Windsor
{
    /// <summary>
    /// Castle Windsor installer.
    /// Configures associations between interfaces and concrete implementaions.
    /// </summary>
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Controllers
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .If(t => t.Name.EndsWith("Controller"))
                    .Configure(c => c.LifestyleTransient()));

            // Processors
            container.Register(
                Component.For<IFileProcessor>().ImplementedBy<LongestLineFileProcessor>().LifestyleTransient());

            // Metadata
            container.Register(
                Component.For<IMetadataRepository>().ImplementedBy<MetadataDbRepository>().LifestyleTransient());

            // Storage
            container.Register(
                Component.For<IStorageConfiguration>().ImplementedBy<StorageConfiguration>().LifestyleSingleton());

            container.Register(
                Component.For<IStorageRepository>().ImplementedBy<FileSystemStorageRepository>().LifestyleTransient());

            // Other
            container.Register(
                Component.For<IChecksumCalculator>().ImplementedBy<Md5ChecksumCalculator>().LifestyleTransient());
        }
    }
}