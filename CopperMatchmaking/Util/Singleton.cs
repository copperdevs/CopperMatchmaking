using System;
using CopperMatchmaking.Info;

namespace CopperMatchmaking.Util
{
    /// <summary>
    /// singleton :(
    /// </summary>
    /// <typeparam name="T">Type of the singleton</typeparam>
    public class Singleton<T>  where T : class
    {
        /// <summary>
        /// Global instance of the singleton
        /// </summary>
        public static T Instance => GetInstance();
        private static T? instance;
    
        private static T GetInstance()
        {
            if (instance is null)
                throw new NullReferenceException($"{nameof(Singleton<T>)} couldn't find an instance.");
            
            return instance;
        }
        
        /// <summary>
        /// Update the instance of the singleton
        /// </summary>
        /// <param name="newInstance">New global instance of the singleton</param>
        protected void SetInstance(T newInstance)
        {
            if (instance != null)
            {
                Log.Error($"Instance of type {nameof(T)} is already set.");
                return;
            }
            instance = newInstance;
        }
    }
}