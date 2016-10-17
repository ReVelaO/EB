using EloBuddy;
using EloBuddy.SDK;
using System;
using Color = System.Drawing.Color;

namespace Sur_s_Annie_
{
    public static class Events
    {
        public static void OnTick(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Addon.Modes.Combo();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Addon.Modes.LastHit();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Addon.Modes.Laneclear();
                    break;
            }
            Manager.LoadKillSteal();
        }
        public static void OnDraw(EventArgs args)
        {
            if (Manager.MDrawQ.CurrentValue) { Manager.Q.DrawRange(Color.OrangeRed); }
            if (Manager.MDrawW.CurrentValue) { Manager.W.DrawRange(Color.OrangeRed); }
            if (Manager.MDrawR.CurrentValue) { Manager.R.DrawRange(Color.Orange); }
        }
        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) { return; }
            if (sender.IsMe 
                && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.LastHit) 
                | Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.LaneClear) 
                && args.Slot == SpellSlot.Q)
            {
                if (Orbwalker.IsAutoAttacking 
                    && Orbwalker.CanBeAborted) { Orbwalker.ResetAutoAttack(); }
            }
        }
    }
}
