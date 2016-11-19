using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using Sur_s_Xin_.Addon.Orb;

namespace Sur_s_Xin_.Addon
{
    public class Events
    {
        private const float SmiteableCircle = 250;

        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += OnAfterAttack;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                Combo.Get();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Jungleclear.Get();
            AutoSmite();
        }

        private static void OnDraw(EventArgs args)
        {
            if (Settings.DibujarE.CurrentValue && Spells.E.IsReady()) Spells.E.DrawRange(Color.Purple);
            if (Settings.DibujarR.CurrentValue && Spells.R.IsReady()) Spells.R.DrawRange(Color.LightBlue);
            if (Settings.DrawSmite.CurrentValue)
                if (Spells.IsSmiteReady)
                    foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                            && !m.IsDead
                                                                                                            &&
                                                                                                            Library
                                                                                                                .MonstersNames
                                                                                                                .Contains
                                                                                                                (m
                                                                                                                    .BaseSkinName)
                                                                                                            &&
                                                                                                            m.IsInRange(
                                                                                                                Player
                                                                                                                    .Instance,
                                                                                                                1000)))
                    {
                        if (Library.GetHealthPrediction(monster, Spells.Smite.CastDelay) >
                            Player.Instance.GetSummonerSpellDamage(monster, DamageLibrary.SummonerSpells.Smite))
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, Color.White);
                        if (Library.GetHealthPrediction(monster, Spells.Smite.CastDelay) <
                            Player.Instance.GetSummonerSpellDamage(monster, DamageLibrary.SummonerSpells.Smite))
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, Color.Lime);
                    }
        }

        private static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            if (Settings.UsarQ.CurrentValue)
                if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                {
                    var starget = target as AIHeroClient;
                    if (starget != null)
                    {
                        if (Settings.FixQ.CurrentValue)
                        {
                            if (starget.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
                                if (Spells.Q.IsReady()) Spells.Q.Cast();
                        }
                        else if (Settings.FixQ.CurrentValue == false)
                        {
                            if (starget.IsInAutoAttackRange(Player.Instance))
                                if (Spells.Q.IsReady()) Spells.Q.Cast();
                        }
                    }
                }
            if (Settings.JusarQ.CurrentValue)
                if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.JungleClear))
                {
                    var monster = target as Obj_AI_Minion;
                    if (monster != null)
                    {
                        if (monster.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
                            if (Spells.Q.IsReady()) Spells.Q.Cast();
                    }
                }
        }

        private static bool InDuel(Obj_AI_Base random)
        {
            return random.HasBuff("Xenzhaointimidate");
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var random = sender as AIHeroClient;
            if ((args.DangerLevel != DangerLevel.High)
                || !Spells.R.IsReady()
                || !random.IsInRange(Player.Instance, Spells.R.Range)
                || InDuel(random)) return;
            if ((random != null) && random.IsInRange(Player.Instance, Spells.R.Range) && random.IsEnemy &&
                (args.DangerLevel >= DangerLevel.Medium)) Spells.R.Cast();
        }

        private static void AutoSmite()
        {
            if (Settings.Autsmite.CurrentValue)
            {
                foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                        && !m.IsDead))
                {
                    if (Settings.UsarSmite.CurrentValue)
                        if (Spells.Smite.IsReady() && monster.IsValidTarget(Spells.Smite.Range))
                            if (Library.MonstersNames.Contains(monster.BaseSkinName)
                                &&
                                (Library.GetHealthPrediction(monster, Spells.Smite.CastDelay) <
                                 Player.Instance.GetSummonerSpellDamage(monster, DamageLibrary.SummonerSpells.Smite)))
                                Spells.Smite.Cast(monster);
                }
            }
        }
    }
}