using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Pitufo.Addon.Orb
{
    internal class Laneclear
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        private static bool IsFluxed(Obj_AI_Base random) => random.HasBuff("RyzeE");
        private static readonly Random random = new Random();
        public static void Get()
        {
            if (PiMenu.LaneMode == 0)
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
                    if (PiMenu.LaneE)
                    if (minion.IsValidTarget(615, true, Ryze.Position))
                    {
                        if ((minionHealth < PiDamages.DamageBySlot(minion, SpellSlot.E))
                            && (minion.CountEnemyMinionsInRange(300) > 1)) PiSkills.E.Cast(minion);
                        else if (IsFluxed(minion)) PiSkills.E.Cast(minion);
                        else if (PiSkills.Q.IsReady()
                                 &&
                                 (PiDamages.DamageBySlot(minion, SpellSlot.E) +
                                  PiDamages.DamageBySlot(minion, SpellSlot.Q) > minion.Health)) PiSkills.E.Cast(minion);
                    }
                    //Set Q
                    if (PiMenu.LaneQ)
                        if (minion.IsInRange(Ryze, 1000)
                        && IsFluxed(minion))
                    {
                        if (PiSkills.Q.IsReady()
                            && minion.IsValidTarget(1000))
                                Core.DelayAction(() => PiSkills.Q.Cast(minion), random.Next(50, 250));
                    }
                    //Set W
                    if (PiMenu.LaneW)
                        if (minion.IsValidTarget(615, true, Ryze.Position))
                        if (minionHealth < PiDamages.DamageBySlot(minion, SpellSlot.W))
                        {
                            if (PiSkills.W.IsReady()
                                && minion.IsValidTarget(615))
                                    Core.DelayAction(() => PiSkills.W.Cast(minion), random.Next(50, 250));
                        }
                }
            }
            if (PiMenu.LaneMode == 1)
            {
                foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsEnemy
                                                                                                    &&
                                                                                                    (m.IsMinion |
                                                                                                     m.IsMonster)
                                                                                                    && !m.IsDead))
                {
                    var fluxed =
                        EntityManager.MinionsAndMonsters.GetLaneMinions()
                            .Count(c => c.HasBuff("RyzeE") && c.IsEnemy && c.IsMinion | c.IsMonster && c.Distance(Ryze) < 1000);
                    var blocke = minion.CountEnemyMinionsInRange(200) > 1;
                    if (PiMenu.LaneE)
                    {
                        if (PiSkills.E.IsReady()
                            && minion.IsValidTarget(615) 
                            && blocke)
                        {
                            PiSkills.E.Cast(minion);
                        }
                        else if (IsFluxed(minion))
                        {
                            if (PiSkills.E.IsReady()
                                && minion.IsValidTarget(615) 
                                && blocke)
                            {
                                PiSkills.E.Cast(minion);
                            }
                        }
                        else if (minion.Health < PiDamages.DamageBySlot(minion, SpellSlot.E))
                        {
                            if (PiSkills.E.IsReady()
                                && minion.IsValidTarget(615))
                            {
                                PiSkills.E.Cast(minion);
                            }
                        }
                    }
                    if (PiMenu.LaneQ)
                    {
                        if (fluxed > 1 
                            && blocke)
                        {
                            if (PiSkills.Q.IsReady()
                                && minion.IsValidTarget(1000))
                            {
                                Core.DelayAction(() => PiSkills.Q.Cast(minion), random.Next(50, 250));
                            }
                        }
                    }

                }
            }
        }
    }
}
