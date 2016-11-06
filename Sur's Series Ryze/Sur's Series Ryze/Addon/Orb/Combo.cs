using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Pitufo.Addon.Orb
{
    internal class Combo
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        public static void Get()
        {
            var target = TargetSelector.GetTarget(1000, DamageType.Magical);
            if (target != null)
            {
                if (PiMenu.ComboMode == 0)
                {
                    if (target.IsInRange(Ryze, 615))
                    {
                        if (PiMenu.ComboQ)
                        {
                            var prediction = PiSkills.Q.GetPrediction(target);
                            if (PiSkills.Q.IsReady()
                                && target.IsValidTarget(1000)
                                && (prediction.HitChance >= Hitchanceq()))
                                PiSkills.Q.Cast(prediction.CastPosition);
                        }
                        if (PiMenu.ComboE)
                        {
                            if (PiSkills.E.IsReady()
                                && target.IsValidTarget(615))
                                PiSkills.E.Cast(target);
                        }
                        if (PiMenu.ComboQ)
                        {
                            var prediction = PiSkills.Q.GetPrediction(target);
                            if (PiSkills.Q.IsReady()
                                && target.IsValidTarget(1000)
                                && (prediction.HitChance >= Hitchanceq()))
                                PiSkills.Q.Cast(prediction.CastPosition);
                        }
                        if (PiMenu.ComboW)
                        {
                            if (PiSkills.W.IsReady()
                                && target.IsValidTarget(615))
                                PiSkills.W.Cast(target);
                        }
                    }
                    else if (target.IsInRange(Ryze, 1000))
                    {
                        if (PiMenu.ComboQ)
                        {
                            var prediction = PiSkills.Q.GetPrediction(target);
                            if (PiSkills.Q.IsReady()
                                && target.IsValidTarget(1000)
                                && (prediction.HitChance >= Hitchanceq()))
                                PiSkills.Q.Cast(prediction.CastPosition);
                        }
                        if (PiMenu.ComboW)
                        {
                            if (PiSkills.W.IsReady()
                                && target.IsValidTarget(615))
                                PiSkills.W.Cast(target);
                        }
                        if (PiMenu.ComboE)
                        {
                            if (PiSkills.E.IsReady()
                                && target.IsValidTarget(615))
                                PiSkills.E.Cast(target);
                        }
                    }
                }
                if (PiMenu.ComboMode == 1)
                {
                    if (target.IsValidTarget(1000) 
                        && target.Distance(Ryze) < 1000)
                    if (PiMenu.ComboQ)
                    {
                        if (PiSkills.Q.IsReady())
                        {
                            PiSkills.Q2.Cast(target);
                        }
                    }
                    if (PiMenu.ComboE)
                    {
                        if (!PiSkills.Q.IsReady() && PiSkills.E.IsReady())
                        {
                            PiSkills.E.Cast(target);
                        }
                    }
                    if (PiMenu.ComboQ)
                    {
                        if (PiSkills.Q.IsReady())
                        {
                            PiSkills.Q2.Cast(target);
                        }
                    }
                    if (PiMenu.ComboW)
                    {
                        if (!PiSkills.Q.IsReady() && !PiSkills.E.IsReady())
                        {
                            PiSkills.W.Cast(target);
                        }
                    }
                }
            }
        }
        private static HitChance Hitchanceq()
        {
            var value = PiMenu.QHitchance;
            if (value == 0)
                return HitChance.Low;
            if (value == 1)
                return HitChance.Medium;
            if (value == 2)
                return HitChance.High;
            return HitChance.Medium;
        }
    }
}
