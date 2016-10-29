using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerRyze.Addon.Orb
{
    public static class Laneclear
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        private static bool IsFluxed(Obj_AI_Base random) { return random.HasBuff("RyzeE"); }
        public static void Get()
        {
            foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions().Where(m => m.IsEnemy 
            && m.IsMinion | m.IsMonster 
            && !m.IsDead).OrderBy(e => e.Health))
            {
                var minionHealth = Prediction.Health.GetPrediction(minion, 250);
                if (Spells.IsEReady
                    && minion.IsValidTarget(615, true, Ryze.Position)
                    && minionHealth < Library.DamageBySlot(minion, SpellSlot.E)) { Spells.E.Cast(minion); }
                if (Spells.IsQReady
                    && minion.IsInRange(Ryze, 1000))
                {
                    if (IsFluxed(minion)) { Spells.Q.Cast(minion); }
                    else if (minionHealth < Library.DamageBySlot(minion, SpellSlot.Q)) { Spells.Q.Cast(minion); }
                }
                if (Spells.IsWReady 
                    && minion.IsValidTarget(615, true, Ryze.Position)
                    && !Spells.IsQReady | minionHealth < Library.DamageBySlot(minion, SpellSlot.W)) { Spells.W.Cast(minion); }
            }
        }
    }
}
