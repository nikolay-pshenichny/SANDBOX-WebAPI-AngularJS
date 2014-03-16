using System.Linq;

using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;

namespace DemoProject.Common.Windsor
{
    /// <summary>
    /// Castle Windsor's custom contributor for Properties Injection
    /// </summary>
    internal class InjectContributor : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            var properties = model.Properties.Where(x => x.Property.IsDefined(typeof(InjectAttribute), true)).ToList();

            properties.ForEach(x => { x.Dependency.IsOptional = false; });
        }
    }
}
