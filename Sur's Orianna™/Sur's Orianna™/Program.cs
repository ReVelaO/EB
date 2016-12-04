namespace Orianna
{
    using System;
    using EloBuddy;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Notifications;
    internal class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += olc;
        }
        static void olc(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Orianna)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Orianna™™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's Orianna™", "Welcome back buddy!"), 20000);

            Addon.MenuManager.Load();
            Addon.EventsManager.Load();
            Addon.BallManager.Load();
        }
    }
}
