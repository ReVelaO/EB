﻿namespace Orianna.Addon.Orb
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu.Values;

    public static class Combo
    {
        public static void Get()
        {
            var t = TargetSelector.GetTarget(SpellManager.Q.Range + Player.Instance.BoundingRadius, DamageType.Magical);
            if (t != null && !t.HasUndyingBuff())
            {
                if (MenuManager.mcombo["q"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.Q.IsReady())
                    {
                        var p = SpellManager.Q.GetPrediction(t);
                        if (p != null && 
                            p.HitChancePercent > 60 || p.HitChance == HitChance.High)
                        {
                            SpellManager.Q.Cast(p.CastPosition);
                        }
                    }
                }
                if (MenuManager.mcombo["w"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.W.IsReady() && BallManager.IsInFloor)
                    {
                        if (BallManager.WBall.CountEnemyHeroesNear > 0)
                        {
                            SpellManager.W.Cast();
                        }
                    }
                }

                if (MenuManager.mcombo["r"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.R.IsReady() && BallManager.IsInFloor)
                    {
                        if (MenuManager.mcombo["re"].Cast<CheckBox>().CurrentValue)
                        {
                            var hp = DamageManager.HPrediction(t, BallManager.RBall.CastDelay);

                            if (BallManager.RBall.IsInBall(t)
                                && (DamageManager.R(t) + Player.Instance.GetAutoAttackDamage(t, true) > hp))
                            {
                                SpellManager.R.Cast();
                            }
                        }
                        else
                        {
                            if (BallManager.RBall.CountEnemyHeroesNear >= MenuManager.mcombo["minr"].Cast<Slider>().CurrentValue)
                            {
                                SpellManager.R.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}