using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Linq;

namespace La_Leyenda_de_Caitlyn.Addon.Orb
{
    public static class Combo
    {
        private static AIHeroClient Caitlyn => Player.Instance;

        public static void Get()
        {
            if (Caitlyn.IsDead || Caitlyn.HasBuff("recall")) return;

            if (SpellHandler.Q.IsReady())
            {
                var t = TargetSelector.GetTarget(SpellHandler.Q.Range, DamageType.Physical);
                if (t.IsValidTarget())
                {
                    if (t.IsInMinimumRange(Brain.RealAARange, SpellHandler.Q.Range))
                    {
                        var p = SpellHandler.Q.GetPrediction(t);
                        if (p != null && p.HitChance > HitChance.Medium)
                        {
                            SpellHandler.Q.Cast(t);
                        }
                    }
                }
            }

            if (SpellHandler.W.IsReady())
            {
                var t = TargetSelector.GetTarget(SpellHandler.W.Range, DamageType.Magical);
                if (t.IsValidTarget())
                {
                    if (t.IsCCd())
                    {
                        Core.DelayAction(() => SpellHandler.W.Cast(t.ServerPosition), 700);
                    }
                    else
                    {
                        if (t.IsInMinimumRange(Brain.RealAARange, SpellHandler.W.Range))
                        {
                            var p = SpellHandler.W.GetPrediction(t);
                            if (p != null && p.HitChance > HitChance.Medium)
                            {
                                Core.DelayAction(() => SpellHandler.W.Cast(t), 700);
                            }
                        }

                        var enemies = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Caitlyn, 400)).FirstOrDefault();
                        if (enemies != null)
                        {
                            var p = SpellHandler.W.GetPrediction(enemies);
                            if (p != null && p.HitChance > HitChance.Medium)
                            {
                                Core.DelayAction(() => SpellHandler.W.Cast(enemies), 550);
                            }
                        }
                    }
                }
            }

            if (SpellHandler.E1.IsReady())
            {
                var enemy = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Caitlyn, 400)).OrderBy(o => Caitlyn.Distance(o)).FirstOrDefault();
                if (enemy != null)
                {
                    var p = SpellHandler.E1.GetPrediction(enemy);
                    if (p != null && p.HitChance > HitChance.Medium)
                    {
                        SpellHandler.E1.Cast(enemy);
                    }
                }
            }

            if (SpellHandler.R.IsReady())
            {
                var enemy = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Caitlyn, Brain.R.GetRange())).FirstOrDefault();
                if (enemy.IsValidTarget())
                {
                    if (enemy.IsInMinimumRange(Brain.RealAARange, Brain.R.GetRange()))
                    {
                        var hp = Prediction.Health.GetPrediction(enemy, Brain.RTime(enemy));
                        var p = SpellHandler.RP.GetPrediction(enemy);

                        if (p != null && !p.GetCollisionObjects<AIHeroClient>().Any(a => a != enemy))
                        {
                            if (DamageHandler.FinalShot(enemy) > hp)
                            {
                                SpellHandler.R.Cast(enemy);
                            }
                        }
                    }
                }
            }
        }
    }
}