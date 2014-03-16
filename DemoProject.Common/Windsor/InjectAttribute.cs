using System;

namespace DemoProject.Common.Windsor
{
    /// <summary>
    /// User this attribute on properties that should be automatically injected by Castle Windsor.
    /// NOTE: Castle should be initialized with InjectFacility in order for this to work.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}
