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
            if (target == null) { return; }
            if (target != null)
            {
                if (target.IsInRange(Ryze, 1000))
                {
                    var targetPredHealth = Prediction.Health.GetPrediction(target, 250);
                    if (Settings.useQ
                        && Spells.IsQReady) { Spells.Q.Cast(target); }
                    if (Settings.forceQ)
                    {
                        if (target.IsValidTarget(615, true, Ryze.Position))
                        {
                            if (Settings.useW)
                            {
                                if (Spells.IsWReady
                                    && !Spells.IsQReady) { Spells.W.Cast(target); }
                                else if (Spells.IsWReady
                                    && targetPredHealth < Library.DamageBySlot(target, SpellSlot.W)) { Spells.W.Cast(target); }
                                else if (Spells.IsWReady) { Spells.W.Cast(target); }
                            }
                            if (Settings.useE)
                            {
                                if (Spells.IsEReady
                                    && !Spells.IsQReady) { Spells.E.Cast(target); }
                                else if (Spells.IsEReady
                                    && targetPredHealth < Library.DamageBySlot(target, SpellSlot.E)) { Spells.E.Cast(target); }
                                else if (Spells.IsEReady
                                    && !Spells.IsQReady
                                    && !Spells.IsWReady) { Spells.E.Cast(target); }
                            }
                        }
                    }
                    else if (!Settings.forceQ)
                    {
                        if (target.IsValidTarget(615, true, Ryze.Position))
                        {
                            if (Settings.useW 
                                && Spells.IsWReady) { Spells.W.Cast(target); }
                            if (Settings.useE 
                                && Spells.IsEReady) { Spells.E.Cast(target); }
                        }
                    }
                }
            }
        }
    }
}
