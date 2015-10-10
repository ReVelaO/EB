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
            ComboMenu.Add("MR", new Slider("Min. Enemies in [R] Range", 1, 1, 5));

            HarassMenu = menu.AddSubMenu("Harass", "farmenu");

            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("HW", new CheckBox("Auto W"));
            HarassMenu.Add("HE", new CheckBox("Auto E"));
            HarassMenu.Add("HMC", new Slider("Min. Mana % for Harass", 75, 1, 100));

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

            Game.OnTick += OrbwalkerModes;
            Drawing.OnDraw += Drawings;
        }

        private static void Drawings(EventArgs args)
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

        private static void OrbwalkerModes(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo();
                    break;
                case Orbwalker.ActiveModes.Harass:
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
            var QPred = Prediction.Position.PredictLinearMissile(target, Q.Range, Q.Radius, Q.CastDelay, Q.Speed, int.MaxValue, myHero.ServerPosition);
            var WPred = Prediction.Position.PredictCircularMissile(target, W.Range, W.Radius, W.CastDelay, W.Speed, myHero.ServerPosition);

            if (!target.IsValid || target == null)
            {
                return;
            }
            if (E.IsReady() && ECHECK)
            {
                E.Cast(target);
            }
            if (Q.IsReady() && QCHECK && QPred.HitChance >= HitChance.High)
            {
                Q.Cast(target);
            }
            if (W.IsReady() && WCHECK && WPred.HitChance >= HitChance.High)
            {
                W.Cast(target.ServerPosition);
            }
            if (R.IsReady() && RCHECK)
            {
                CustomR_Cast(target);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Control = HarassMenu["HMC"].Cast<Slider>().CurrentValue;
            bool WCHECK = HarassMenu["HW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = HarassMenu["HE"].Cast<CheckBox>().CurrentValue;

            if (W.IsReady() && WCHECK && myHero.ManaPercent >= Control)
            {
                W.Cast(target.ServerPosition);
            }
            if (E.IsReady() && ECHECK && myHero.ManaPercent >= Control)
            {
                E.Cast(target);
            }
        }

        private static void KS()
        {
            bool QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(any => !any.HasBuffOfType(BuffType.Invulnerability)))
            {
                var QPred = Prediction.Position.PredictLinearMissile(enemy, Q.Range, Q.Radius, Q.CastDelay, Q.Speed, int.MaxValue, myHero.ServerPosition);
                var WPred = Prediction.Position.PredictCircularMissile(enemy, W.Range, W.Radius, W.CastDelay, W.Speed, myHero.ServerPosition);

                if (QCHECK && enemy.IsValidTarget(Q.Range) && myHero.GetSpellDamage(enemy, SpellSlot.Q) > (enemy.Health - 5) && QPred.HitChance >= HitChance.High && !enemy.IsDead)
                {
                    Q.Cast(enemy);
                }
                if (WCHECK && enemy.IsValidTarget(W.Range) && myHero.GetSpellDamage(enemy, SpellSlot.W) > (enemy.Health - 5) && WPred.HitChance >= HitChance.High && !enemy.IsDead)
                {
                    W.Cast(enemy.ServerPosition);
                }
                if (ECHECK && enemy.IsValidTarget(E.Range) && myHero.GetSpellDamage(enemy, SpellSlot.E) > (enemy.Health - 5) && !enemy.IsDead)
                {
                    E.Cast(enemy);
                }
            }
        }

        private static void CustomR_Cast(AIHeroClient target)
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
