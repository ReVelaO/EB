using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Notifications;

namespace Pitufo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Ryze)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#29EB8A'>Sur's Series:</font> <font color='#2981FD'>Ryze</font> successfully loaded. <font color='#FFFFFF'>Enjoy!</font>]");
            Notifications.Show(new SimpleNotification("Sur's Series: Ryze", "Welcome back buddy!"), 20000);
            Addon.PiSkills.Load();
            Addon.PiSums.Load();
            Addon.PiMenu.Load();
            Addon.PiEvents.Load();
        }
    }
}
