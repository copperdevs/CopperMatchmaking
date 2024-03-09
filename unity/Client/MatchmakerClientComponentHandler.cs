using System;
using System.Collections;
using System.Collections.Generic;
using CopperMatchmaking.Components;
using CopperMatchmaking.Info;

namespace CopperMatchmaking.Client
{
    public partial class MatchmakerClient : IEnumerable<BaseComponent>
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
            var isServerOnly = Attribute.GetCustomAttribute(component.GetType(), typeof(ServerOnlyComponentAttribute)) != null;

            if (isServerOnly)
            {
                Log.Warning($"Component trying to be added to the client is server only.");
                return;
            }

            Log.Info($"Registering new client component of type {component.GetType().FullName}");
            
            components.Add(component);
            
            component.Server = null;
            component.Client = this;
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