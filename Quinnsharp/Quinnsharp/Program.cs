using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace Quinnsharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            Addon.API.IsSupportedHero(Champion.Quinn);
            Addon.API.Welcome("Quinn#");

            Addon.SpellHandler.Load();
            Addon.EventHandler.Load();
        }
    }
}