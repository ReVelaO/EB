using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Linq;

namespace La_Leyenda_de_Caitlyn.Addon.Orb
{
    public static class Update
    {
        private static AIHeroClient Caitlyn => Player.Instance;

        public static void Get(EventArgs args)
        {
            if (Caitlyn.IsDead || Caitlyn.HasBuff("recall")) return;

            var enemy = EntityManager.Heroes.Enemies.Where(x => Caitlyn.GetAutoAttackDamage(x, true) > Prediction.Health.GetPrediction(x, (int)Caitlyn.AttackCastDelay)).FirstOrDefault(d => d.IsInRange(Caitlyn, Caitlyn.GetAutoAttackRange()));

            if (enemy.IsValidTarget())
            {
                if (Orbwalker.CanAutoAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackTo, enemy);
                }
            }

            if (SpellHandler.Q.IsReady())
            {
                var qenemy = EntityManager.Heroes.Enemies.Where(x => DamageHandler.PeaceMaker(x) > Prediction.Health.GetPrediction(x, SpellHandler.Q.Time(x))).FirstOrDefault(d => d.IsInRange(Caitlyn, SpellHandler.Q.Range));
                if (qenemy.IsValidTarget())
                {
                    if (qenemy.IsInMinimumRange(Brain.RealAARange, SpellHandler.Q.Range))
                    {
                        var p = SpellHandler.Q.GetPrediction(qenemy);
                        if (p != null && p.HitChance > HitChance.Medium)
                        {
                            SpellHandler.Q.Cast(qenemy);
                        }
                    }
                }
            }

            if (SpellHandler.R.IsReady())
            {
                var renemy = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Caitlyn, Brain.R.GetRange())).FirstOrDefault();
                if (renemy.IsValidTarget())
                {
                    if (renemy.IsInMinimumRange(Brain.RealAARange, Brain.R.GetRange()))
                    {
                        var hp = Prediction.Health.GetPrediction(renemy, Brain.RTime(renemy));
                        var p = SpellHandler.RP.GetPrediction(renemy);

                        if (p != null && !p.GetCollisionObjects<AIHeroClient>().Any(a => a != renemy))
                        {
                            if (DamageHandler.FinalShot(renemy) > hp)
                            {
                                SpellHandler.R.Cast(renemy);
                            }
                        }
                    }
                }
            }
        }
    }
}