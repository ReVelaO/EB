using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Annie_.Addon.GetModes
{
    public static class LastHit
    {
        private static AIHeroClient me => ObjectManager.Player;
        public static void get()
        {
            if (Manager.MFarmQ.CurrentValue)
            {
                if (Manager.MFarmQ2.CurrentValue)
                {
                    if (Manager.OnStunPassive) { return; }
                }
                if (!Manager.Q.IsReady()) { return; }
                foreach (var minions in EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(Manager.Q.Range)
                && x.IsEnemy && !x.IsDead && x.IsMinion
                && Prediction.Health.GetPrediction(x, Manager.Q.CastDelay) < Manager.DamageBySpell(x, Manager.Q.Slot)))
                {
                    if (minions == null) { return; }
                    if (Manager.Q.IsReady()
                        && minions.IsValidTarget(Manager.Q.Range)) { Manager.Q.Cast(minions); }
                }
            }
            else
            {
                return;
            }
        }
    }
}
