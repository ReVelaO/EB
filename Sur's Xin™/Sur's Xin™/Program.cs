using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Notifications;
using Sur_s_Xin_.Addon;

namespace Sur_s_Xin_
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.XinZhao)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Xin™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's Xin™", "Welcome back buddy!"), 20000);
            Spells.Get();
            Settings.Load();
            Events.Load();
        }
    }
}