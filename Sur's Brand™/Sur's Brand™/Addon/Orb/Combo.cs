using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Sur_s_Brand_.Addon.Orb
{
    public class Combo
    {
        static bool IsBlazed(AIHeroClient random) => random.HasBuff("BrandAblaze");
        static AIHeroClient Brand => ObjectManager.Player;
        public static void Get()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null)
                return;
            if (Settings.usarE.CurrentValue)
            {
                if (Spells.E.IsReady())
                    if (target.IsValidTarget(Spells.E.Range)) Spells.E.Cast(target);
            }
            if (Settings.usarQ.CurrentValue)
            {
                if (target.IsValidTarget(Spells.Q.Range))
                    if (Spells.Q.IsReady())
                    {
                        var pred = Prediction.Position.PredictLinearMissile(target, Spells.Q.Range, Spells.Q.Radius, Spells.Q.CastDelay, Spells.Q.Speed, int.MaxValue, Brand.Position);
                        if (Settings.usarQ1.CurrentValue == 0)
                        {
                            if (Spells.Q.IsReady() && pred.HitChancePercent >= Settings.sliderA.CurrentValue)
                            {
                                Spells.Q.Cast(pred.CastPosition);
                            }
                        }
                        if (Settings.usarQ1.CurrentValue == 1)
                        {
                            if (IsBlazed(target) && pred.HitChancePercent >= Settings.sliderA.CurrentValue)
                            {
                                Spells.Q.Cast(pred.CastPosition);
                            }
                        }
                    }
            }
            if (Settings.usarW.CurrentValue)
            {
                if (target.IsValidTarget(Spells.W.Range))
                {
                    if (Spells.W.IsReady())
                    {
                        var wPred = Prediction.Position.PredictCircularMissile(target, Spells.W.Range, Spells.W.Radius, Spells.W.CastDelay, Spells.W.Speed, Brand.Position);
                        if (IsBlazed(target)
                            && (wPred.HitChancePercent >= Settings.sliderA.CurrentValue))
                        {
                            Spells.W.Cast(wPred.CastPosition);
                        }
                        else
                        {
                            if (!IsBlazed(target)
                                && (wPred.HitChancePercent >= Settings.sliderA.CurrentValue)) Spells.W.Cast(wPred.CastPosition);
                        }
                    }
                }
            }
            if (Settings.usarR.CurrentValue)
            {
                if (target.IsValidTarget(Spells.R.Range))
                {
                    if (Spells.R.IsReady()) //Ultimate Logic
                    {
                        if ((target.TotalShieldHealth() < Library.DamageBySlot(target, SpellSlot.R)) 
                            || target.HealthPercent <= 30 & target.CountEnemyMinionsInRange(Spells.R.Range) > 0)
                        {
                            Spells.R.Cast(target);
                        }
                        else
                        {
                            if (target.CountEnemiesInRange(Spells.R.Range) >= Settings.sliderR.CurrentValue)
                            {
                                Spells.R.Cast(target);
                            }
                        }
                    }
                }
            }
        }
    }
}
