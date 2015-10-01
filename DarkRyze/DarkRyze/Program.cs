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
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Ryze)
                return;

            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
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

            FarmMenu = menu.AddSubMenu("Farm", "farmenu");

            FarmMenu.AddGroupLabel("Last Hit Settings");
            FarmMenu.Add("LHQ", new CheckBox("Use Q"));

            KSMenu = menu.AddSubMenu("Kill Steal (KS)", "ksmenu");

            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.Add("KSQ", new CheckBox("Auto Q"));
            KSMenu.Add("KSW", new CheckBox("Auto W"));
            KSMenu.Add("KSE", new CheckBox("Auto E"));

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
                Drawing.DrawCircle(myHero.Position, 900, System.Drawing.Color.BlueViolet);
            }

            if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(myHero.Position, 600, System.Drawing.Color.BlueViolet);
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combos();
                    break;
                    case Orbwalker.ActiveModes.LastHit:
                    LastHit();
                    break;
            }

            KS();
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
            var target = TS.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(600))
            {
                if (W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
            }
        }

        public static void QWER()
        {
            var target = TS.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                if (Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    UseR();
                }
            }
        }

        public static void QEWR()
        {
            var target = TS.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                if (Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && R.IsReady())
                {
                    UseR();
                }

            }
        }

        public static void WQRE()
        {
            var target = TS.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(600))
            {
                if (W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && E.IsReady())
                {
                    UseE();
                }
                if (!E.IsReady() && W.IsReady())
                {
                    UseW();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    UseQ();
                }
                if (!Q.IsReady() && R.IsReady())
                {
                    UseR();
                }
                if (!R.IsReady() && E.IsReady())
                {
                    UseE();
                }
            }
        }

        public static void UseQ()
        {
            var target = TS.GetTarget(900, DamageType.Magical);
            var qpred = Q.GetPrediction(target);

            if (target.IsValidTarget(900))
            {
                Q.Cast(qpred.UnitPosition);
            }
        }

        public static void UseW()
        {
            var target = TS.GetTarget(600, DamageType.Magical);

            if (target.IsValidTarget(600))
            {
                W.Cast(target);
            }
        }

        public static void UseE()
        {
            var target = TS.GetTarget(600, DamageType.Magical);

            if (target.IsValidTarget(600))
            {
                E.Cast(target);
            }
        }

        public static void UseR()
        {
            if (myHero.HasBuff("ryzepassivecharged") || myHero.GetBuffCount("ryzepassivestack") == 4)
            {
                R.Cast();
            }
        }

        public static void KS()
        {
            var QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            var WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            var ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            foreach (var enemy in HeroManager.Enemies.Where(any => !any.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (WCHECK && enemy.IsValidTarget(W.Range) && WDamage(enemy) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    W.Cast(enemy);
                }
                if (QCHECK && enemy.IsValidTarget(Q.Range) && QDamage(enemy) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    Q.Cast(enemy);
                }
                if (ECHECK && enemy.IsValidTarget(E.Range) && EDamage(enemy) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    E.Cast(enemy);
                }
            }
        }

        public static void LastHit()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            var xx =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(x => x.IsEnemy && x.Distance(myHero) < myHero.GetAutoAttackRange())
                    .OrderBy(x => x.Health)
                    .FirstOrDefault();

            if (FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue && QDamage(xx) > (xx.Health - 5) && xx.Distance(myHero) < Q.Range && !xx.IsDead)
            {
                 Q.Cast(xx);
                 return;
            }
        }

        private static double QDamage(Obj_AI_Base target)
        {
           return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
               (float)(new double[] { 60, 85, 110, 135, 160 }[Q.Level - 1] + 0.55 * myHero.FlatMagicDamageMod + 
                       new double[] { 2, 2.5, 3.0, 3.5, 4.0 }[Q.Level - 1] / 100 * myHero.MaxMana));
        }

        public static float WDamage(Obj_AI_Base target)
        {
            return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 80, 100, 120, 140, 160 }[W.Level - 1] +
                0.4f * myHero.FlatMagicDamageMod +
                0.02f * myHero.MaxMana);
        }

        public static float EDamage(Obj_AI_Base target)
        {
            return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 36, 52, 68, 84, 100 }[E.Level - 1] + 
                0.2f * myHero.FlatMagicDamageMod + 
                0.025f * myHero.MaxMana);
        }
        
    }
}
