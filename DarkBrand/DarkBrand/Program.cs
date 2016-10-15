using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Notifications;

namespace DarkBrand
{
    static class Program
    {
        private static Menu ComboMenu, DrawingsMenu, KSMenu, HarassMenu, menu;
        private static Spell.Skillshot Q;
        private static Spell.Skillshot W;
        private static Spell.Targeted E;
        private static Spell.Targeted R;
        private static AIHeroClient Brand { get { return ObjectManager.Player; } }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += BrandLoad;
        }

        private static void BrandLoad(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Brand)
                return;
            Chat.Print("<font color='#FFFFFF'>[Addon Brand: </font><font color='#34BFD6'>Loaded</font>]");
            Notifications.Show(new SimpleNotification("Dark Brand","Welcome back buddy!"));
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1600, 60);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 5, int.MaxValue, 250);
            E = new Spell.Targeted(SpellSlot.E, 640);
            R = new Spell.Targeted(SpellSlot.R, 750);

            menu = MainMenu.AddMenu("DarkBrand", "DarkBrand");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");

            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("QU", new CheckBox("Use Q"));
            ComboMenu.Add("WU", new CheckBox("Use W"));
            ComboMenu.Add("EU", new CheckBox("Use E"));
            ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("MR", new Slider("Min. Enemies in [R] Range", 1, 1, 5));
            ComboMenu.Add("ha", new Slider("Hitchance percent of habilities", 69, 1, 100));

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
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue) { Q.DrawRange(Color.Orange); }
            if (DrawingsMenu["DW"].Cast<CheckBox>().CurrentValue) { W.DrawRange(Color.Orange); }
            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue) { E.DrawRange(Color.Orange); }
            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue) { R.DrawRange(Color.Red); }
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
            KillSteal();
        }
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            bool QCHECK = ComboMenu["QU"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = ComboMenu["WU"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = ComboMenu["EU"].Cast<CheckBox>().CurrentValue;
            bool RCHECK = ComboMenu["RU"].Cast<CheckBox>().CurrentValue;
            float hitchancep = ComboMenu["ha"].Cast<Slider>().CurrentValue;
            var QPred = Prediction.Position.PredictLinearMissile(target, Q.Range, Q.Radius, Q.CastDelay, Q.Speed, int.MaxValue, Brand.Position);
            var WPred = Prediction.Position.PredictCircularMissile(target, W.Range, W.Radius, W.CastDelay, W.Speed, Brand.Position);

            if (!target.IsValid || target == null)
            {
                return;
            }
            if (ECHECK)
            {
                if (!E.IsReady()) { return; }
                if (E.IsReady())
                {
                    if (target.IsValidTarget(E.Range)) { E.Cast(target); }
                }
            }
            if (QCHECK)
            {
                if (!Q.IsReady()) { return; }
                if (Q.IsReady())
                {
                    if (target.IsValidTarget(Q.Range))
                    {
                        if (IsBlazed(target)
                            && QPred.HitChancePercent >= hitchancep) { Q.Cast(QPred.CastPosition); }
                        else
                        {
                            if (QPred.HitChance >= HitChance.High) { Q.Cast(QPred.CastPosition); }
                        }
                    }
                }
            }
            if (WCHECK)
            {
                if (!W.IsReady()) { return; }
                if (W.IsReady())
                {
                    if (IsBlazed(target) 
                        && WPred.HitChancePercent >= hitchancep) { W.Cast(WPred.CastPosition); }
                    else
                    {
                        if (!IsBlazed(target) 
                            && WPred.HitChancePercent >= hitchancep) { W.Cast(WPred.CastPosition); }
                    }
                }
            }
            if (RCHECK)
            {
                if (!R.IsReady()) { return; }
                if (R.IsReady()) { CustomR_Cast(target); }
            }
        }
        private static void Harass()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            float Control = HarassMenu["HMC"].Cast<Slider>().CurrentValue;
            bool WCHECK = HarassMenu["HW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = HarassMenu["HE"].Cast<CheckBox>().CurrentValue;
            if (target == null)
            {
                return;
            }
            if (WCHECK)
            {
                if (!W.IsReady()) { return; }
                if (W.IsReady())
                {
                    if (target.IsValidTarget(W.Range)
                        && Brand.ManaPercent >= Control) { W.Cast(target.Position); }
                }
            }
            if (ECHECK)
            {
                if (!E.IsReady()) { return; }
                if (E.IsReady())
                {
                    if (target.IsValidTarget(E.Range)
                        && Brand.ManaPercent >= Control) { E.Cast(target); }
                }
            }
        }
        private static void KillSteal()
        {
            bool QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => !x.HasBuffOfType(BuffType.Invulnerability) && x.IsValidTarget(Q.Range) && x.IsEnemy && !x.IsDead))
            {
                var QPred = Prediction.Position.PredictLinearMissile(enemy, Q.Range, Q.Radius, Q.CastDelay, Q.Speed, int.MaxValue, Brand.Position);
                var WPred = Prediction.Position.PredictCircularMissile(enemy, W.Range, W.Radius, W.CastDelay, W.Speed, Brand.Position);

                if (QCHECK)
                {
                    if (!Q.IsReady()) { return; }
                    if (enemy.IsValidTarget(Q.Range)
                    && Brand.GetSpellDamage(enemy, SpellSlot.Q) > (enemy.TotalShieldHealth() - 5)
                    && QPred.HitChance >= HitChance.High
                    && !enemy.IsDead) { Q.Cast(QPred.CastPosition); }
                }
                if (WCHECK)
                {
                    if (!W.IsReady()) { return; }
                    if (enemy.IsValidTarget(W.Range)
                    && Brand.GetSpellDamage(enemy, SpellSlot.W) > (enemy.TotalShieldHealth() - 5)
                    && WPred.HitChance >= HitChance.High
                    && !enemy.IsDead) { W.Cast(WPred.CastPosition); }
                }
                if (ECHECK)
                {
                    if (!E.IsReady()) { return; }
                    if (enemy.IsValidTarget(E.Range)
                    && Brand.GetSpellDamage(enemy, SpellSlot.E) > (enemy.TotalShieldHealth() - 5)
                    && !enemy.IsDead) { E.Cast(enemy); }
                }
            }
        }
        private static void CustomR_Cast(AIHeroClient target)
        {
            float SliderInRange = ComboMenu["MR"].Cast<Slider>().CurrentValue;

            if (!R.IsReady()) { return; }
            if (R.IsReady())
            {
                if (target.IsValidTarget(R.Range))
                {
                    if ((target.TotalShieldHealth() - 5) < Brand.GetSpellDamage(target, SpellSlot.R)) { R.Cast(target); }
                    else
                    {
                        if (target.CountEnemiesInRange(R.Range) >= SliderInRange) { R.Cast(target); }
                    }
                }
            }
        }
        private static bool IsBlazed(AIHeroClient target) => target.HasBuff("BrandAblaze");
    }
}
