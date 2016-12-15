using EloBuddy;
using EloBuddy.SDK.Events;
using System;

namespace La_Leyenda_de_Caitlyn
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Caitlyn) return;

            Addon.SpellHandler.Load();
            Addon.EventHandler.Load();
        }
    }
}