using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using System;

namespace La_Leyenda_de_Caitlyn.Addon
{
    public static class EventHandler
    {
        private static AIHeroClient Caitlyn => Player.Instance;

        public static void Load()
        {
            Game.OnTick += OnTick;
            Game.OnTick += Orb.Update.Get;
            //Drawing.OnDraw += OnDraw;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                Orb.Combo.Get();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Orb.Laneclear.Get();
            }
        }

        /*private static void OnDraw(EventArgs args)
        {
            if (!Caitlyn.IsDead)
            {
                Circle.Draw(SharpDX.Color.LightGreen, 715, Caitlyn.Position);
            }

            if (SpellHandler.Q.IsReady())
            {
                SpellHandler.Q.DrawRange(System.Drawing.Color.Orange);
            }

            if (SpellHandler.W.IsReady())
            {
                SpellHandler.W.DrawRange(System.Drawing.Color.Orange);
            }

            if (SpellHandler.E.IsReady())
            {
                SpellHandler.E.DrawRange(System.Drawing.Color.Orange);
            }
        }*/

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (!sender.IsEnemy) return;

            if (SpellHandler.W.IsReady() && args.Sender.IsInRange(Caitlyn, SpellHandler.W.Range))
            {
                if (args.DangerLevel == DangerLevel.High)
                {
                    var p = SpellHandler.W.GetPrediction(args.Sender);
                    if (p.HitChance > HitChance.Medium)
                    {
                        SpellHandler.W.Cast(args.Sender);
                    }
                }
            }
        }
    }
}