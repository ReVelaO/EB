using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Sur_s_Annie_
{
    static class Program
    {
        private static AIHeroClient me => ObjectManager.Player;
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        private static void OnLoadingComplete(EventArgs args)
        {
            if (me.Hero != Champion.Annie)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + me.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Annie™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Manager.LoadSkills();
            Manager.LoadMenu();
            Manager.LoadEvents();
        }
    }
}
