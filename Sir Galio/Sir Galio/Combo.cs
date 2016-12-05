namespace Galio
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using System.Linq;
    using static Menus;
    internal class Combo
    {
        public static void ComboExecute()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if ((target == null) || target.IsInvulnerable) return;

            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
                {
                    if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady())
                    {
                        var qhitchance = Spells.Q.GetPrediction(target);
                        if (qhitchance.HitChancePercent >= ComboMenu["qhit"].Cast<Slider>().CurrentValue)
                        {
                            Spells.Q.Cast(qhitchance.CastPosition);
                        }
                    }
                }
            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Spells.E.Range) && Spells.E.IsReady())
                {
                    var ehitchance = Spells.E.GetPrediction(target);
                    if (ehitchance.HitChancePercent >= ComboMenu["ehit"].Cast<Slider>().CurrentValue)
                    {
                        Spells.E.Cast(ehitchance.CastPosition);
                    }
                }
            }

            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                var enemy = EntityManager.Heroes.Enemies.FirstOrDefault(x => x.IsValidTarget(Spells.R.Range) && x.IsValid);

                if (enemy.IsValidTarget(Spells.R.Range) && Spells.R.IsReady()
                    && Player.Instance.CountEnemiesInRange(600) >= ComboMenu["REnemies"].Cast<Slider>().CurrentValue)
                {
                        Spells.R.Cast();
                }
            }

            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
            {
                // currently in development

                //Protect Me
                if (Spells.W.IsReady() && Player.Instance.IsInDanger(80, 300))
                {
                    Spells.W.Cast(Player.Instance);
                }
            }
        }
    }
}
