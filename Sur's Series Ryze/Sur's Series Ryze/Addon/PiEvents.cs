using System;
using EloBuddy;
using EloBuddy.SDK;
using Color = System.Drawing.Color;
namespace Pitufo.Addon
{
    internal class PiEvents
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
            {
                if (PiMenu.DisableAA)
                {
                    if (PiSkills.W.IsReady() 
                        || PiSkills.E.IsReady())
                        Orbwalker.DisableAttacking = true;
                }
                else
                {
                    Orbwalker.DisableAttacking = false;
                }
                Orb.Combo.Get();
            }
            else
            {
                Orbwalker.DisableAttacking = false;
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (PiMenu.LaneMana)
                {
                    if (Player.Instance.ManaPercent > PiMenu.LaneSliMana)
                    {
                        Orb.Laneclear.Get();
                    }
                    else if (Player.Instance.ManaPercent < PiMenu.LaneSliMana)
                    {
                        return;
                    }
                }
                else if (!PiMenu.LaneMana)
                {
                    Orb.Laneclear.Get();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Orb.Jungleclear.Get();
            }
            OnKillSteal();
            OnEmbrace();
            OnSafe();
        }

        private static void OnDraw(EventArgs args)
        {
            if (PiMenu.DrawQ && PiSkills.Q.IsReady())
            {
                PiSkills.Q.DrawRange(Color.FromArgb(150, Color.DodgerBlue));
            }
            if (PiMenu.DrawWE && (PiSkills.W.IsReady() | PiSkills.E.IsReady()))
            {
                PiSkills.W.DrawRange(Color.FromArgb(150, Color.DeepSkyBlue));
            }
            if (PiMenu.DrawIg && !PiSums.Ignite.Slot.Equals(SpellSlot.Unknown))
            {
                if (PiSums.IsIgniteReady)
                {
                    PiSums.Ignite.DrawRange(Color.FromArgb(150, Color.IndianRed));
                }
            }
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                if (sender.Owner.IsMe)
                    if ((args.Slot == SpellSlot.W)
                        && PiSkills.Q.IsReady() && PiSkills.E.IsReady())
                        args.Process = false;
        }
        private static void OnKillSteal()
        {
            if (PiMenu.KsQ)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Find(
                        f =>
                            f.TotalShieldHealth() < PiDamages.DamageBySlot(f, SpellSlot.Q) && f.IsEnemy &&
                            !f.IsInvulnerable);
                if (enemy != null)
                    if (enemy.IsValidTarget(1000) && PiSkills.Q.IsReady())
                {
                    PiSkills.Q.Cast(enemy);
                }
            }
            if (PiMenu.KsW)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Find(
                        f =>
                            f.TotalShieldHealth() < PiDamages.DamageBySlot(f, SpellSlot.W) && f.IsEnemy &&
                            !f.IsInvulnerable);
                if (enemy != null)
                    if (enemy.IsValidTarget(615) && PiSkills.W.IsReady())
                {
                    PiSkills.W.Cast(enemy);
                }
            }
            if (PiMenu.KsE)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Find(
                        f =>
                            f.TotalShieldHealth() < PiDamages.DamageBySlot(f, SpellSlot.E) && f.IsEnemy &&
                            !f.IsInvulnerable);
                if (enemy != null)
                    if (enemy.IsValidTarget(615) && PiSkills.E.IsReady())
                {
                    PiSkills.E.Cast(enemy);
                }
            }
            if (PiMenu.KsIg)
            {
                var enemy =
                    EntityManager.Heroes.Enemies.Find(
                        f =>
                            PiSums.GetHealthPrediction(f, 250) < PiSums.GetDamage(f, DamageLibrary.SummonerSpells.Ignite) && f.IsEnemy &&
                            !f.IsInvulnerable);
                if (enemy != null && PiSums.Ignite.Slot != SpellSlot.Unknown)
                if (enemy.IsValidTarget(PiSums.Ignite.Range) && PiSums.Ignite.IsReady())
                {
                    PiSums.Ignite.Cast(enemy);
                }
            }
        }
        private static void OnSafe()
        {
            var rdistance = 0;
            if (PiSkills.R.Level == 1)
                rdistance = 1750;
            else if (PiSkills.R.Level == 2)
                rdistance = 3000;
            var torre = EntityManager.Turrets.Allies.Find(f => !f.IsDead
                                                               && (f.Distance(Ryze) < rdistance));
            if (torre != null)
                if (PiSkills.R.IsReady()
                    && PiMenu.SafeUlt)
                    PiSkills.R.Cast(torre.Position);
        }

        private static void OnEmbrace()
        {
            if (PiMenu.seraph)
            {
                if (PiStuff.Embrace.IsOwned()
                    && PiStuff.Embrace.IsReady())
                {
                    if (Player.Instance.ManaPercent >= 22
                        && Player.Instance.CountEnemiesInRange(PiMenu.seraphRange) > 1
                        && Player.Instance.HealthPercent <= PiMenu.seraphSli)
                    {
                        PiStuff.Embrace.Cast();
                    }
                }
            }
        }
    }
}
