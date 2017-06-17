using EloBuddy;
using EloBuddy.SDK;

namespace Twitchsharp.Addon
{
    public static class DamageHandler
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static float SD(Obj_AI_Base obj)
        {
            var index = Player.GetSpell(SpellSlot.E).Level - 1;
            float d = 0;
            if (SpellHandler.E.IsReady())
            {
                var Base = new float[] { 20, 35, 50, 65, 80 }[index];
                var PerStacks = new float[] { 15, 20, 25, 30, 35 }[index];
                var BonusAD = 0.25f * Twitch.FlatPhysicalDamageMod;
                var BonusAP = 0.20f * Twitch.TotalMagicalDamage;
                d = Base + ((PerStacks + BonusAD + BonusAP) * Brain.CountVenom(obj));
            }
            return Twitch.CalculateDamageOnUnit(obj, DamageType.Physical, d);
        }
    }
}
