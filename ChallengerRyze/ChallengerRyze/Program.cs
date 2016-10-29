using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace ChallengerRyze
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Ryze) { return; }
            Addon.Spells.Get();
            Addon.Settings.Load();
            Addon.Events.Load();
        }
    }
}
