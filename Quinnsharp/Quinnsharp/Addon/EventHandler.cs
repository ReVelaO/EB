using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using Color = System.Drawing.Color;

namespace Quinnsharp.Addon
{
    public static class EventHandler
    {
        private static AIHeroClient Quinn => Player.Instance;

        public static void Load()
        {
            Game.OnTick += OnTick;
            Game.OnUpdate += OnUpdate;
            Orbwalker.OnPostAttack += OnAfterAttack;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Orb.Laneclear.Get();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Orb.Jungleclear.Get();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Orb.Flee.Get();
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            Orb.Update.KillSteal();
        }

        private static void OnDraw(EventArgs args)
        {
            if (SpellHandler.Q.IsReady())
            {
                SpellHandler.Q.DrawRange(Color.FromArgb(130, Color.LightBlue));
            }

            if (SpellHandler.E.IsReady())
            {
                SpellHandler.E.DrawRange(Color.FromArgb(130, Color.DarkBlue));
            }
        }

        private static void OnAfterAttack(AttackableUnit unit, EventArgs args)
        {
            if (SpellHandler.Q.IsReady())
            {
                var target = unit as AIHeroClient;
                if (target.IsHitable())
                {
                    var p = SpellHandler.Q.GetPrediction(target);
                    if (p != null && p.HitChance > HitChance.Medium)
                    {
                        SpellHandler.Q.Cast(p.CastPosition);
                    }
                }
            }
            if (SpellHandler.E.IsReady())
            {
                var target = unit as AIHeroClient;
                if (target.IsHitable())
                {
                    SpellHandler.E.Cast(target);
                }
            }
        }
    }
}