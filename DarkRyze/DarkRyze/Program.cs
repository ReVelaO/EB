using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System.Drawing;

namespace DarkRyze
{
    class Program
    {
        public static Menu ComboMenu, DrawingsMenu, KSMenu, MiscMenu, FarmMenu, menu;
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Bootstrap.Init(null);
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 50);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);


            menu = MainMenu.AddMenu("DarkRyze", "DarkRyze");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            
            ComboMenu.AddGroupLabel("Combo Selector");
            var cs = ComboMenu.Add("css", new Slider("Combo Selector", 0, 0, 3));
            var co = new[] { "WQER", "QWER", "QEWR", "WQRE" };
            cs.DisplayName = co[cs.CurrentValue];

            cs.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = co[changeArgs.NewValue];
                };

            /*FarmMenu = menu.AddSubMenu("Farm", "farmenu");

            FarmMenu.AddGroupLabel("Last Hit Settings");
            FarmMenu.Add("LHQ", new CheckBox("Use Q"));

            KSMenu = menu.AddSubMenu("Kill Steal (KS)", "ksmenu");

            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.Add("KSQ", new CheckBox("Auto Q"));
            KSMenu.Add("KSW", new CheckBox("Auto W"));
            KSMenu.Add("KSE", new CheckBox("Auto E"));*/

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");

            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            DrawingsMenu.Add("DWE", new CheckBox("Draw W + E"));

            MiscMenu = menu.AddSubMenu("Misc", "miscmenu");

            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("Misc1", new CheckBox("Anti-Gapcloser [W Usage]"));
            MiscMenu.Add("Misc2", new CheckBox("Auto-Interrupt [W Usage]"));

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, 900, System.Drawing.Color.BlueViolet);
            }

            if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, 600, System.Drawing.Color.BlueViolet);
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combos();
                    break;
                    //case Orbwalker.ActiveModes.LastHit: (Working on it :')
                    //LastHit();
                    //break;
            }

            //KS(); (Working on it :')
        }

        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            var WGCHECK = MiscMenu["Misc1"].Cast<CheckBox>().CurrentValue;
            if (W.IsReady() && WGCHECK)
            {
                W.Cast(sender);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs args)
        {
            var WICHECK = MiscMenu["Misc2"].Cast<CheckBox>().CurrentValue;
            if (W.IsReady() && WICHECK)
            {
                W.Cast(sender);
            }
        }

        public static void Combos()
        {
              var options = ComboMenu["css"].DisplayName;
              switch (options)
              {
                 case "WQER":
                    WQER();
                    break;
                 case "QWER":
                    QWER();
                    break;
                 case "QEWR":
                    QEWR();
                    break;
                 case "WQRE":
                    WQRE();
                    break;
              }
        }

        public static void WQER()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                UseW();
                UseQ();
                UseE();
                UseR();
            }
        }

        public static void QWER()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                UseQ();
                UseW();
                UseE();
                UseR();
            }
        }

        public static void QEWR()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                UseQ();
                UseE();
                UseW();
                UseR();
            }
        }

        public static void WQRE()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                UseW();
                UseQ();
                UseR();
                UseE();
            }
        }

        public static void UseQ()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (Q.IsReady())
            {
                Q.Cast(target.ServerPosition);
            }
        }

        public static void UseW()
        {
            var target = TargetSelector.GetTarget(600, DamageType.Magical);

            if (W.IsReady())
            {
                W.Cast(target);
            }
        }

        public static void UseE()
        {
            var target = TargetSelector.GetTarget(600, DamageType.Magical);

            if (E.IsReady())
            {
                E.Cast(target);
            }
        }

        public static void UseR()
        {
            if (R.IsReady() && _Player.HasBuff("ryzepassivecharged") || _Player.GetBuffCount("ryzepassivestack") == 4)
            {
                R.Cast();
            }
        }

        /*public static void KS()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || !target.IsValidTarget() || target.IsInvulnerable)
                return;

            var QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            var WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            var ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            if (QCHECK
                && QDamage(target) > target.Health
                && target.IsValidTarget(Q.Range))
                Q.Cast(target);

            if (WCHECK
                && WDamage(target) > target.Health
                && target.IsValidTarget(W.Range))
                W.Cast(target);

            if (ECHECK
                && EDamage(target) > target.Health
                && target.IsValidTarget(E.Range))
                E.Cast(target);
        }

        public static void LastHit()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            var xx =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(x => x.IsEnemy && x.Distance(_Player) < _Player.GetAutoAttackRange())
                    .OrderBy(x => x.Health)
                    .FirstOrDefault();

            if (FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue && QDamage(xx) > xx.Health && xx.Distance(_Player) < Q.Range)
            {
                 Q.Cast(xx);
                 return;
            }
        }

        private static double QDamage(Obj_AI_Base target)
        {
           return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
               (float)(new double[] { 60, 85, 110, 135, 160 }[Q.Level] + 0.55 * _Player.FlatMagicDamageMod + new double[] { 2, 2.5, 3.0, 3.5, 4.0 }[Q.Level] / 100 * _Player.MaxMana));
        }

        //public static float QDamage(Obj_AI_Base target)
        //{
        //    return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
        //        (float)(new[] { 60, 85, 110, 135, 160 }[Q.Level] + (0.55 * _Player.FlatMagicDamageMod) + (new [] { 2, 2.5, 3.0, 3.5, 4.0 }[Q.Level] / 100 * _Player.MaxMana)));
        //}

        public static float WDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 80, 100, 120, 140, 160 }[W.Level] + (0.4 * _Player.FlatMagicDamageMod) + (0.02 * _Player.MaxMana)));
        }

        public static float EDamage(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new[] { 36, 52, 68, 84, 100 }[E.Level] + (0.2 * _Player.FlatMagicDamageMod) + (0.025 * _Player.MaxMana)));
        }
        */
    }
}
