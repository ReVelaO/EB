using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Annie_.Addon.GetModes
{
    public static class Laneclear
    {
        private static AIHeroClient me => ObjectManager.Player;
        public static void get()
        {
            if (!Manager.Q.IsReady() && !Manager.W.IsReady()) { return; }
            foreach (var minions in EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(Manager.Q.Range)
            && x.IsEnemy && !x.IsDead && x.IsMinion))
            {
                if (Manager.MLaneQ.CurrentValue)
                {
                    if (!Manager.Q.IsReady()) { return; }
                    if (Manager.MLaneQ2.CurrentValue)
                    {
                        var PredictHealth = Prediction.Health.GetPrediction(minions, Manager.Q.CastDelay) < Manager.DamageBySpell(minions, Manager.Q.Slot);
                        if (PredictHealth
                            && minions.IsValidTarget(Manager.Q.Range)
                            && Manager.Q.IsReady()
                            && Manager.MLaneMana.CurrentValue >= me.ManaPercent) { Manager.Q.Cast(minions); }
                    }
                    else
                    {
                        if (minions.IsValidTarget(Manager.Q.Range)
                        && Manager.Q.IsReady() 
                        && Manager.MLaneMana.CurrentValue >= me.ManaPercent) { Manager.Q.Cast(minions); }
                    }
                }
                if (Manager.MLaneW.CurrentValue)
                {
                    if (!Manager.W.IsReady()) { return; }
                    if (minions.IsValidTarget(Manager.W.Range)
                        && Manager.W.IsReady()
                        && Manager.MLaneMana.CurrentValue >= me.ManaPercent)
                    {
                        if (me.CountEnemyMinionsInRange(Manager.W.Range) >= Manager.MLaneSW.CurrentValue) { Manager.W.Cast(minions.Position); }
                    }
                }
            }
        }
    }
}
