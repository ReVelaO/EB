using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerRyze.Addon.Orb
{
    public static class Laneclear
    {
        private static AIHeroClient Ryze => ObjectManager.Player;

        private static bool IsFluxed(Obj_AI_Base random) => random.HasBuff("RyzeE");

        public static void Get()
        {
            foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsEnemy
                                                                                                &&
                                                                                                (m.IsMinion |
                                                                                                 m.IsMonster)
                                                                                                && !m.IsDead)
                .OrderBy(e => e.Health))
            {
                var minionHealth = Prediction.Health.GetPrediction(minion, 250);
                if (minion.IsValidTarget(615, true, Ryze.Position))
                    if ((minionHealth < Library.DamageBySlot(minion, SpellSlot.E))
                        && (minion.CountEnemyMinionsInRange(300) > 1)) Spells.CastE(minion);
                    else if (IsFluxed(minion)) Spells.CastE(minion);
                    else if (Spells.Q.IsReady() 
                        && Orbwalker.IsAutoAttacking) { Spells.CastE(minion);}
                if (minion.IsInRange(Ryze, 1000)
                    && IsFluxed(minion)) Spells.CastQ(minion);
                if (minion.IsValidTarget(615, true, Ryze.Position))
                    if (minionHealth < Library.DamageBySlot(minion, SpellSlot.W)) Spells.CastW(minion);
            }
        }
    }
}