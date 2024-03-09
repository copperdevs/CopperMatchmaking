using System;

namespace CopperMatchmaking.Components
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ServerOnlyComponentAttribute : Attribute
    {
        
    }
}