using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace Twitchsharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Twitch) return;

            Addon.SpellHandler.Load();
            Addon.MenuHandler.Load();
            //Addon.DamageIndicator.Load();
            Addon.EventHandler.Load();
        }
    }
}