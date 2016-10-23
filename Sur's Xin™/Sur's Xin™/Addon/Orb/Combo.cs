using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon.Orb
{
    public class Combo
    {
        static AIHeroClient Xin => ObjectManager.Player;
        public static void Get()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Mixed);
            if (target == null) { return; }
            if (target.IsValidTarget(Spells.E.Range))
            {
                if (Settings.usarE.CurrentValue)
                {
                    if (Spells.E.IsReady() && target.IsInRange(Xin, Spells.E.Range))
                    {
                        Spells.E.Cast(target);
                    }
                }
                if (Settings.usarW.CurrentValue)
                {
                    if (Spells.W.IsReady() && target.IsInRange(Xin, Xin.GetAutoAttackRange()))
                    {
                        Spells.W.Cast();
                    }
                }
                if (Settings.usarR.CurrentValue)
                {
                    if (target.TotalShieldHealth() < Library.DamageBySlot(target, SpellSlot.R) 
                        && target.IsInRange(Xin, Spells.R.Range))
                    {
                        Spells.R.Cast();
                    }
                }
            }
        }
    }
}
