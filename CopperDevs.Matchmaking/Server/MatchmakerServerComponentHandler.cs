using System;
using System.Collections;
using System.Collections.Generic;
using CopperDevs.Matchmaking.Components;
using CopperDevs.Matchmaking.Data;

namespace CopperDevs.Matchmaking.Server
{
    public partial class MatchmakerServer : IEnumerable<BaseComponent>
    {
        private List<BaseComponent> components = new List<BaseComponent>();

        private void UpdateComponents()
        {
            foreach (var component in components) 
                component.Update();
        }
        
        /// <summary>
        /// Add a new component
        /// </summary>
        /// <param name="component">Component to add</param>
        public void Add(BaseComponent component)
        {
            var isClientOnly = Attribute.GetCustomAttribute(component.GetType(), typeof(ClientOnlyComponentAttribute)) != null;

            if (isClientOnly)
            {
                Log.Warning($"Component trying to be added to the server is client only.");
                return;
            }
            
            Log.Info($"Registering new server component of type {component.GetType().FullName}");

            components.Add(component);
            
            component.Server = this;
            component.Client = null;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the collection of components
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection of components.</returns>
        public IEnumerator<BaseComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}