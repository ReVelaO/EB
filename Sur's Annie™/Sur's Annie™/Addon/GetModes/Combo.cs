using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Annie_.Addon.GetModes
{
    public static class Combo
    {
        public static void get()
        {
            var enemy = TargetSelector.GetTarget(Manager.Q.Range, DamageType.Magical);
            if (enemy == null || !enemy.IsValidTarget(Manager.Q.Range)) { return; }
            if (!Manager.Q.IsReady() 
                && !Manager.W.IsReady() 
                && !Manager.R.IsReady()) { return; }
            if (enemy.IsValidTarget(Manager.Q.Range))
            {
                if (Manager.MComboQ.CurrentValue)
                {
                    if (enemy.IsValidTarget(Manager.Q.Range)
                        && Manager.Q.IsReady()) { Manager.Q.Cast(enemy); }
                }
                if (Manager.MComboW.CurrentValue)
                {
                    if (enemy.IsValidTarget(Manager.W.Range)
                        && Manager.W.IsReady()) { Manager.W.Cast(enemy); }
                }
                if (Manager.MComboR.CurrentValue)
                {
                    if (enemy.IsValidTarget(Manager.R.Range)
                        && Manager.R.IsReady()) { Manager.R.Cast(enemy.Position); }
                }
            }
        }
    }
}
