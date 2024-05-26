using System;

namespace CopperDevs.Matchmaking.Components
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ServerOnlyComponentAttribute : Attribute
    {
        
    }
}