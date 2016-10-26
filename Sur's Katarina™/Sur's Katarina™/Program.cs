using System;
using EloBuddy.SDK.Events;
using EloBuddy;
using EloBuddy.SDK.Notifications;

namespace Sur_s_Katarina_
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Katarina)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Katarina™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's Katarina™", "Welcome back buddy!"), 20000);
            Addon.Spells.Get();
            Addon.Settings.Load();
            Addon.Events.Load();
        }
    }
}
