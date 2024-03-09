using System;
using System.Collections.Generic;
using System.Linq;
using CopperMatchmaking.Data;
using CopperMatchmaking.Info;

namespace CopperMatchmaking.Components
{
    /// <summary>
    /// Component for removing lobbies after a certain period of time
    /// </summary>
    [ServerOnlyComponent]
    public class LobbyTimeoutComponent : BaseComponent
    {
        private Dictionary<uint, CreatedLobby> Lobbies => Server?.LobbyManager.lobbies!;
        
        /// <summary>
        /// Time in seconds that the host of a lobby has to send the join code for said lobby 
        /// </summary>
        public float LobbyTimeoutTime = 5;
        
        /// <summary>
        /// Update the component
        /// </summary>
        protected internal override void Update()
        {
            foreach (var lobby in Lobbies.Values.ToList().Where(lobby => (DateTime.Now - lobby.LobbyCreationTime).Seconds >= LobbyTimeoutTime))
            {
                Log.Info($"The host of lobby {lobby.LobbyId} has taken too long to send the join code. Timing out the lobby.");
                Lobbies.Remove(lobby.LobbyId);
                
                Server?.QueueManager.ReturnLobby(lobby);
            }
        }
    }
}