using System;

namespace CopperMatchmaking.Utility
{
    internal static class Util
    {
        internal static void InitAnnouncer(string message, Action action)
        {
            Log.Info(message);
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}