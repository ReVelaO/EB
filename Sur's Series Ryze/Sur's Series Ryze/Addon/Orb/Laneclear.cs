using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Pitufo.Addon.Orb
{
    internal class Laneclear
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
                //Set E
                if (minion.IsValidTarget(615, true, Ryze.Position))
                {
                    if ((minionHealth < PiDamages.DamageBySlot(minion, SpellSlot.E))
                        && (minion.CountEnemyMinionsInRange(300) > 1)) PiSkills.E.Cast(minion);
                    else if (IsFluxed(minion)) PiSkills.E.Cast(minion);
                    else if (PiSkills.Q.IsReady()
                             && (PiDamages.DamageBySlot(minion, SpellSlot.E) + PiDamages.DamageBySlot(minion, SpellSlot.Q) > minion.Health)) PiSkills.E.Cast(minion);
                }
                //Set Q
                if (minion.IsInRange(Ryze, 1000)
                    && IsFluxed(minion))
                {
                    if (PiSkills.Q.IsReady()
                        && minion.IsValidTarget(1000))
                        PiSkills.Q.Cast(minion);
                }
                //Set W
                if (minion.IsValidTarget(615, true, Ryze.Position))
                    if (minionHealth < PiDamages.DamageBySlot(minion, SpellSlot.W))
                    {
                        if (PiSkills.W.IsReady()
                                && minion.IsValidTarget(615))
                            PiSkills.W.Cast(minion);
                    }
            }
        }
    }
}
