using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Evelynn.Addon.Orb;

namespace Evelynn.Addon
{
    internal class EveEvents
    {
        private const float SmiteableCircle = 250;
        private static AIHeroClient Evelynn => ObjectManager.Player;

        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                Combo.Get();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Jungleclear.Get();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (EveMenu.Lmm.CurrentValue)
                {
                    if (Evelynn.ManaPercent > EveMenu.LmmS.CurrentValue) Laneclear.Get();
                }
                else if (!EveMenu.Lmm.CurrentValue)
                {
                    Laneclear.Get();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                if (EveSpells.W.IsReady()) EveSpells.W.Cast();
            OnKillSteal();
            OnMaxShield();
        }

        private static void OnDraw(EventArgs args)
        {
            if (EveMenu.Dq.CurrentValue) EveSpells.Q.DrawRange(Color.MediumPurple);
            if (EveMenu.De.CurrentValue && EveSpells.E.IsReady()) EveSpells.E.DrawRange(Color.LightCoral);
            if (EveMenu.Dr.CurrentValue && EveSpells.R.IsReady()) EveSpells.R.DrawRange(Color.BlueViolet);
            if (EveMenu.DSt.CurrentValue)
                if (EveSpells.Smite.IsReady())
                    foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                            && !m.IsDead
                                                                                                            &&
                                                                                                            EveSpells
                                                                                                                .Monsters
                                                                                                                .Contains
                                                                                                                (
                                                                                                                    m
                                                                                                                        .BaseSkinName) &&
                                                                                                            m.IsInRange(
                                                                                                                Player
                                                                                                                    .Instance,
                                                                                                                1000)))
                    {
                        if (EveSpells.GetHealthPrediction(monster, EveSpells.Smite.CastDelay) >
                            EveDamages.Smite(monster))
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, Color.Azure);
                        if (EveSpells.GetHealthPrediction(monster, EveSpells.Smite.CastDelay) <
                            EveDamages.Smite(monster))
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, Color.Lime);
                    }
        }

        private static void OnKillSteal()
        {
            foreach (var random in EntityManager.Heroes.Enemies.Where(w => w.IsInRange(Evelynn, 650)))
            {
                if (EveMenu.Kq.CurrentValue)
                if (EveSpells.Q.IsReady()
                    && (random.TotalShieldHealth() < EveDamages.Q(random))) EveSpells.Q.Cast();
                if (EveMenu.Ke.CurrentValue)
                if (EveSpells.E.IsReady()
                    && (random.TotalShieldHealth() < EveDamages.E(random))) EveSpells.E.Cast(random);
                if (EveMenu.Ki.CurrentValue)
                    if (EveSpells.Ignite.Slot != SpellSlot.Unknown)
                    if (EveSpells.Ignite.IsReady()
                        && (random.TotalShieldHealth() < EveDamages.Ignite(random)))
                        EveSpells.Ignite.Cast(random);
            }
        }

        private static void OnMaxShield()
        {
            if (EveMenu.AutoR.CurrentValue)
            {
                var max =
                    EntityManager.Heroes.Enemies.Where(w => w.IsValidTarget(EveSpells.R.Range));
                var prediction = EveSpells.R.GetBestCircularCastPosition(max, 85, EveSpells.R.CastDelay);
                if (max != null)
                    if ((prediction.HitNumber >= 4)
                        && EveSpells.R.IsReady()
                        && (Evelynn.CountAlliesInRange(1000) >= 3)
                        && (Evelynn.CountEnemiesInRange(1000) >= 3))
                        EveSpells.R.Cast(prediction.CastPosition);
            }
        }
    }
}