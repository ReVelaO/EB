using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Linq;

namespace Quinnsharp.Addon.Orb
{
    public static class Update
    {
        private static AIHeroClient Quinn => Player.Instance;

        public static void KillSteal()
        {
            if (Orbwalker.CanAutoAttack)
            {
                var enemy = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Quinn, Quinn.GetAutoAttackRange()) && x.HPrediction((int)Quinn.AttackCastDelay) < Quinn.GetAutoAttackDamage(x, true)).FirstOrDefault();
                if (enemy.IsHitable())
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, enemy);
                }
            }

            if (SpellHandler.Q.IsReady())
            {
                var enemy = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Quinn, SpellHandler.Q.Range) && x.HPrediction(SpellHandler.Q.Time(x)) < x.GetQDamage()).FirstOrDefault();
                if (enemy.IsHitable())
                {
                    var p = SpellHandler.Q.GetPrediction(enemy);

                    if (p != null && p.HitChance > HitChance.Medium)
                    {
                        SpellHandler.Q.Cast(enemy);
                    }
                }
            }

            if (SpellHandler.E.IsReady())
            {
                var enemy = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Quinn, SpellHandler.E.Range) && x.HPrediction(250) < x.GetEDamage()).FirstOrDefault();
                if (enemy != null
                    && !enemy.IsInvulnerable
                    && !enemy.IsDead && !Orbwalker.CanAutoAttack)
                {
                    SpellHandler.E.Cast(enemy);
                }
            }
        }
    }
}