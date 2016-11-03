using EloBuddy;
using EloBuddy.SDK;

namespace Evelynn.Addon.Orb
{
    public class Combo
    {
        private static AIHeroClient Evelynn => ObjectManager.Player;

        public static void Get()
        {
            var target = TargetSelector.GetTarget(650, DamageType.Mixed);
            if ((target == null) || target.IsInvulnerable) return;
            if (target != null)
            {
                if (EveMenu.Cq.CurrentValue)
                    EveSpells.CastQ(target);
                if (EveMenu.Ce.CurrentValue)
                    EveSpells.CastE(target);
                if (EveMenu.Cw.CurrentValue)
                    EveSpells.CastW(target);
                if (EveMenu.Cr.CurrentValue)
                    if (target.IsInRange(Evelynn, EveSpells.R.Range))
                        if ((Evelynn.HealthPercent <= 79) && (Evelynn.HealthPercent < target.HealthPercent))
                            EveSpells.R.Cast(target.Position);
            }
        }
    }
}