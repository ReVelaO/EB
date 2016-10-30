using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using Color = System.Drawing.Color;
using System.Linq;

namespace Sur_s_Xin_.Addon
{
    public class Events
    {
        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            Orbwalker.OnPostAttack += OnAfterAttack;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }
        static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }
            else if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.JungleClear))
            {
                Orb.Jungleclear.Get();
            }
        }
        private static float SmiteableCircle = 250;
        static void OnDraw(EventArgs args)
        {
            if (Settings.dibujarE.CurrentValue && Spells.E.IsReady()) Spells.E.DrawRange(Color.Purple);
            if (Settings.dibujarR.CurrentValue && Spells.R.IsReady()) Spells.R.DrawRange(Color.LightBlue);
            if (Settings.drawSmite.CurrentValue)
            {
                if (Spells.IsSmiteReady)
                {
                    foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                            && !m.IsDead
                            && Library.MonstersNames.Contains(m.BaseSkinName)
                            && m.IsInRange(Player.Instance, 1000)))
                    {
                        if (Library.GetHealthPrediction(monster, Spells.Smite.CastDelay) > Player.Instance.GetSummonerSpellDamage(monster, DamageLibrary.SummonerSpells.Smite))
                        {
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, Color.White);
                        }
                        if (Library.GetHealthPrediction(monster, Spells.Smite.CastDelay) < Player.Instance.GetSummonerSpellDamage(monster, DamageLibrary.SummonerSpells.Smite))
                        {
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, Color.Lime);
                        }
                    }
                }
            }
        }
        static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            if (Settings.usarQ.CurrentValue)
            {
                if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                {
                    var starget = target as AIHeroClient;
                    if (Settings.fixQ.CurrentValue)
                    {
                        if (starget.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
                        {
                            if (Spells.Q.IsReady()) { Spells.Q.Cast(); }
                        }
                    }
                    else if (Settings.fixQ.CurrentValue == false)
                    {
                        if (starget.IsInAutoAttackRange(Player.Instance))
                        {
                            if (Spells.Q.IsReady()) { Spells.Q.Cast(); }
                        }
                    }
                }
            }
            if (Settings.jusarQ.CurrentValue)
            {
                if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.JungleClear))
                {
                    var monster = target as Obj_AI_Minion;
                    if (monster.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
                    {
                        if (Spells.Q.IsReady()) { Spells.Q.Cast(); }
                    }
                }
            }
        }
        private static bool InDuel(Obj_AI_Base random) { return random.HasBuff("Xenzhaointimidate"); }
        static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var random = sender as AIHeroClient;
            if (args.DangerLevel != DangerLevel.High
                || !Spells.R.IsReady()
                || !random.IsInRange(Player.Instance, Spells.R.Range)
                || InDuel(random)) { return; }
            if (random.IsInRange(Player.Instance, Spells.R.Range) 
                && random.IsEnemy 
                && args.DangerLevel >= DangerLevel.Medium) { Spells.R.Cast(); }
        }
    }
}
