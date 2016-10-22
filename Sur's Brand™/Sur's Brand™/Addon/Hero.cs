using EloBuddy;
using EloBuddy.SDK.Notifications;

namespace Sur_s_Brand_.Addon
{
    public class Hero
    {
        public static void Get()
        {
            if (Player.Instance.Hero != Champion.Brand)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's " + Player.Instance.ChampionName + "™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's " + Player.Instance.ChampionName + "™", "Welcome back buddy!"), 20000);
        }
    }
}
