using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Notifications;
using Evelynn.Addon;

namespace Evelynn
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Evelynn)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#29EB8A'>Sur's Series:</font> <font color='#CC32EF'>Evelynn</font> successfully loaded. <font color='#FFFFFF'>Enjoy!</font>]");
            Notifications.Show(new SimpleNotification("Sur's Series: Evelynn", "Welcome back buddy!"), 20000);
            EveSpells.Load();
            EveMenu.Load();
            EveEvents.Load();
        }
    }
}