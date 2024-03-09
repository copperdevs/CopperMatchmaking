using CopperMatchmaking.Client;
using CopperMatchmaking.Server;

namespace CopperMatchmaking.Components
{
    /// <summary>
    /// Base class for a component
    /// </summary>
    public class BaseComponent
    {
        /// <summary>
        /// Current instance of the server
        /// </summary>
        /// <remarks>
        /// Is null if the component is added to the client
        /// </remarks>
        protected internal MatchmakerServer? Server { get; internal set; }
        
        /// <summary>
        /// Current instance of the client
        /// </summary>
        /// <remarks>
        /// Is null if the component is added to the server
        /// </remarks>
        protected internal MatchmakerClient? Client { get; internal set; }

        /// <summary>
        /// Gives the state of if the component is added on the client
        /// </summary>
        public bool IsClientComponent => Client != null;
        
        /// <summary>
        /// Gives the state of if the component is added on the server
        /// </summary>
        public bool IsServerComponent => Server != null;
        
        /// <summary>
        /// Virtual function called when the component is added the server
        /// </summary>
        protected internal virtual void Start()
        {
        }
        
        /// <summary>
        /// Virtual function called when the server is stopped
        /// </summary>
        protected internal virtual void Stop()
        {
        }
        
        /// <summary>
        /// Virtual function called every time the server is updated
        /// </summary>
        protected internal virtual void Update()
        {
        }
    }
}