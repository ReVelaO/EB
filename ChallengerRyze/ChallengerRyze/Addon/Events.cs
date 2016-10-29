using System;
using EloBuddy;
using EloBuddy.SDK;
using Color = System.Drawing.Color;

namespace ChallengerRyze.Addon
{
    public static class Events
    {
        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
        }
        static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }
            else if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.LaneClear))
            {
                Orb.Laneclear.Get();
            }
        }
        static void OnDraw(EventArgs args)
        {
            if (Settings.drawQ
                && Spells.Q.IsReady()) { Spells.Q.DrawRange(Color.FromArgb(150, Color.LightBlue)); }
            if (Settings.drawWE
                && Spells.W.IsReady() 
                && Spells.E.IsReady()) { Spells.W.DrawRange(Color.FromArgb(150, Color.LightBlue)); }
        }
    }
}
