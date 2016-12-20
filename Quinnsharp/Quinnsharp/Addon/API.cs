using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Notifications;

namespace Quinnsharp.Addon
{
    public static class API
    {
        private static AIHeroClient myHero => Player.Instance;

        public static bool IsSupportedHero(Champion heroSupported)
        {
            if (myHero.Hero != heroSupported)
            {
                return false;
            }

            return true;
        }

        public static void Welcome(string addonname, string notifimessage = null)
        {
            Chat.Print("<font color='#FFFFFF'>[" + addonname + " loaded. Enjoy!</font>");
            Notifications.Show(new SimpleNotification(addonname, notifimessage == null ? "Welcome Back Buddy!" : notifimessage), 20000);
        }

        public static bool IsHitable(this Obj_AI_Base obj)
        {
            return obj != null && !obj.IsInvulnerable && !obj.IsDead;
        }

        public static float HPrediction(this Obj_AI_Base obj, int time)
        {
            return Prediction.Health.GetPrediction(obj, time);
        }

        public static int Time(this Spell.Skillshot spell, Obj_AI_Base target)
        {
            return (int)((myHero.Distance(target.ServerPosition, false) / spell.Speed) * 1000) + spell.CastDelay;
        }
    }
}