using System;
using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerRyze.Addon.Orb
{
    public static class Combo
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        public static void Get()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Magical);
            if (target != null)
            {
                if (target.IsInRange(Ryze, 615))
                {
                    if (Settings.UseE)
                    Spells.CastE(target);
                    if (Settings.UseQ)
                    Spells.CastQ(target);
                    if (Settings.UseW 
                        && !Spells.Q.IsReady() 
                        && !Spells.E.IsReady())
                    Spells.CastW(target);
                }
                else if (target.IsInRange(Ryze, 1000))
                {
                    if (Settings.UseQ)
                    Spells.CastQ(target);
                    if (Settings.UseW && !Spells.Q.IsReady())
                    Spells.CastW(target);
                    if (Settings.UseE)
                    Spells.CastE(target);
                }
            }
        }
    }
}