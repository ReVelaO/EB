using System;
using ChallengerRyze.Addon;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace ChallengerRyze
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Ryze) return;
            Spells.Get();
            Settings.Load();
            Events.Load();
        }
    }
}