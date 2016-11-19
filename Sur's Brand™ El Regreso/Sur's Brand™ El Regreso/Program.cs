namespace Brand
{
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Spells;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using Color = System.Drawing.Color;
    internal class Program
    {
        static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1600, 60) { AllowedCollisionCount = 0 };
        static Spell.Skillshot W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 850, int.MaxValue, 250) { AllowedCollisionCount = -1 };
        static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 630);
        static Spell.Targeted R = new Spell.Targeted(SpellSlot.R, 750);
        static Spell.Targeted Ignite = new Spell.Targeted(SummonerSpells.Ignite.Slot, 600);
        static Menu menu, combomenu, lanemenu, killmenu, drawingsmenu;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }
        static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Brand)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Brand™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's Brand™", "Welcome back buddy!"), 20000);

            menu = MainMenu.AddMenu("BRAND", "index0");
            menu.AddGroupLabel("[FREE] Brand Addon by Surprise");

            combomenu = menu.AddSubMenu("Combo", "index1");
            combomenu.AddLabel("¿Which spells do you want to use?");
            combomenu.AddSeparator(14);
            combomenu.Add("q", new CheckBox("Use Q"));
            combomenu.AddSeparator(8);
            combomenu.Add("qm", new ComboBox("Q Mode:", 0, "Fast", "Stun"));
            combomenu.AddSeparator(8);
            combomenu.Add("w", new CheckBox("Use W"));
            combomenu.Add("e", new CheckBox("Use E"));
            combomenu.Add("r", new CheckBox("Use R"));
            combomenu.Add("rk", new CheckBox("Use R Killable"));
            combomenu.AddSeparator(8);
            combomenu.Add("minr", new Slider("Min. Enemies [R]", 2, 1, 5));

            lanemenu = menu.AddSubMenu("Laneclear", "index2");
            lanemenu.AddLabel("¿Which spells do you want to use?");
            lanemenu.AddSeparator(14);
            lanemenu.Add("q", new CheckBox("Use Q"));
            lanemenu.Add("w", new CheckBox("Use W"));
            lanemenu.Add("e", new CheckBox("Use E"));
            lanemenu.AddSeparator(14);
            lanemenu.AddLabel("¿Enable Mana-Manager?");
            lanemenu.AddSeparator(14);
            lanemenu.Add("mm", new CheckBox("Enable Mana-Manager"));
            lanemenu.AddSeparator(8);
            lanemenu.Add("mms", new Slider("Stop at {0}% Mana", 69, 1));
            lanemenu.AddSeparator(8);
            lanemenu.AddLabel("¿How many minions (minimum) do you want hit?");
            lanemenu.AddSeparator(8);
            lanemenu.Add("wmin", new Slider("Spell W: Hit {0} Minions", 3, 1, 6));
            lanemenu.AddSeparator(8);
            lanemenu.Add("emin", new Slider("Spell E: Hit {0} Minions", 3, 1, 6));

            killmenu = menu.AddSubMenu("Kill Steal", "index3");
            killmenu.AddLabel("¿Which spells do you want to use?");
            killmenu.AddSeparator(14);
            killmenu.Add("q", new CheckBox("Use Q"));
            killmenu.Add("w", new CheckBox("Use W"));
            killmenu.Add("e", new CheckBox("Use E"));
            killmenu.Add("i", new CheckBox("Use Ignite"));

            drawingsmenu = menu.AddSubMenu("Drawings", "index4");
            drawingsmenu.AddLabel("¿Which spells do you want to draw?");
            drawingsmenu.AddSeparator(14);
            drawingsmenu.Add("q", new CheckBox("Draw Q"));
            drawingsmenu.Add("w", new CheckBox("Draw W"));
            drawingsmenu.Add("e", new CheckBox("Draw E"));
            drawingsmenu.Add("r", new CheckBox("Draw R"));
            drawingsmenu.Add("i", new CheckBox("Draw Ignite"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }
        static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var MANACHECK = lanemenu["mm"].Cast<CheckBox>().CurrentValue;
                var MANASLI = lanemenu["mms"].Cast<Slider>().CurrentValue;
                if (MANACHECK)
                {
                    if (Player.Instance.ManaPercent > MANASLI)
                    {
                        Laneclear();
                    }
                }
                else
                {
                    Laneclear();
                }
            }
            KillSteal();
        }
        static void Drawing_OnDraw(EventArgs args)
        {
            var QCHECK = drawingsmenu["q"].Cast<CheckBox>().CurrentValue;
            var WCHECK = drawingsmenu["w"].Cast<CheckBox>().CurrentValue;
            var ECHECK = drawingsmenu["e"].Cast<CheckBox>().CurrentValue;
            var RCHECK = drawingsmenu["r"].Cast<CheckBox>().CurrentValue;
            var ICHECK = drawingsmenu["i"].Cast<CheckBox>().CurrentValue;

            if (QCHECK && Q.IsReady())
            {
                Q.DrawRange(Color.FromArgb(130, Color.DarkOrange));
            }
            if (WCHECK && W.IsReady())
            {
                W.DrawRange(Color.FromArgb(130, Color.OrangeRed));
            }
            if (ECHECK && E.IsReady())
            {
                E.DrawRange(Color.FromArgb(130, Color.Orange));
            }
            if (RCHECK && R.IsReady())
            {
                R.DrawRange(Color.FromArgb(130, Color.Red));
            }
            if (ICHECK && HasIgnite && Ignite.IsReady())
            {
                Ignite.DrawRange(Color.FromArgb(130, Color.DarkRed));
            }
        }
        static float BestTarget()
        {
            float r = 0;
            if (Q.IsReady() | W.IsReady())
            {
                r = 1100;
            }
            else
            {
                r = R.Range;
            }
            return r;
        }
        static bool IsBlazed(Obj_AI_Base e) => e.Buffs.Any(a => a.Name.ToLower().Equals("brandablaze"));
        static bool HasIgnite => SummonerSpells.PlayerHas(SummonerSpellsEnum.Ignite);
        static float GetIgniteDMG(Obj_AI_Base e) => DamageLibrary.GetSummonerSpellDamage(Player.Instance, e, DamageLibrary.SummonerSpells.Ignite);
        static bool Debuffed(Obj_AI_Base e) => e.HasBuffOfType(BuffType.Slow)
            || e.HasBuffOfType(BuffType.Snare)
            || e.HasBuffOfType(BuffType.Stun)
            || e.HasBuffOfType(BuffType.Taunt)
            || e.HasBuffOfType(BuffType.Polymorph)
            || e.HasBuffOfType(BuffType.Fear)
            || e.HasBuffOfType(BuffType.Charm);
        static void Combo()
        {
            var QCHECK = combomenu["q"].Cast<CheckBox>().CurrentValue;
            var QMODE = combomenu["qm"].Cast<ComboBox>().CurrentValue;
            var WCHECK = combomenu["w"].Cast<CheckBox>().CurrentValue;
            var ECHECK = combomenu["e"].Cast<CheckBox>().CurrentValue;
            var RCHECK = combomenu["r"].Cast<CheckBox>().CurrentValue;
            var RKCHECK = combomenu["rk"].Cast<CheckBox>().CurrentValue;
            var RKHIT = combomenu["minr"].Cast<Slider>().CurrentValue;
            var e = TargetSelector.GetTarget(BestTarget(), DamageType.Magical);
            if (e != null && e.IsValidTarget(BestTarget()))
            {
                if (E.IsReady() && ECHECK)
                {
                    E.Cast(e);
                }
                if (Q.IsReady() && QCHECK)
                {
                    var p = Q.GetPrediction(e);
                    if (p.HitChance >= HitChance.High)
                    {
                        switch (QMODE)
                        {
                            case 0:
                                Q.Cast(p.UnitPosition);
                                break;
                            case 1:
                                if (IsBlazed(e))
                                {
                                    Q.Cast(p.CastPosition);
                                }
                                break;
                        }
                        if (!W.IsReady() && !E.IsReady())
                        {
                            Q.Cast(p.UnitPosition);
                        }
                    }
                }
                if (W.IsReady() && WCHECK)
                {
                    var p = W.GetPrediction(e);
                    if (p.HitChance >= HitChance.High)
                    {
                        switch (Debuffed(e))
                        {
                            case true:
                                W.Cast(p.CastPosition);
                                break;
                            case false:
                                W.Cast(p.UnitPosition);
                                break;
                        }
                    }
                }
                if (R.IsReady() && RCHECK)
                {
                    if (RKCHECK)
                    {
                        if (HPrediction(e, R.CastDelay) < DamageBySlot(e, SpellSlot.R))
                        {
                            R.Cast(e);
                        }
                    }
                    if (e.CountEnemiesInRange(400) >= RKHIT && e.HealthPercent < 95)
                    {
                        R.Cast(e);
                    }
                }
            }
        }
        static void Laneclear()
        {
            var QCHECK = lanemenu["q"].Cast<CheckBox>().CurrentValue;
            var WCHECK = lanemenu["w"].Cast<CheckBox>().CurrentValue;
            var ECHECK = lanemenu["e"].Cast<CheckBox>().CurrentValue;
            var WHIT = lanemenu["wmin"].Cast<Slider>().CurrentValue;
            var EHIT = lanemenu["emin"].Cast<Slider>().CurrentValue;

            if (QCHECK)
            {
                var m0 = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(BestTarget()));
                if (m0 != null)
                {
                    if (Q.IsReady())
                    {
                        var p = Q.GetBestLinearCastPosition(m0);
                        if (p.HitNumber == 1)
                        {
                            Q.Cast(p.CastPosition);
                        }
                    }
                }
            }
            if (ECHECK)
            {
                var m1 = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(BestTarget()) && IsBlazed(x)).FirstOrDefault();
                if (m1 != null)
                {
                    if (E.IsReady())
                    {
                        if (m1.CountEnemyMinionsInRange(300) >= EHIT)
                        {
                            E.Cast(m1);
                        }
                    }
                }
            }
            if (WCHECK)
            {
                var m2 = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(BestTarget())).OrderBy(o=>o.Health);
                if (m2 != null)
                {
                    if (W.IsReady())
                    {
                        var p = W.GetBestCircularCastPosition(m2);
                        if (p.HitNumber >= WHIT)
                        {
                            W.Cast(p.CastPosition);
                        }
                    }
                }
            }
        }
        static void KillSteal()
        {
            var QCHECK = killmenu["q"].Cast<CheckBox>().CurrentValue;
            var WCHECK = killmenu["w"].Cast<CheckBox>().CurrentValue;
            var ECHECK = killmenu["e"].Cast<CheckBox>().CurrentValue;
            var ICHECK = killmenu["i"].Cast<CheckBox>().CurrentValue;

            var r = EntityManager.Heroes.Enemies.Find(x => x.IsValidTarget(BestTarget()));
            if (r != null)
            {
                if (QCHECK && Q.IsReady())
                {
                    var p = Q.GetPrediction(r);
                    if (p.HitChance >= HitChance.High 
                        && HPrediction(r, Q.CastDelay) < DamageBySlot(r, SpellSlot.Q))
                    {
                        switch (Debuffed(r))
                        {
                            case true:
                                Q.Cast(p.CastPosition);
                                break;
                            case false:
                                Q.Cast(p.UnitPosition);
                                break;
                        }
                    }
                }
                if (WCHECK && W.IsReady())
                {
                    var p = W.GetPrediction(r);
                    if (p.HitChance >= HitChance.High 
                        && HPrediction(r, W.CastDelay) < DamageBySlot(r, SpellSlot.W))
                    {
                        switch (Debuffed(r))
                        {
                            case true:
                                W.Cast(p.CastPosition);
                                break;
                            case false:
                                W.Cast(p.UnitPosition);
                                break;
                        }
                    }
                }
                if (ECHECK && E.IsReady())
                {
                    if (HPrediction(r, E.CastDelay) < DamageBySlot(r, SpellSlot.E))
                    {
                        E.Cast(r);
                    }
                }
                if (ICHECK && HasIgnite && Ignite.IsReady())
                {
                    if (HPrediction(r, Ignite.CastDelay) < GetIgniteDMG(r))
                    {
                        Ignite.Cast(r);
                    }
                }
            }
        }
        static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            var Damage = 0f;
            if (slot == SpellSlot.Q)
            {
                if (Q.IsReady())
                    Damage += new float[] { 80, 110, 140, 170, 200 }[Player.GetSpell(slot).Level - 1] +
                              0.55f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.W)
            {
                if (W.IsReady())
                    Damage += new float[] { 75, 120, 165, 210, 255 }[Player.GetSpell(slot).Level - 1] +
                              0.6f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.E)
            {
                if (E.IsReady())
                    Damage += new float[] { 70, 90, 110, 130, 150 }[Player.GetSpell(slot).Level - 1] +
                              0.35f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.R)
            {
                if (R.IsReady())
                    Damage += new float[] { 100, 200, 300 }[Player.GetSpell(slot).Level - 1] +
                              0.25f * Player.Instance.FlatMagicDamageMod;
            }
            return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
        static float HPrediction(Obj_AI_Base e, int d) => Prediction.Health.GetPrediction(e, d);
    }
}
