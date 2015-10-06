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
using Color = System.Drawing.Color;

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

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            if (DrawingsMenu["DW"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Orange, Radius = E.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Red, Radius = R.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    Harass();
                    break;
            }
            KS();
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            bool QCHECK = ComboMenu["QU"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = ComboMenu["WU"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = ComboMenu["EU"].Cast<CheckBox>().CurrentValue;
            bool RCHECK = ComboMenu["RU"].Cast<CheckBox>().CurrentValue;
            
            if (E.IsReady() && target.IsValidTarget(E.Range) && ECHECK)
            {
                E_Cast(target);
            }
            if (Q.IsReady() && target.IsValidTarget(Q.Range) && QCHECK)
            {
                Q_Cast(target);
            }
            if (W.IsReady() && target.IsValidTarget(W.Range) && WCHECK)
            {
                W_Cast(target);
            }
            if (R.IsReady() && target.IsValidTarget(R.Range) && RCHECK)
            {
                R_Cast(target);
            }
        }

        private static void Harass()
        {
            var WTarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            var ETarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            bool WCHECK = HarassMenu["HW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = HarassMenu["HE"].Cast<CheckBox>().CurrentValue;

            if (W.IsReady() && WCHECK)
            {
                W_Cast(WTarget);
            }
            if (E.IsReady() && ECHECK)
            {
                E_Cast(ETarget);
            }
        }

        private static void KS()
        {
            bool QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(any => !any.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (QCHECK && enemy.IsValidTarget(Q.Range) && myHero.GetSpellDamage(enemy, SpellSlot.Q) > (enemy.Health - 5) && !enemy.IsDead)
                {
                    Q.Cast(enemy);
                }
                if (WCHECK && enemy.IsValidTarget(W.Range) && myHero.GetSpellDamage(enemy, SpellSlot.W) > (enemy.Health - 5) && !enemy.IsDead)
                {
                    W_Cast(enemy);
                }
                if (ECHECK && enemy.IsValidTarget(E.Range) && myHero.GetSpellDamage(enemy, SpellSlot.E) > (enemy.Health - 5) && !enemy.IsDead)
                {
                    E.Cast(enemy);
                }
            }
        }

        private static void Q_Cast(AIHeroClient target)
        {
            var QPred = Q.GetPrediction(target);

            if (target.IsValidTarget(Q.Range) && QPred.HitChance >= HitChance.High)
            {
                Q.Cast(target);
            }
        }

        private static void W_Cast(AIHeroClient target)
        {
            var WPred = W.GetPrediction(target);

            if (target.IsValidTarget(W.Range) && WPred.HitChance >= HitChance.Medium)
            {
                W.Cast(target);
            }

        }

        private static void E_Cast(AIHeroClient target)
        {
            if (target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
        }

        private static void R_Cast(AIHeroClient target)
        {
            var CustomRange = myHero.CountEnemiesInRange(R.Range) >= ComboMenu["MR"].Cast<Slider>().CurrentValue;
            bool KSRCHECK = ComboMenu["RK"].Cast<CheckBox>().CurrentValue;

            if (CustomRange)
            {
                if (KSRCHECK)
                {
                    if ((target.Health - 5) < myHero.GetSpellDamage(target, SpellSlot.R))
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
    }
}
