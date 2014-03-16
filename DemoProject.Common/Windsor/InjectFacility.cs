using Castle.MicroKernel.Facilities;

namespace DemoProject.Common.Windsor
{
    /// <summary>
    /// This facility tells Castle Windsor to search for [Inject] attributes and perform injections.
    /// use "yourWindsorContainer.AddFacility(new InjectFacility());" to setup the injection
    /// </summary>
    public class InjectFacility : AbstractFacility
    {
        protected override void Init()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new InjectContributor());
        }
    }
}
