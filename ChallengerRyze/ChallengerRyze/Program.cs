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
using SpellData = ChallengerRyze.DamageIndicator.SpellData;

namespace ChallengerRyze
{
    class Program
    {
        public static Menu ComboMenu, DrawingsMenu, KSMenu, MiscMenu, ThemeMenu, FarmMenu, menu;
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static DamageIndicator.DamageIndicator Indicator;
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

            Indicator = new DamageIndicator.DamageIndicator();
            Indicator.Add("Combo", new SpellData(0, DamageType.True, Color.LightSlateGray));

            menu = MainMenu.AddMenu("Challenger Ryze", "challengerryze");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            
            ComboMenu.AddGroupLabel("Combo Selector");
            var cs = ComboMenu.Add("css", new Slider("Combo Selector", 0, 0, 1));
            var co = new[] { "Addon Combo", "Slutty Combo"};
            cs.DisplayName = co[cs.CurrentValue];

            cs.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = co[changeArgs.NewValue];
                };
            ComboMenu.AddGroupLabel("Addon Combo");
            ComboMenu.Add("AUQ", new CheckBox("Use Q"));
            ComboMenu.Add("AUW", new CheckBox("Use W"));
            ComboMenu.Add("AUE", new CheckBox("Use E"));
            ComboMenu.Add("AUR", new CheckBox("Use R"));

            ComboMenu.AddGroupLabel("Slutty Combo");
            ComboMenu.Add("SUQ", new CheckBox("Use Q"));
            ComboMenu.Add("SUW", new CheckBox("Use W"));
            ComboMenu.Add("SUE", new CheckBox("Use E"));
            ComboMenu.Add("SUR", new CheckBox("Use R"));
            ComboMenu.Add("SUAR", new CheckBox("Use R [Rooted Enemy]"));

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
            DrawingsMenu.Add("draw.Damage", new CheckBox("Draw Combo Damage"));

            MiscMenu = menu.AddSubMenu("Misc", "miscmenu");

            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("Misc1", new CheckBox("Anti-Gapcloser [W Usage]"));
            MiscMenu.Add("Misc2", new CheckBox("Auto-Interrupt [W Usage]"));

            ThemeMenu = menu.AddSubMenu("Theme Style", "themestyle");

            ThemeMenu.AddGroupLabel("Themes Settings");
            var xs = ThemeMenu.Add("xss", new Slider("Theme Styles", 0, 0, 4));
            var xo = new[] { "Off", "Theme: Raven", "Theme: Academy", "Theme: Challenger", "Theme: Crystal"};
            xs.DisplayName = xo[xs.CurrentValue];

            xs.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = xo[changeArgs.NewValue];
                };
            TS.init();
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var themes = ThemeMenu["xss"].DisplayName;
            switch (themes)
            {
                case "Off":
                    myHero.SetSkinId(0);
                    if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
                    {
                       new Circle() { Color = Color.BlueViolet, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.BlueViolet, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    break;
                case "Theme: Raven":
                    myHero.SetSkinId(2);
                    if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.Black, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.Black, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    break;
                case "Theme: Academy":
                    myHero.SetSkinId(5);
                    if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.Yellow, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.Yellow, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    break;
                case "Theme: Challenger":
                    myHero.SetSkinId(4);
                    if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.LightSteelBlue, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.LightSteelBlue, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    break;
                case "Theme: Crystal":
                    myHero.SetSkinId(7);
                    if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.BlueViolet, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
                    {
                        new Circle() { Color = Color.BlueViolet, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
                    }
                    break;
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Indicator.Update("Combo", new SpellData((int)ComboDamage(), DamageType.Magical, Color.LightSlateGray));

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ComboMode();
                    break;
                    case Orbwalker.ActiveModes.LastHit:
                    LastHit();
                    break;
            }

            KS();
        }

        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            bool WGCHECK = MiscMenu["Misc1"].Cast<CheckBox>().CurrentValue;
            if (W.IsReady() && WGCHECK)
            {
                W.Cast(sender);
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs args)
        {
            bool WICHECK = MiscMenu["Misc2"].Cast<CheckBox>().CurrentValue;
            if (W.IsReady() && WICHECK)
            {
                W.Cast(sender);
            }
        }

        public static void ComboMode()
        {
              var options = ComboMenu["css"].DisplayName;
              switch (options)
              {
                 case "Addon Combo":
                    AddonCombo();
                    break;
                 case "Slutty Combo":
                    SluttyCombo();
                    break;
              }
        }

        private static void KS()
        {
            bool QCHECK = KSMenu["KSQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = KSMenu["KSW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = KSMenu["KSE"].Cast<CheckBox>().CurrentValue;

            foreach (var enemy in EntityManager.Heroes.Enemies.Where(any => !any.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (WCHECK && enemy.IsValidTarget(W.Range) && myHero.GetSpellDamage(enemy, SpellSlot.W) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    W.Cast(enemy);
                }
                if (QCHECK && enemy.IsValidTarget(Q.Range) && myHero.GetSpellDamage(enemy, SpellSlot.Q) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    Q.Cast(enemy);
                }
                if (ECHECK && enemy.IsValidTarget(E.Range) && myHero.GetSpellDamage(enemy, SpellSlot.E) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    E.Cast(enemy);
                }
            }
        }

        private static void LastHit()
        {
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            var minion = ObjectManager.Get<Obj_AI_Minion>()
                    .Where(x => x.IsEnemy && x.IsValidTarget(Q.Range))
                    .OrderBy(x => x.Health)
                    .FirstOrDefault();
            bool QCHECK = FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue;

            if (QCHECK && myHero.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health && !minion.IsDead)
            {
                 Q.Cast(minion);
            }
        }

        private static float ComboDamage()
        {
            var target = TS.GetTarget(1200, DamageType.Magical);
            var ComboDMG = 0f;

            if (Q.IsReady())
                ComboDMG += myHero.GetSpellDamage(target, SpellSlot.Q);

            if (W.IsReady())
                ComboDMG += myHero.GetSpellDamage(target, SpellSlot.W);

            if (E.IsReady())
                ComboDMG += myHero.GetSpellDamage(target, SpellSlot.E);

            return ComboDMG;

        }

        private static void Q_Cast()
        {
            var target = TS.GetTarget(900, DamageType.Magical);
            var QPred = Q.GetPrediction(target);

            if (target.IsValidTarget(900))
            {
                Q.Cast(QPred.UnitPosition);
            }
        }

        private static void W_Cast()
        {
            var target = TS.GetTarget(600, DamageType.Magical);

            if (target.IsValidTarget(600))
            {
                W.Cast(target);
            }
        }

        private static void E_Cast()
        {
            var target = TS.GetTarget(600, DamageType.Magical);

            if (target.IsValidTarget(600))
            {
                E.Cast(target);
            }
        }

        private static void R_Cast()
        {
            if (myHero.HasBuff("ryzepassivecharged") || myHero.GetBuffCount("ryzepassivestack") == 4)
            {
                R.Cast();
            }
        }

        private static void AddonCombo()
        {
            var target = TS.GetTarget(900, DamageType.Magical);

            if (target.IsValidTarget(900))
            {
                if (W.IsReady())
                {
                    W_Cast();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    Q_Cast();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    E_Cast();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    R_Cast();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    W_Cast();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    Q_Cast();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    E_Cast();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    R_Cast();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    W_Cast();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    Q_Cast();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    E_Cast();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    R_Cast();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    W_Cast();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    Q_Cast();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    E_Cast();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    R_Cast();
                }
                if (!R.IsReady() && W.IsReady())
                {
                    W_Cast();
                }
                if (!W.IsReady() && Q.IsReady())
                {
                    Q_Cast();
                }
                if (!Q.IsReady() && E.IsReady())
                {
                    E_Cast();
                }
                if (!E.IsReady() && R.IsReady())
                {
                    R_Cast();
                }
            }
        }

        private static void SluttyCombo()
        {
            var target = TS.GetTarget(900, DamageType.Magical);
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            var QPred = Q.GetPrediction(target);
            bool StacksBuff = myHero.HasBuff("ryzepassivestack");
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            bool QCHECK = ComboMenu["SUQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = ComboMenu["SUW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = ComboMenu["SUE"].Cast<CheckBox>().CurrentValue;
            bool RCHECK = ComboMenu["SUR"].Cast<CheckBox>().CurrentValue;
            bool SRCHECK = ComboMenu["SUAR"].Cast<CheckBox>().CurrentValue;

            if (target.IsValidTarget(Q.Range))
            {
                if (Stacks <= 2 || !StacksBuff)
                {
                    if (target.IsValidTarget(Q.Range) && QCHECK && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(W.Range) && WCHECK && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(E.Range) && ECHECK && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady() && RCHECK)
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            if (SRCHECK && target.HasBuff("RyzeW"))
                                R.Cast();
                            if (!SRCHECK)
                                R.Cast();
                        }
                    }
                }
                if (Stacks == 3)
                {
                    if (target.IsValidTarget(Q.Range) && QCHECK && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && ECHECK && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (target.IsValidTarget(W.Range) && WCHECK && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (R.IsReady() && RCHECK)
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            if (SRCHECK && target.HasBuff("RyzeW"))
                                R.Cast();
                            if (!SRCHECK)
                                R.Cast();
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
                                R.Cast();
                        }
                    }
                }
                if (Stacks == 4)
                {
                    if (target.IsValidTarget(W.Range) && WCHECK && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(Q.Range) && QCHECK && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && ECHECK && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady() && RCHECK)
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            if (SRCHECK && target.HasBuff("RyzeW"))
                                R.Cast();
                            if (!SRCHECK)
                                R.Cast();
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
                                R.Cast();
                        }
                    }
                }
                if (Pasive)
                {
                    if (target.IsValidTarget(W.Range) && WCHECK && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(Q.Range) && QCHECK && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && ECHECK && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady() && RCHECK)
                    {
                        if (target.IsValidTarget(W.Range)
                            && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            if (SRCHECK && target.HasBuff("RyzeW"))
                                R.Cast();
                            if (!SRCHECK)
                                R.Cast();
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
                                R.Cast();
                        }
                    }
                }
            }
            else
            {
                if (WCHECK
                    && W.IsReady()
                    && target.IsValidTarget(W.Range))
                    W.Cast(target);

                if (QCHECK
                    && Q.IsReady()
                    && target.IsValidTarget(Q.Range))
                    Q.Cast(QPred.UnitPosition);

                if (ECHECK
                    && E.IsReady()
                    && target.IsValidTarget(E.Range))
                    E.Cast(target);
            }
            if (!R.IsReady() || Stacks != 4 || !RCHECK) return;

            if (Q.IsReady() || W.IsReady() || E.IsReady()) return;

            R.Cast();
        }
    }
}
