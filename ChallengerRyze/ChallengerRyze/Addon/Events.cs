using System;
using System.Drawing;
using System.Linq;
using ChallengerRyze.Addon.Orb;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace ChallengerRyze.Addon
{
    public static class Events
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            Spellbook.OnCastSpell += OnCastSpell;
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                Combo.Get();
            else if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.LaneClear))
                Laneclear.Get();
            else if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.JungleClear))
                Jungleclear.Get();
            OnKillSteal();
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DrawQ
                && Spells.Q.IsReady()) Spells.Q.DrawRange(Color.FromArgb(150, Color.DodgerBlue));
            if (Settings.DrawWe && (Spells.W.IsReady() | Spells.E.IsReady()))
                Spells.W.DrawRange(Color.FromArgb(150, Color.DeepSkyBlue));
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (Settings.ForceQ 
                && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                if (sender.Owner.IsMe)
                {
                    if (args.Slot == SpellSlot.W
                        && Spells.Q.IsReady() && Spells.E.IsReady())
                    {
                        args.Process = false;
                    }
                }
            }
        }

        private static void OnKillSteal()
        {
            if (Settings._ksq.CurrentValue)
            {
                foreach (var random in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(615) 
                && !x.IsInvulnerable && x.TotalShieldHealth() < Library.DamageBySlot(x, SpellSlot.Q)))
                {
                    Spells.CastQ(random);
                }
            }
            if (Settings._ksw.CurrentValue)
            {
                foreach (var random in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(615)
                && !x.IsInvulnerable && x.TotalShieldHealth() < Library.DamageBySlot(x, SpellSlot.W)))
                {
                    Spells.CastW(random);
                }
            }
            if (Settings._kse.CurrentValue)
            {
                foreach (var random in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(615)
                && !x.IsInvulnerable && x.TotalShieldHealth() < Library.DamageBySlot(x, SpellSlot.E)))
                {
                    Spells.CastE(random);
                }
            }
            if (Settings._ignite.CurrentValue)
            {
                foreach (var random in EntityManager.Heroes.Enemies.Where(x => x.IsInRange(Ryze, Spells.Ignite.Range)))
                {
                    if (Spells.GetHealthPrediction(random, Spells.Ignite.CastDelay) < Library.Ignite(random)
                        && Spells.Ignite.IsReady()
                        && Spells.Ignite.CanCast(random))
                    {
                        Spells.Ignite.Cast(random);
                    }
                }
            }
        }
    }
}