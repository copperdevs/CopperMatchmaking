using System;

namespace CopperDevs.Matchmaking.Components
{
    /// <summary>
    /// Apply to a <see cref="BaseComponent"/> to signify it should only work when added to a <see cref="CopperDevs.Matchmaking.Client.MatchmakerClient"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ClientOnlyComponentAttribute : Attribute
    {
    }
}