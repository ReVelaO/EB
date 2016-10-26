using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Katarina_.Addon.Orb
{
    public static class Combo
    {
        private static AIHeroClient Kata => ObjectManager.Player;
        public static void Get()
        {
            var enemy = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (enemy == null) { return; }

            if (enemy != null)
            {
                if (Settings.useQ
                    && enemy.IsValidTarget(Spells.Q.Range)
                    && Spells.Q.IsReady() && Events.ultimate == false) { Spells.Q.Cast(enemy); }
                if (Settings.useE
                    && enemy.IsValidTarget(Spells.E.Range)
                    && Spells.E.IsReady() && Events.ultimate == false) { Spells.E.Cast(enemy); }
                if (Settings.useW
                    && enemy.IsInRange(Kata, Spells.W.Range)
                    && Spells.W.IsReady() && Events.ultimate == false) { Spells.W.Cast(); }
                if (Settings.useR)
                {
                    if (Spells.R.IsReady())
                    {
                        if (enemy.TotalShieldHealth() < Library.DamageBySlot(enemy, SpellSlot.R)
                            && enemy.IsInRange(Kata, Spells.R.Range)) { Spells.R.Cast(); }
                        else if (Kata.CountEnemiesInRange(Spells.R.Range) > 1
                            && enemy.IsInRange(Kata, Spells.R.Range)) { Spells.R.Cast(); }
                    }
                }
            }
        }
    }
}
