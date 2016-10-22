using System;
using EloBuddy.SDK.Events;
namespace Sur_s_Brand_
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        static void OnLoadingComplete(EventArgs args)
        {
            Addon.Hero.Get();
            Addon.Spells.Get();
            Addon.Settings.Load();
            Addon.Events.Load();
        }
    }
}
