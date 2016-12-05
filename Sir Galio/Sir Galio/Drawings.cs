namespace Galio
{
    using System;
    using EloBuddy;
    using EloBuddy.SDK.Menu.Values;
    using static Menus;
    using static Spells;
    internal class Drawings
    {
        public static void CreateDrawings()
        {
            Drawing.OnDraw += Drawing_OnDraw;
        }
        static void Drawing_OnDraw(EventArgs args)
        {

            if (DrawingsMenu["qdraw"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                Q.DrawRange(System.Drawing.Color.Orange);
            }

            if (DrawingsMenu["edraw"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                E.DrawRange(System.Drawing.Color.Crimson);
            }

            if (DrawingsMenu["rdraw"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                R.DrawRange(System.Drawing.Color.Crimson);
            }

        }
    }
}