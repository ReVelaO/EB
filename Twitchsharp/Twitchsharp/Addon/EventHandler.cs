using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using Color = System.Drawing.Color;

namespace Twitchsharp.Addon
{
    public static class EventHandler
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static void Load()
        {
            Game.OnTick += OnTick;
            Game.OnTick += Orb.Update.Get;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnTick(EventArgs args)
        {
                        //QSettings
            if (MenuHandler.misc["QRecall"].Cast<CheckBox>().CurrentValue && (MenuHandler.misc["QRecall2"].Cast<KeyBind>().CurrentValue) && SpellHandler.Q.IsReady())
            {
                SpellHandler.Q.Cast();
            }
            
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (MenuHandler.laneclear["amc"].Cast<CheckBox>().CurrentValue == true)
                {
                    if (Twitch.ManaPercent >= MenuHandler.laneclear["sm"].Cast<Slider>().CurrentValue)
                    {
                        Orb.Laneclear.Get();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Orb.Laneclear.Get();
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Orb.Jungleclear.Get();
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (MenuHandler.drawings["aa"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SharpDX.Color.LightGreen, Brain.GetRealAA(), Twitch.Position);
            }

            if (MenuHandler.drawings["w"].Cast<CheckBox>().CurrentValue)
            {
                if (SpellHandler.W.IsReady())
                {
                    SpellHandler.W.DrawRange(Color.FromArgb(130, Color.ForestGreen));
                }
            }

            if (MenuHandler.drawings["e"].Cast<CheckBox>().CurrentValue)
            {
                if (SpellHandler.E.IsReady())
                {
                    SpellHandler.E.DrawRange(Color.FromArgb(130, Color.LightSeaGreen));
                }
            }
        }
    }
}
