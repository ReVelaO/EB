namespace Orianna.Addon.Orb
{
    using EloBuddy;

    using EloBuddy.SDK;

    using EloBuddy.SDK.Enumerations;

    using EloBuddy.SDK.Menu.Values;

    internal class Combo
    {
        public static void Get()
        {
            if (MenuManager.mcombo["q"].Cast<CheckBox>().CurrentValue)
            {
                var t = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);

                if (t != null && t.IsValidTarget(SpellManager.Q.Range))
                {
                    if (SpellManager.Q.IsReady())
                    {
                        var p = SpellManager.Q.GetPrediction(t);
                        if (p.HitChance >= HitChance.High)
                        {
                            SpellManager.Q.Cast(p.CastPosition);
                        }
                    }
                }
            }
            if (MenuManager.mcombo["w"].Cast<CheckBox>().CurrentValue)
            {
                var t = TargetSelector.GetTarget(SpellManager.ActiveRange, DamageType.Magical);

                if (t != null && t.IsValidTarget(SpellManager.ActiveRange))
                {
                    if (SpellManager.W.IsReady())
                    {
                        if (BallManager.WBall.CountEnemyHeroesNear > 0)
                        {
                            SpellManager.W.Cast();
                        }
                    }
                }
            }
            if (MenuManager.mcombo["r"].Cast<CheckBox>().CurrentValue)
            {
                var t = TargetSelector.GetTarget(SpellManager.ActiveRange, DamageType.Magical);

                if (t != null && t.IsValidTarget(SpellManager.ActiveRange))
                {
                    if (SpellManager.R.IsReady())
                    {
                        if (BallManager.RBall.CountEnemyHeroesNear == 1
                            && DamageManager.HPrediction(t, BallManager.RBall.CastDelay) < DamageManager.R(t) && MenuManager.mcombo["re"].Cast<CheckBox>().CurrentValue)
                        {
                            SpellManager.R.Cast();
                        }
                        else if (BallManager.RBall.CountEnemyHeroesNear >= MenuManager.mcombo["minr"].Cast<Slider>().CurrentValue)
                        {
                            SpellManager.R.Cast();
                        }
                    }
                }
            }
        }
    }
}
