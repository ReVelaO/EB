using System;
using System.Linq;
namespace Lux
{
    #region Elobuddy References
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Spells;
    #endregion
    internal class Program
    {
        #region Spells & Menu var
        static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 1175, SkillShotType.Linear, 250, 1200, 70) { AllowedCollisionCount = 0 };
        static Spell.Skillshot W = new Spell.Skillshot(SpellSlot.W, 1075, SkillShotType.Linear, 250, 1300, 95) { AllowedCollisionCount = -1 };
        static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Circular, 250, 1300, 330) { AllowedCollisionCount = -1 };
        static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 3300, SkillShotType.Linear, 1000, int.MaxValue, 190) { AllowedCollisionCount = -1 };
        static Spell.Targeted Ignite = new Spell.Targeted(SummonerSpells.Ignite.Slot, 600);
        static Menu menu, qmenu, wmenu, emenu, rmenu, predmenu;
        #endregion End S&M
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }
        static readonly string[] hitchances = { "Low", "Medium", "High" };
        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Lux)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            #region Menu
            Chat.Print("[<font color='#29EB8A'>Sur's Series:</font> <font color='#FAF092'>Lux</font> successfully loaded. <font color='#FFFFFF'>Enjoy!</font>]");
            Notifications.Show(new SimpleNotification("Sur's Series: Lux", "Welcome back buddy!"), 20000);
            menu = MainMenu.AddMenu("Lux", "index0");
            menu.AddLabel("Sur's Series: Lux", 24);
            menu.AddLabel("[FREE] Lux Addon by Surprise", 16);
            qmenu = menu.AddSubMenu("Q Settings", "index1");
            qmenu.AddLabel("¿Use Q in which modes?");
            qmenu.AddSeparator(14);
            qmenu.Add("cq", new CheckBox("Combo"));
            qmenu.Add("lq", new CheckBox("Laneclear"));
            qmenu.Add("jq", new CheckBox("Jungleclear"));
            qmenu.AddSeparator(14);
            qmenu.AddLabel("¿Enable Auto Q?");
            qmenu.AddSeparator(14);
            qmenu.Add("autoq", new CheckBox("Auto Q"));
            qmenu.AddSeparator(14);
            qmenu.AddLabel("¿Auto Q in which situations?");
            qmenu.AddSeparator(14);
            qmenu.Add("qkill", new CheckBox("Killable"));
            qmenu.Add("qdash", new CheckBox("Dash"));
            qmenu.Add("qstun", new CheckBox("Stun"));
            qmenu.Add("qslow", new CheckBox("Slow"));
            qmenu.Add("qsnare", new CheckBox("Snare"));
            qmenu.Add("qtaunt", new CheckBox("Taunt"));

            wmenu = menu.AddSubMenu("W Settings", "index2");
            wmenu.AddLabel("¿Auto W in which modes?");
            wmenu.AddSeparator(14);
            wmenu.Add("mew", new CheckBox("Me"));
            wmenu.Add("allyw", new CheckBox("Ally"));
            wmenu.AddSeparator(14);
            wmenu.AddLabel("¿Enable Auto W?");
            wmenu.AddSeparator(14);
            wmenu.Add("autow", new CheckBox("Auto W"));
            wmenu.AddSeparator(14);
            wmenu.AddLabel("Me");
            wmenu.Add("mehpw", new Slider("At {0}% HP", 70, 1));
            wmenu.AddSeparator(14);
            wmenu.AddLabel("Ally");
            wmenu.Add("allyhpw", new Slider("At {0}% HP", 70, 1));
            wmenu.AddSeparator(14);
            wmenu.AddLabel("¿Auto W in which ally situations?");
            wmenu.AddSeparator(14);
            wmenu.Add("wstun", new CheckBox("Stun"));
            wmenu.Add("wslow", new CheckBox("Slow"));
            wmenu.Add("wsnare", new CheckBox("Snare"));
            wmenu.Add("wtaunt", new CheckBox("Taunt"));

            emenu = menu.AddSubMenu("E Settings", "index3");
            emenu.AddLabel("¿Use E in which modes?");
            emenu.AddSeparator(14);
            emenu.Add("ce", new CheckBox("Combo"));
            emenu.Add("le", new CheckBox("Laneclear"));
            emenu.Add("je", new CheckBox("Jungleclear"));
            emenu.AddSeparator(14);
            emenu.AddLabel("¿Enable Auto E?");
            emenu.AddSeparator(14);
            emenu.Add("autoe", new CheckBox("Auto E"));
            emenu.AddSeparator(14);
            emenu.AddLabel("¿Auto E in which situations?");
            emenu.AddSeparator(14);
            emenu.Add("ekill", new CheckBox("Killable"));
            emenu.Add("edash", new CheckBox("Dash"));
            emenu.Add("estun", new CheckBox("Stun"));
            emenu.Add("eslow", new CheckBox("Slow"));
            emenu.Add("esnare", new CheckBox("Snare"));
            emenu.Add("etaunt", new CheckBox("Taunt"));

            rmenu = menu.AddSubMenu("R Settings", "index4");
            rmenu.AddLabel("¿Use R in which modes?");
            rmenu.AddSeparator(14);
            rmenu.Add("cr", new CheckBox("Combo"));
            rmenu.AddSeparator(14);
            rmenu.AddLabel("¿Enable Auto R?");
            rmenu.AddSeparator(14);
            rmenu.Add("aar", new CheckBox("Auto R"));
            rmenu.AddSeparator(14);
            rmenu.AddLabel("¿Auto R in which situations?");
            rmenu.AddSeparator(14);
            rmenu.Add("rkill", new CheckBox("Killable"));
            rmenu.Add("rbaron", new CheckBox("Baron/Herald"));
            rmenu.Add("rdragon", new CheckBox("Dragons"));
            rmenu.Add("rbuffs", new CheckBox("Buffs Blue/Red"));

            predmenu = menu.AddSubMenu("Hitchances", "index5");
            predmenu.AddLabel("Please, choose spells hitchances below");
            predmenu.AddSeparator(14);
            var qp = predmenu.Add("qpreda", new Slider("Hitchance Q", 2, 0, 2));
            qp.OnValueChange += delegate
            {
                qp.DisplayName = "Hitchance: " + hitchances[qp.CurrentValue];
            };
            qp.DisplayName = "Hitchance: " + hitchances[qp.CurrentValue];
            predmenu.AddSeparator(14);
            var qw = predmenu.Add("wpreda", new Slider("Hitchance W", 2, 0, 2));
            qw.OnValueChange += delegate
            {
                qw.DisplayName = "Hitchance: " + hitchances[qw.CurrentValue];
            };
            qw.DisplayName = "Hitchance: " + hitchances[qw.CurrentValue];
            predmenu.AddSeparator(14);
            var qe = predmenu.Add("epreda", new Slider("Hitchance E", 2, 0, 2));
            qe.OnValueChange += delegate
            {
                qe.DisplayName = "Hitchance: " + hitchances[qe.CurrentValue];
            };
            qe.DisplayName = "Hitchance: " + hitchances[qe.CurrentValue];
            #endregion End Menu
            Game.OnTick += Game_OnTick;
            Dash.OnDash += delegate (Obj_AI_Base sender, Dash.DashEventArgs dash)
            {
                if (qmenu["qdash"].Cast<CheckBox>().CurrentValue)
                {
                    if (sender.IsEnemy)
                    {
                        if (InRange(sender, Q.Range) && Q.IsReady())
                        {
                            Q.Cast(dash.EndPos);
                        }
                    }
                }
            };
        }
        static bool InRange(Obj_AI_Base e, float range) => e.IsInRange(Player.Instance, range);
        static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                #region Combo
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (target != null && !target.IsInvulnerable)
                {
                    if (qmenu["cq"].Cast<CheckBox>().CurrentValue)
                    {
                        var prediction = Q.GetPrediction(target);
                        if (Q.IsReady()
                            && prediction.HitChance >= hitq() && InRange(target, Q.Range))
                        {
                            Q.Cast(prediction.CastPosition);
                        }
                    }
                    if (emenu["ce"].Cast<CheckBox>().CurrentValue)
                    {
                        var prediction = E.GetPrediction(target);
                        if (E.IsReady()
                            && prediction.HitChance >= hite() && InRange(target, E.Range))
                        {
                            E.Cast(prediction.CastPosition);
                        }
                    }
                    if (rmenu["cr"].Cast<CheckBox>().CurrentValue)
                    {
                        var sum = DamageBySlot(target, SpellSlot.Q)
                            + DamageBySlot(target, SpellSlot.E)
                            + PassiveDMG(target);
                        if (HPrediction(target, 250) < sum) { return; }
                        else
                        {
                            var prediction = R.GetPrediction(target);
                            if (InRange(target, R.Range) && R.IsReady()
                                && prediction.HitChance >= HitChance.High
                                && HPrediction(target, R.CastDelay) < DamageBySlot(target, SpellSlot.R))
                            {
                                R.Cast(prediction.CastPosition);
                            }
                        }
                    }
                }
                #endregion End Combo
            }
            AutoQ();
            AutoE();
            AutoW();
            Special();
        }
        #region AutoQ
        static void AutoQ()
        {
            if (qmenu["autoq"].Cast<CheckBox>().CurrentValue)
            {
                if (qmenu["qkill"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, Q.Range)
                    && HPrediction(f, Q.CastDelay) < DamageBySlot(f, SpellSlot.Q)
                    && !f.IsInvulnerable && !f.IsDead);
                    if (random != null)
                    {
                        var prediction = Q.GetPrediction(random);
                        if (Q.IsReady() && prediction.HitChance >= HitChance.High)
                        {
                            Q.Cast(prediction.CastPosition);
                        }
                    }
                }
                if (qmenu["qstun"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, Q.Range)
                    && f.HasBuffOfType(BuffType.Stun));
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            Q.Cast(random);
                        }
                    }
                }
                if (qmenu["qslow"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, Q.Range)
                    && f.HasBuffOfType(BuffType.Slow));
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            Q.Cast(random);
                        }
                    }
                }
                if (qmenu["qsnare"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, Q.Range)
                    && f.HasBuffOfType(BuffType.Snare));
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            Q.Cast(random);
                        }
                    }
                }
                if (qmenu["qtaunt"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, Q.Range)
                    && f.HasBuffOfType(BuffType.Taunt));
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            Q.Cast(random);
                        }
                    }
                }
            }
        }
        #endregion End AutoQ
        #region AutoE
        static void AutoE()
        {
            if (emenu["autoe"].Cast<CheckBox>().CurrentValue)
            {
                if (emenu["ekill"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, E.Range)
                    && HPrediction(f, E.CastDelay) < DamageBySlot(f, SpellSlot.E)
                    && !f.IsInvulnerable && !f.IsDead);
                    if (random != null)
                    {
                        var prediction = E.GetPrediction(random);
                        if (E.IsReady() && prediction.HitChance >= HitChance.High)
                        {
                            E.Cast(prediction.CastPosition);
                        }
                    }
                }
                if (emenu["estun"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, E.Range)
                    && f.HasBuffOfType(BuffType.Stun));
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            E.Cast(random);
                        }
                    }
                }
                if (emenu["eslow"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, E.Range)
                    && f.HasBuffOfType(BuffType.Slow));
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            E.Cast(random);
                        }
                    }
                }
                if (emenu["esnare"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, E.Range)
                    && f.HasBuffOfType(BuffType.Snare));
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            E.Cast(random);
                        }
                    }
                }
                if (emenu["etaunt"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => InRange(f, E.Range)
                    && f.HasBuffOfType(BuffType.Taunt));
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            E.Cast(random);
                        }
                    }
                }
            }
        }
        #endregion End AutoE
        #region AutoW
        static void AutoW()
        {
            if (wmenu["mew"].Cast<CheckBox>().CurrentValue)
            {
                if (W.IsReady() && Player.Instance.CountEnemiesInRange(800) >= 1 || Player.Instance.IsUnderTurret() 
                    && ((HPrediction(Player.Instance, W.CastDelay)/Player.Instance.TotalHealth()) * 100) <= wmenu["mehpw"].Cast<Slider>().CurrentValue)
                {
                    W.Cast(Player.Instance);
                }
            }
            if (wmenu["allyw"].Cast<CheckBox>().CurrentValue)
            {
                var a = EntityManager.Heroes.Allies.Find(c => InRange(c, W.Range) && ((HPrediction(c, W.CastDelay) / c.TotalHealth()) * 100) <= wmenu["allyhpw"].Cast<Slider>().CurrentValue);
                if (a != null)
                {
                    var prediction = W.GetPrediction(a);
                    if (W.IsReady() && InRange(a, W.Range)
                        && prediction.HitChance >= hitw() && a.CountEnemiesInRange(800) >= 1)
                    {
                        W.Cast(prediction.CastPosition);
                    }
                }
                if (wmenu["wstun"].Cast<CheckBox>().CurrentValue)
                {
                    var ally = EntityManager.Heroes.Allies.Find(c => InRange(c, W.Range) && c.HasBuffOfType(BuffType.Stun) && c.CountEnemiesInRange(800) >= 1);
                    if (ally != null)
                    {
                        var prediction = W.GetPrediction(ally);
                        if (W.IsReady() && prediction.HitChance >= hitw())
                        {
                            W.Cast(prediction.CastPosition);
                        }
                    }
                }
                if (wmenu["wslow"].Cast<CheckBox>().CurrentValue)
                {
                    var ally = EntityManager.Heroes.Allies.Find(c => InRange(c, W.Range) && c.HasBuffOfType(BuffType.Slow) && c.CountEnemiesInRange(800) >= 1);
                    if (ally != null)
                    {
                        var prediction = W.GetPrediction(ally);
                        if (W.IsReady() && prediction.HitChance >= hitw())
                        {
                            W.Cast(prediction.CastPosition);
                        }
                    }
                }
                if (wmenu["wsnare"].Cast<CheckBox>().CurrentValue)
                {
                    var ally = EntityManager.Heroes.Allies.Find(c => InRange(c, W.Range) && c.HasBuffOfType(BuffType.Snare) && c.CountEnemiesInRange(800) >= 1);
                    if (ally != null)
                    {
                        var prediction = W.GetPrediction(ally);
                        if (W.IsReady() && prediction.HitChance >= hitw())
                        {
                            W.Cast(prediction.CastPosition);
                        }
                    }
                }
                if (wmenu["wtaunt"].Cast<CheckBox>().CurrentValue)
                {
                    var ally = EntityManager.Heroes.Allies.Find(c => InRange(c, W.Range) && c.HasBuffOfType(BuffType.Taunt) && c.CountEnemiesInRange(800) >= 1);
                    if (ally != null)
                    {
                        var prediction = W.GetPrediction(ally);
                        if (W.IsReady() && prediction.HitChance >= hitw())
                        {
                            W.Cast(prediction.CastPosition);
                        }
                    }
                }
            }
        }
        #endregion End AutoW
        #region MonsterList
        static readonly string[] Dragons = { "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air", "SRU_Dragon_Elder" };
        static readonly string[] Barons = { "SRU_Baron", "SRU_RiftHerald" };
        static readonly string[] Buffs = { "SRU_Red", "SRU_Blue" };
        #endregion
        #region AutoR
        static void AutoR()
        {
            if (rmenu["aar"].Cast<CheckBox>().CurrentValue)
            {
                if (rmenu["rkill"].Cast<CheckBox>().CurrentValue)
                {
                    var rdm = EntityManager.Heroes.Enemies.Find(f => InRange(f, R.Range)
                    && HPrediction(f, R.CastDelay) < DamageBySlot(f, SpellSlot.R));
                    if (rdm != null && !rdm.IsInvulnerable)
                    {
                        var prediction = R.GetPrediction(rdm);
                        if (prediction.HitChance >= HitChance.High && R.IsReady())
                        {
                            R.Cast(prediction.CastPosition);
                        }
                    }
                }
                if (rmenu["rbaron"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var mob in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(w => InRange(w, R.Range)
                     && HPrediction(w, R.CastDelay) < DamageBySlot(w, SpellSlot.R)
                     && w.IsMonster && Barons.Contains(w.BaseSkinName)))
                    {
                        if (mob != null)
                        {
                            if (R.IsReady())
                            {
                                R.Cast(mob.Position);
                            }
                        }
                    }
                }
                if (rmenu["rdragon"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var mob in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(w => InRange(w, R.Range)
                     && HPrediction(w, R.CastDelay) < DamageBySlot(w, SpellSlot.R)
                     && w.IsMonster && Dragons.Contains(w.BaseSkinName)))
                    {
                        if (mob != null)
                        {
                            if (R.IsReady())
                            {
                                R.Cast(mob.Position);
                            }
                        }
                    }
                }
                if (rmenu["rbuffs"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var mob in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(w => InRange(w, R.Range)
                     && HPrediction(w, R.CastDelay) < DamageBySlot(w, SpellSlot.R)
                     && w.IsMonster && Buffs.Contains(w.BaseSkinName)))
                    {
                        if (mob != null)
                        {
                            if (R.IsReady())
                            {
                                R.Cast(mob.Position);
                            }
                        }
                    }
                }
                #region Special R
                var random = EntityManager.Heroes.Enemies.Where(f => InRange(f, R.Range) 
                && HPrediction(f, R.CastDelay) < DamageBySlot(f, SpellSlot.R));
                var rgodly = R.GetBestLinearCastPosition(random);
                if (rgodly.HitNumber >= 3 && R.IsReady())
                {
                    R.Cast(rgodly.CastPosition);
                }
                #endregion End Special R
            }
        }
        #endregion End AutoR
        #region Extensions
        static float HPrediction(Obj_AI_Base e, int spellcastdelay)
        {
            return Prediction.Health.GetPrediction(e, spellcastdelay);
        }
        static HitChance hitq()
        {
            var value = predmenu["qpreda"].Cast<Slider>().CurrentValue;
            if (value == 0)
                return HitChance.Low;
            if (value == 1)
                return HitChance.Medium;
            if (value == 2)
                return HitChance.High;
            return HitChance.Medium;
        }
        static HitChance hitw()
        {
            var value = predmenu["wpreda"].Cast<Slider>().CurrentValue;
            if (value == 0)
                return HitChance.Low;
            if (value == 1)
                return HitChance.Medium;
            if (value == 2)
                return HitChance.High;
            return HitChance.Medium;
        }
        static HitChance hite()
        {
            var value = predmenu["epreda"].Cast<Slider>().CurrentValue;
            if (value == 0)
                return HitChance.Low;
            if (value == 1)
                return HitChance.Medium;
            if (value == 2)
                return HitChance.High;
            return HitChance.Medium;
        }
        static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            var damage = 0f;
            if (slot == SpellSlot.Q)
            {
                if (Q.IsReady())
                    damage += new float[] { 50, 100, 150, 200, 250 }[Player.GetSpell(slot).Level - 1] +
                              0.70f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.E)
            {
                if (E.IsReady())
                    damage += new float[] { 60, 105, 150, 195, 240 }[Player.GetSpell(slot).Level - 1] +
                              0.60f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.R)
            {
                if (R.IsReady())
                    damage += new float[] { 300, 400, 500 }[Player.GetSpell(slot).Level - 1] +
                              0.75f * Player.Instance.FlatMagicDamageMod;
            }
            return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical, damage);
        }
        static float PassiveDMG(Obj_AI_Base e)
        {
            var auto = Player.Instance.GetAutoAttackDamage(e);
            var passive = (10 * Player.Instance.Level) + (0.20f * Player.Instance.FlatMagicDamageMod);
            var sum = auto + passive;
            return Player.Instance.CalculateDamageOnUnit(e, DamageType.Mixed, sum);
        }
        #endregion End Extensions
        #region Auto Spells if Hit 5
        static void Special()
        {
            var xix = EntityManager.Heroes.Enemies.Where(f => InRange(f, 1200));
            var qgod = Q.GetBestLinearCastPosition(xix);
            var egod = E.GetBestCircularCastPosition(xix);
            var rgod = R.GetBestLinearCastPosition(xix);
            if (xix != null)
            {
                if (qgod.HitNumber == 2 && egod.HitNumber == 5 && rgod.HitNumber == 5 
                    && Q.IsReady() && E.IsReady() && R.IsReady())
                {
                    Q.Cast(qgod.CastPosition);
                    E.Cast(egod.CastPosition);
                    R.Cast(rgod.CastPosition);
                }
            }
        }
        #endregion End asih5
    }
}