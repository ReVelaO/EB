using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using Color = System.Drawing.Color;

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
        }
        static void OnDraw(EventArgs args)
        {
            if (Settings.dibujarE.CurrentValue && Spells.E.IsReady()) Spells.E.DrawRange(Color.Purple);
            if (Settings.dibujarR.CurrentValue && Spells.R.IsReady()) Spells.R.DrawRange(Color.LightBlue);
        }
        static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            var starget = target as AIHeroClient;
            if (starget == null) { return; }
            if (starget != null)
            {
                if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                {
                    if (Settings.usarQ.CurrentValue)
                    {
                        //Fix Q get real AA range of Xin => Perfect Q usage.
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
            }
        }
        static bool InDuel(AIHeroClient random) => random.HasBuff("Xenzhaointimidate");
        static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var random = sender as AIHeroClient;
            if (args.DangerLevel != DangerLevel.High
                || !Spells.R.IsReady()
                || !random.IsInRange(Player.Instance, Spells.R.Range)
                || InDuel(random)) { return; }
            if (random.IsInRange(Player.Instance, Spells.R.Range)) { Spells.R.Cast(); }
        }
    }
}
