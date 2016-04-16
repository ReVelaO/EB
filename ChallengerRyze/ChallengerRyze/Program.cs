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

namespace ChallengerRyze
{
    class Program
    {
        public static Menu ComboMenu, DrawingsMenu, /*ItemsMenu, */HarassMenu, KSMenu, MiscMenu, ThemeMenu, FarmMenu, LaneMenu, JungleMenu, menu;
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

            menu = MainMenu.AddMenu("Challenger Ryze", "challengerryze");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");

            ComboMenu.AddGroupLabel("Combo Selector");
            var cs = ComboMenu.Add("css", new Slider("Combo Selector", 0, 0, 1));
            var co = new[] { "Addon Combo", "Slutty Combo" };
            cs.DisplayName = co[cs.CurrentValue];

            cs.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = co[changeArgs.NewValue];
                };

            ComboMenu.AddGroupLabel("Slutty Combo");
            ComboMenu.Add("SUQ", new CheckBox("Use Q"));
            ComboMenu.Add("SUW", new CheckBox("Use W"));
            ComboMenu.Add("SUE", new CheckBox("Use E"));
            ComboMenu.Add("SUR", new CheckBox("Use R"));
            ComboMenu.Add("SUAR", new CheckBox("Use R [Rooted Enemy]"));

            FarmMenu = menu.AddSubMenu("LastHit", "farmenu");

            FarmMenu.AddGroupLabel("LastHit Settings");
            FarmMenu.Add("LHQ", new CheckBox("Use Q"));
            FarmMenu.Add("LHM", new Slider("Mana", 55, 1, 100));

            LaneMenu = menu.AddSubMenu("Laneclear", "lanemenu");

            LaneMenu.AddGroupLabel("Laneclear Settings");
            LaneMenu.Add("LCQ", new CheckBox("Use Q"));
            LaneMenu.Add("LCW", new CheckBox("Use W"));
            LaneMenu.Add("LCE", new CheckBox("Use E"));
            LaneMenu.Add("LCR", new CheckBox("Use R"));

            JungleMenu = menu.AddSubMenu("Jungleclear", "junglemenu");

            JungleMenu.AddGroupLabel("Jungleclear Settings");
            JungleMenu.Add("JCQ", new CheckBox("Use Q"));
            JungleMenu.Add("JCW", new CheckBox("Use W"));
            JungleMenu.Add("JCE", new CheckBox("Use E"));
            JungleMenu.Add("JCR", new CheckBox("Use R"));

            HarassMenu = menu.AddSubMenu("Harass", "hsmenu");

            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("HSQ", new CheckBox("Use Q"));
            HarassMenu.Add("HSW", new CheckBox("Use W"));
            HarassMenu.Add("HSE", new CheckBox("Use E"));
            HarassMenu.Add("HSM", new Slider("Mana", 55, 1, 100));

            KSMenu = menu.AddSubMenu("Kill Steal (KS)", "ksmenu");

            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.Add("KSQ", new CheckBox("Auto Q"));
            KSMenu.Add("KSW", new CheckBox("Auto W"));
            KSMenu.Add("KSE", new CheckBox("Auto E"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");

            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            DrawingsMenu.Add("DWE", new CheckBox("Draw W + E"));
            DrawingsMenu.Add("DSTK", new CheckBox("Draw Stacks HUD"));

            /*ItemsMenu = menu.AddSubMenu("Items", "itemsmenu");

            ItemsMenu.AddGroupLabel("Items Settings");
            ItemsMenu.Add("US", new CheckBox("Use Seraph's Embrace"));
            ItemsMenu.Add("Vida", new Slider("HP %", 55, 1, 100));*/

            MiscMenu = menu.AddSubMenu("Misc", "miscmenu");

            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("Misc1", new CheckBox("Anti-Gapcloser [W Usage]"));
            MiscMenu.Add("Misc2", new CheckBox("Auto-Interrupt [W Usage]"));
            MiscMenu.AddGroupLabel("Addon Cast Helper");
            MiscMenu.Add("CAC", new CheckBox("Enable Addon Cast Helper"));
            var cx = MiscMenu.Add("csss", new Slider("Addon Cast Method", 0, 0, 1));
            var ca = new[] { "Mode: Kite", "Mode: To target" };
            cx.DisplayName = ca[cx.CurrentValue];

            cx.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = ca[changeArgs.NewValue];
                };

            ThemeMenu = menu.AddSubMenu("Theme Style", "themestyle");

            ThemeMenu.AddGroupLabel("Themes Settings");
            ThemeMenu.Add("te", new CheckBox("Enable Theme Style", false));
            var xs = ThemeMenu.Add("xss", new Slider("Theme Styles", 0, 0, 4));
            var xo = new[] { "Off", "Theme: Raven", "Theme: Academy", "Theme: Challenger", "Theme: Crystal" };
            xs.DisplayName = xo[xs.CurrentValue];

            xs.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
            {
                sender.DisplayName = xo[changeArgs.NewValue];
            };

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }

        static int GetPassiveBuff
        {
            get
            {
            	var data = myHero.GetBuff("ryzepassivestack");
                return data == null ? 0 : data.Count;
            }
        }
        
        static void Drawing_OnDraw(EventArgs args)
        {
            var themes = ThemeMenu["xss"].DisplayName;
            var Posicion = Drawing.WorldToScreen(myHero.Position);
            bool TCHECK = ThemeMenu["te"].Cast<CheckBox>().CurrentValue;
            
            
            
            if (myHero.HasBuff("ryzepassivestack") && !myHero.IsDead && DrawingsMenu["DSTK"].Cast<CheckBox>().CurrentValue)
            {
            	Drawing.DrawText(Posicion.X -70, Posicion.Y +30, Color.LightPink, "Stacks active: " + GetPassiveBuff, 34);
            }
            
            if (TCHECK)
            {
                switch (themes)
                {
                    case "Off":
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
            if (!TCHECK)
            {
                if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
                {
                    new Circle() { Color = Color.BlueViolet, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
                }
                if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
                {
                    new Circle() { Color = Color.BlueViolet, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
                }
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ComboMode();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    LastHit();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Harass();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    Laneclear();
                    break;
                case Orbwalker.ActiveModes.JungleClear:
                    Jungleclear();
                    break;
            }

            KS();
        }

        static void Gapcloser_OnGapCloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            bool WGCHECK = MiscMenu["Misc1"].Cast<CheckBox>().CurrentValue;
            if (W.IsReady() && WGCHECK)
            {
                W.Cast(sender);
            }
        }

        static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs args)
        {
            bool WICHECK = MiscMenu["Misc2"].Cast<CheckBox>().CurrentValue;
            if (W.IsReady() && WICHECK)
            {
                W.Cast(sender);
            }
        }

        static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args) //Hi, what are u doing here? :')
        {
            bool CACCHECK = MiscMenu["CAC"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Modes = MiscMenu["csss"].DisplayName;

            if (!CACCHECK) return;

            if (CACCHECK)
            {
                switch (Modes)
                {
                    case "Mode: Kite":
                        if (args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W || args.Slot == SpellSlot.E)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, myHero.ServerPosition - 50);
                        }
                        break;
                    case "Mode: To target":
                        if (args.Slot == SpellSlot.Q || args.Slot == SpellSlot.W || args.Slot == SpellSlot.E)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target.ServerPosition);
                        }
                        break;
                }
            }
        }
        
        static void ComboMode()
        {
            var options = ComboMenu["css"].DisplayName;
            switch (options)
            {
                case "Addon Combo":
                    WQER();
                    break;
                case "Slutty Combo":
                    SluttyCombo();
                    break;
            }
        }

        static void KS()
        {
            if (!Q.IsReady() && !W.IsReady() && !E.IsReady()) return;

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

        static void LastHit()
        {   
        	var QCHECK = FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue;
        	var MANA_VALUE = FarmMenu["LHM"].Cast<Slider>().CurrentValue;
            var minionlane = EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsEnemy && a.IsInRange(myHero.Position, Q.Range) && a.IsValid && !a.IsDead);
            foreach (var kenemy in minionlane)
            if (QCHECK && myHero.GetSpellDamage(kenemy, SpellSlot.Q) >= kenemy.Health && myHero.ManaPercent >= MANA_VALUE)
        	{
        		Q.Cast(kenemy);
        	}
        }

        static void Q_Cast(Obj_AI_Base target) //Hi, what are u doing here? :')
        {
            if (!Q.IsReady() || !Q.IsLearned || Q.IsOnCooldown) return;

            var QPred = Q.GetPrediction(target);
            if (target.IsValidTarget(900) && !target.HasBuff("RyzeW") && !myHero.HasBuff("ryzepassivecharged") && QPred.HitChance >= HitChance.Medium)
            {
                Q.Cast(target);
            }
            else if (target.IsValidTarget(900) && target.HasBuff("RyzeW") | myHero.HasBuff("ryzepassivecharged"))
            {
                Q.Cast(QPred.UnitPosition);
            }
        }

        static void R_Cast()
        {
            if (!R.IsReady() || !R.IsLearned || R.IsOnCooldown) return;
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            if (Pasive || Stacks == 4 && !Q.IsReady() | !W.IsReady() | !E.IsReady())
            {
                R.Cast();
            }
        }

        static void Harass()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target == null || !target.IsValid) return;

            bool QCHECK = HarassMenu["HSQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = HarassMenu["HSW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = HarassMenu["HSE"].Cast<CheckBox>().CurrentValue;
            var MANA_VALUE = HarassMenu["HSM"].Cast<Slider>().CurrentValue;
            if (!myHero.HasBuff("ryzepassivecharged"))
            {
                if (E.IsReady() && target.IsValidTarget(E.Range) && ECHECK && myHero.ManaPercent >= MANA_VALUE)
                {
                    E.Cast(target);
                }
                if (W.IsReady() && target.IsValidTarget(W.Range) && WCHECK && myHero.ManaPercent >= MANA_VALUE)
                {
                    W.Cast(target);
                }
                if (Q.IsReady() && target.IsValidTarget(Q.Range) && QCHECK && myHero.ManaPercent >= MANA_VALUE)
                {
                    Q.Cast(target);
                }
            }
            if (myHero.HasBuff("ryzepassivecharged"))
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
                if (E.IsReady() && target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                }
            }
        }

        static void Laneclear()
        {          
            bool QCHECK = LaneMenu["LCQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = LaneMenu["LCW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = LaneMenu["LCE"].Cast<CheckBox>().CurrentValue;
            bool RCHECK = LaneMenu["LCR"].Cast<CheckBox>().CurrentValue;
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            var minionlane = EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsEnemy && a.IsInRange(myHero.Position, Q.Range) && a.IsValid && !a.IsDead);
            foreach (var kenemy in minionlane)
	            {
	            	if (!Pasive)
		            {
		                if (Q.IsReady() && QCHECK)
		                {
		                    Q.Cast(kenemy);
		                }
		                if (!Q.IsReady() && E.IsReady() && ECHECK)
		                {
		                    E.Cast(kenemy);
		                }
		                if (!E.IsReady() && W.IsReady() && WCHECK)
		                {
		                    W.Cast(kenemy);
		                }
		                if (R.IsReady() && RCHECK)
		                {
		                    R_Cast();
		                }
		            }
	            if (Pasive)
	            {
	                if (Q.IsReady() && QCHECK)
	                {
	                    Q.Cast(kenemy);
	                }
	                if (!Q.IsReady() && E.IsReady() && ECHECK)
	                {
	                    E.Cast(kenemy);
	                }
	                if (!E.IsReady() && W.IsReady() && WCHECK)
	                {
	                    W.Cast(kenemy);
	                }
	                if (R.IsReady() && RCHECK)
	                {
	                    R.Cast();
	                }
	            }         
				
	        }
        }
        static void Jungleclear()
        {
            var jungle = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(a => a.Health).FirstOrDefault(a => a.IsValidTarget(Q.Range));
            bool QCHECK = JungleMenu["JCQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = JungleMenu["JCW"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = JungleMenu["JCE"].Cast<CheckBox>().CurrentValue;
            bool RCHECK = JungleMenu["JCR"].Cast<CheckBox>().CurrentValue;
            bool Pasive = myHero.HasBuff("ryzepassivecharged");		
                if (!Pasive)
	            {
	                if (Q.IsReady() && QCHECK)
	                {
	                    Q.Cast(jungle);
	                }
	                if (!Q.IsReady() && E.IsReady() && ECHECK)
	                {
	                    E.Cast(jungle);
	                }
	                if (!E.IsReady() && W.IsReady() && WCHECK)
	                {
	                    W.Cast(jungle);
	                }
	                if (R.IsReady() && RCHECK)
	                {
	                    R_Cast();
	                }
	            }
	            if (Pasive)
	            {
	                if (Q.IsReady() && QCHECK)
	                {
	                    Q.Cast(jungle);
	                }
	                if (!Q.IsReady() && E.IsReady() && ECHECK)
	                {
	                    E.Cast(jungle);
	                }
	                if (!E.IsReady() && W.IsReady() && WCHECK)
	                {
	                    W.Cast(jungle);
	                }
	                if (R.IsReady() && RCHECK)
	                {
	                    R.Cast();
	                }
	            }     
        }

        static void WQER()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);

            if (target == null || !target.IsValid) return;

            if (target.IsValidTarget(Q.Range))
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
                if (!W.IsReady() && Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    Q_Cast(target);
                }
                if (!Q.IsReady() && E.IsReady() && target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                }
                if (R.IsReady() && target.IsValidTarget(W.Range))
                {
                    R_Cast();
                }
            }
            if (myHero.HasBuff("ryzepassivecharged"))
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
                if (!W.IsReady() && Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    Q_Cast(target);
                }
                if (!Q.IsReady() && E.IsReady() && target.IsValidTarget(E.Range))
                {
                    E.Cast(target);
                }
                if (R.IsReady() && target.IsValidTarget(W.Range))
                {
                    R.Cast();
                }
            }
        }

        static void SluttyCombo()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            var QPred = Prediction.Position.PredictLinearMissile(target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MinValue, myHero.Position);
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