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

namespace DarkBrand
{
    class Program
    {
        public static Menu ComboMenu, DrawingsMenu, KSMenu, HarassMenu, menu;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Brand)
                return;

            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1200, 85);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 5, int.MaxValue, 250);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Targeted(SpellSlot.R, 750);


            menu = MainMenu.AddMenu("DarkBrand", "DarkBrand");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("QU", new CheckBox("Use Q"));
            ComboMenu.Add("WU", new CheckBox("Use W"));
            ComboMenu.Add("EU", new CheckBox("Use E"));
            ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.Add("RK", new CheckBox("Use R if Killable"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("MR", new Slider("Min.Enemy in [R] Range", 1, 1, 5));

            HarassMenu = menu.AddSubMenu("Harass", "farmenu");

            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("HW", new CheckBox("Auto W"));
            HarassMenu.Add("HE", new CheckBox("Auto E"));

            KSMenu = menu.AddSubMenu("Kill Steal (KS)", "ksmenu");

            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.Add("KSQ", new CheckBox("Auto Q"));
            KSMenu.Add("KSW", new CheckBox("Auto W"));
            KSMenu.Add("KSE", new CheckBox("Auto E"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");

            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            DrawingsMenu.Add("DW", new CheckBox("Draw W"));
            DrawingsMenu.Add("DE", new CheckBox("Draw E"));
            DrawingsMenu.Add("DR", new CheckBox("Draw R"));

            /*MiscMenu = menu.AddSubMenu("Misc", "miscmenu");

            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("Misc1", new CheckBox("Anti-Gapcloser [W Usage]"));
            MiscMenu.Add("Misc2", new CheckBox("Auto-Interrupt [W Usage]"));*/

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(myHero.Position, Q.Range, System.Drawing.Color.Orange);
            }

            if (DrawingsMenu["DW"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(myHero.Position, W.Range, System.Drawing.Color.Orange);
            }

            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(myHero.Position, E.Range, System.Drawing.Color.Orange);
            }

            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(myHero.Position, R.Range, System.Drawing.Color.DarkRed);
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combos();
                    break;
                    case Orbwalker.ActiveModes.Harass:
                    Harass();
                    break;
            }

            KS();
        }

        public static void Combos()
        {
            var QCHECK = ComboMenu["QU"].Cast<CheckBox>().CurrentValue;
            var WCHECK = ComboMenu["WU"].Cast<CheckBox>().CurrentValue;
            var ECHECK = ComboMenu["EU"].Cast<CheckBox>().CurrentValue;
            var RCHECK = ComboMenu["RU"].Cast<CheckBox>().CurrentValue;

            if (E.IsReady() && ECHECK)
            {
                UseE();
            }

            if (Q.IsReady() && QCHECK)
            {
                UseQ();
            }

            if (W.IsReady() && WCHECK)
            {
                UseW();
            }

            if (R.IsReady() && RCHECK)
            {
                UseR();
            }

        }

        public static void Harass()
        {
            var wt = TS.GetTarget(W.Range, DamageType.Magical);
            var et = TS.GetTarget(E.Range, DamageType.Magical);
            var WCHECK = HarassMenu["HW"].Cast<CheckBox>().CurrentValue;
            var ECHECK = HarassMenu["HE"].Cast<CheckBox>().CurrentValue;

            if (W.IsReady())
            {
                UseW();
            }

            if (ECHECK && E.IsReady())
            {
                UseE();
            }

        }

        public static void UseQ()
        {
            var target = TS.GetTarget(Q.Range, DamageType.Magical);
            var QPred = Q.GetPrediction(target);

            if (target.IsValidTarget(Q.Range) && QPred.HitChance >= HitChance.High)
            {
                Q.Cast(target);
            }
        }

        public static void UseW()
        {
            var target = TS.GetTarget(W.Range, DamageType.Magical);
            var WPred = W.GetPrediction(target);

            if (target.IsValidTarget(W.Range) && WPred.HitChance >= HitChance.High)
            {
                W.Cast(target);
            }
        }

        public static void UseE()
        {
            var target = TS.GetTarget(E.Range, DamageType.Magical);

            if (target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
        }

        public static void UseR()
        {
            var target = TS.GetTarget(R.Range, DamageType.Magical);
            var CustomRange = myHero.CountEnemiesInRange(R.Range) >= ComboMenu["MR"].Cast<Slider>().CurrentValue;

            if (CustomRange)
            {
                if (ComboMenu["RK"].Cast<CheckBox>().CurrentValue)
                {
                    if ((target.Health - 5) < RDamage(target))
                    {
                        R.Cast(target);
                    }
                }

                else
                {
                    R.Cast(target);
                }
            }
        }

        public static void KS()
        {
            var QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            var WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            var ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(Q.Range) && !x.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (QCHECK && QDamage(enemy) > (enemy.Health - 10))
                {
                    Q.Cast(enemy);
                }
            }

            foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(W.Range) && !x.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (WCHECK && WDamage(enemy) > (enemy.Health - 10))
                {
                    W.Cast(enemy);
                }
            }


            foreach (var enemy in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (ECHECK && EDamage(enemy) > (enemy.Health - 10))
                {
                    E.Cast(enemy);
                }
            }
        }

        public static float QDamage(Obj_AI_Base target)
        {
            return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 80, 120, 160, 200, 240 }[Q.Level - 1] +
                0.65f * myHero.FlatMagicDamageMod);
        }

        public static float WDamage(Obj_AI_Base target)
        {
            return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 75, 120, 165, 210, 255 }[W.Level - 1] +
                0.6f * myHero.FlatMagicDamageMod);
        }

        public static float EDamage(Obj_AI_Base target)
        {
            return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 70, 105, 140, 175, 210 }[E.Level - 1] + 
                0.55f * myHero.FlatMagicDamageMod);
        }

        public static float RDamage(Obj_AI_Base target)
        {
            return myHero.CalculateDamageOnUnit(target, DamageType.Magical,
                new[] { 150, 250, 350 }[R.Level - 1] +
                0.5f * myHero.FlatMagicDamageMod);
        }
    }
}
