namespace Zyra
{
    #region EB References
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Spells;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Enumerations;
    using Color = System.Drawing.Color;
    #endregion End EB
    internal class Program
    {
        #region Spells var & Menu
        static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear, 850, int.MaxValue, 85) { AllowedCollisionCount = -1 };
        static Spell.SimpleSkillshot W = new Spell.SimpleSkillshot(SpellSlot.W, 850);
        static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Linear, 250, 1400, 70) { AllowedCollisionCount = -1 };
        static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 500, 2000, 525) { AllowedCollisionCount = -1 };
        static Spell.Targeted Ignite = new Spell.Targeted(SummonerSpells.Ignite.Slot, 600);
        static bool InRange(Obj_AI_Base e, float range) => e.IsInRange(Player.Instance, range);
        static Menu menu, qmenu, wmenu, emenu, rmenu, igmenu, predmenu;
        static readonly string[] hitchances = { "Low", "Medium", "High" };
        #endregion End
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }
        static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Zyra)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Zyra™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's Zyra™", "Welcome back buddy!"), 20000);
            menu = MainMenu.AddMenu("ZYRA", "index0");
            menu.AddGroupLabel("[FREE] Zyra Addon by Surprise");
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
            qmenu.Add("qslow", new CheckBox("Slow", false));
            qmenu.Add("qsnare", new CheckBox("Snare", false));
            qmenu.Add("qtaunt", new CheckBox("Taunt"));
            qmenu.AddSeparator(14);
            qmenu.AddLabel("¿Draw Q Range?");
            qmenu.AddSeparator(14);
            qmenu.Add("qdraw", new CheckBox("Draw"));

            wmenu = menu.AddSubMenu("W Settings", "index2");
            wmenu.AddLabel("¿Draw W Range?");
            wmenu.AddSeparator(14);
            wmenu.Add("wdraw", new CheckBox("Draw"));

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
            emenu.Add("eslow", new CheckBox("Slow", false));
            emenu.Add("esnare", new CheckBox("Snare", false));
            emenu.Add("etaunt", new CheckBox("Taunt"));
            emenu.AddSeparator(14);
            emenu.AddLabel("¿Draw E Range?");
            emenu.AddSeparator(14);
            emenu.Add("edraw", new CheckBox("Draw"));

            rmenu = menu.AddSubMenu("R Settings", "index4");
            rmenu.AddLabel("¿Draw R Range?");
            rmenu.AddSeparator(14);
            rmenu.Add("rdraw", new CheckBox("Draw"));

            igmenu = menu.AddSubMenu("Ignite Settings", "index5");
            igmenu.AddLabel("¿Auto Ignite?");
            igmenu.AddSeparator(14);
            igmenu.Add("igdo", new CheckBox("Auto Ignite"));

            predmenu = menu.AddSubMenu("Hitchances", "index6");
            predmenu.AddLabel("Please, choose spells hitchances below");
            predmenu.AddSeparator(14);
            var qp = predmenu.Add("qpreda", new Slider("Hitchance Q", 2, 0, 2));
            qp.OnValueChange += delegate
            {
                qp.DisplayName = "Hitchance Q: " + hitchances[qp.CurrentValue];
            };
            qp.DisplayName = "Hitchance Q: " + hitchances[qp.CurrentValue];
            predmenu.AddSeparator(14);
            var qe = predmenu.Add("epreda", new Slider("Hitchance E", 2, 0, 2));
            qe.OnValueChange += delegate
            {
                qe.DisplayName = "Hitchance E: " + hitchances[qe.CurrentValue];
            };
            qe.DisplayName = "Hitchance E: " + hitchances[qe.CurrentValue];

            Game.OnTick += Game_OnCombo;
            Game.OnTick += Game_OnIgnite;
            Game.OnTick += Game_OnExtraTicks;
            Game.OnTick += Game_OnLaneclear;
            Game.OnTick += Game_OnJungleclear;
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
                if (emenu["edash"].Cast<CheckBox>().CurrentValue)
                {
                    if (sender.IsEnemy)
                    {
                        if (InRange(sender, E.Range) && E.IsReady())
                        {
                            E.Cast(dash.EndPos);
                        }
                    }
                }
            };
            Drawing.OnDraw += delegate
            {
                if (qmenu["qdraw"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady()) { Q.DrawRange(Color.FromArgb(130, Color.LightGreen)); }
                }
                if (wmenu["wdraw"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady()) { W.DrawRange(Color.FromArgb(130, Color.LightPink)); }
                }
                if (emenu["edraw"].Cast<CheckBox>().CurrentValue)
                {
                    if (E.IsReady()) { E.DrawRange(Color.FromArgb(130, Color.ForestGreen)); }
                }
                if (rmenu["rdraw"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady()) { R.DrawRange(Color.FromArgb(130, Color.OrangeRed)); }
                }
            };
        }
        static float BestTarget()
        {
            float r = 0;
            if (E.IsReady())
            {
                r = E.Range;
            }
            else
            {
                r = 800;
            }
            return r;
        }
        static void Game_OnCombo(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                var e = TargetSelector.GetTarget(BestTarget(), DamageType.Magical);
                if (e != null && !e.IsInvulnerable)
                {
                    if (W.IsReady() && Q.IsReady() | E.IsReady())
                    {
                        W.Cast(e.Position);
                    }
                    if (Q.IsReady())
                    {
                        if (e.IsRooted)
                        {
                            var prediction = Q.GetPrediction(e);
                            if (prediction.HitChance >= hitq())
                            {
                                Q.Cast(prediction.CastPosition);
                            }
                        }
                        else
                        {
                            var prediction = Q.GetPrediction(e);
                            if (prediction.HitChance >= hitq())
                            {
                                Q.Cast(prediction.UnitPosition);
                            }
                        }
                    }
                    if (E.IsReady())
                    {
                        var prediction = E.GetPrediction(e);
                        if (prediction.HitChance >= hite())
                        {
                            E.Cast(prediction.UnitPosition);
                        }
                    }
                    if (R.IsReady())
                    {
                        var sum = DamageBySlot(e, SpellSlot.Q)
                                + DamageBySlot(e, SpellSlot.E)
                                + GetIgniteDmg(e);
                        if (HPrediction(e, 250) < sum) { return; }
                        else
                        {
                            if (HPrediction(e, R.CastDelay) < DamageBySlot(e, SpellSlot.R))
                            {
                                var prediction = R.GetPrediction(e);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    R.Cast(prediction.UnitPosition);
                                }
                            }
                            else
                            {
                                var prediction = R.GetPrediction(e);
                                if (e.CountEnemyHeroesInRangeWithPrediction(R.Width) > 1 && prediction.HitChance >= HitChance.High)
                                {
                                    R.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
            }
        }
        static void Game_OnLaneclear(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var m = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(BestTarget()) && !x.IsDead).OrderBy(o => o.Distance(Player.Instance));
                if (m != null)
                {
                    if (W.IsReady() && Q.IsReady() | E.IsReady())
                    {
                        var wm = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsEnemy && x.IsMinion && !x.IsDead && x.IsValid).OrderBy(o => o.Distance(Player.Instance)).FirstOrDefault();
                        W.Cast(wm.Position);
                    }
                    if (Q.IsReady())
                    {
                        var qpred = Q.GetBestLinearCastPosition(m, Q.CastDelay);
                        if (qpred.HitNumber >= 3)
                        {
                            Q.Cast(qpred.CastPosition);
                        }
                    }
                    if (E.IsReady())
                    {
                        var epred = E.GetBestLinearCastPosition(m, E.CastDelay);
                        if (epred.HitNumber >= 3)
                        {
                            E.Cast(epred.CastPosition);
                        }
                    }
                }
            }
        }
        static void Game_OnJungleclear(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                var mob = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x => x.IsEnemy && x.IsMonster && !x.IsDead && x.IsValidTarget(BestTarget())).OrderBy(o => o.MaxHealth).FirstOrDefault();
                if (mob != null)
                {
                    if (W.IsReady() && Q.IsReady() | E.IsReady())
                    {
                        W.Cast(mob.Position);
                    }
                    if (Q.IsReady())
                    {
                        var prediction = Q.GetPrediction(mob);
                        if (prediction.HitChance >= HitChance.Medium)
                        {
                            Q.Cast(prediction.CastPosition);
                        }
                    }
                    if (E.IsReady())
                    {
                        var prediction = E.GetPrediction(mob);
                        if (prediction.HitChance >= HitChance.Medium)
                        {
                            E.Cast(prediction.CastPosition);
                        }
                    }
                }
            }
        }
        static void Game_OnIgnite(EventArgs args)
        {
            if (igmenu["igdo"].Cast<CheckBox>().CurrentValue)
            {
                if (!SummonerSpells.PlayerHas(SummonerSpellsEnum.Ignite)) { return; }
                var e = EntityManager.Heroes.Enemies.Find(f => f.IsInRange(Player.Instance, Ignite.Range) && !f.IsInvulnerable && !f.IsDead);
                if (e != null)
                {
                    if (Prediction.Health.GetPrediction(e, 200) < DamageLibrary.GetSummonerSpellDamage(Player.Instance, e, DamageLibrary.SummonerSpells.Ignite))
                    {
                        Ignite.Cast(e);
                    }

                }
            }
        }
        static void Game_OnExtraTicks(EventArgs args)
        {
            AutoQ();
            AutoE();
        }
        static void AutoQ()
        {
            if (qmenu["autoq"].Cast<CheckBox>().CurrentValue)
            {
                if (qmenu["qkill"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && HPrediction(f, Q.CastDelay) < DamageBySlot(f, SpellSlot.Q)
                    && !f.IsInvulnerable && !f.IsDead);
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.CastPosition);
                                }
                            }
                            else
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
                if (qmenu["qstun"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Stun) && !f.IsDead);
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.CastPosition);
                                }
                            }
                        }
                    }
                }
                if (qmenu["qslow"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Slow) && !f.IsDead);
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            var prediction = Q.GetPrediction(random);
                            if (prediction.HitChance >= HitChance.High)
                            {
                                Q.Cast(prediction.UnitPosition);
                            }
                        }
                    }
                }
                if (qmenu["qsnare"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Snare) && !f.IsDead);
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.CastPosition);
                                }
                            }
                            else
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
                if (qmenu["qtaunt"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Taunt) && !f.IsDead);
                    if (random != null)
                    {
                        if (Q.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.CastPosition);
                                }
                            }
                            else
                            {
                                var prediction = Q.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    Q.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
            }
        }
        static void AutoE()
        {
            if (emenu["autoe"].Cast<CheckBox>().CurrentValue)
            {
                if (emenu["ekill"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && HPrediction(f, E.CastDelay) < DamageBySlot(f, SpellSlot.E)
                    && !f.IsInvulnerable && !f.IsDead);
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = E.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    E.Cast(prediction.CastPosition);
                                }
                            }
                            else
                            {
                                var prediction = E.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    E.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
                if (emenu["estun"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Stun) && !f.IsDead);
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                                var prediction = E.GetPrediction(random);
                            if (prediction.HitChance >= HitChance.High)
                            {
                                E.Cast(prediction.CastPosition);
                            }
                        }
                    }
                }
                if (emenu["eslow"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Slow) && !f.IsDead);
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            var prediction = E.GetPrediction(random);
                            if (prediction.HitChance >= HitChance.High)
                            {
                                E.Cast(prediction.UnitPosition);
                            }
                        }
                    }
                }
                if (emenu["esnare"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Snare) && !f.IsDead);
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = E.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    E.Cast(prediction.CastPosition);
                                }
                            }
                            else
                            {
                                var prediction = E.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    E.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
                if (emenu["etaunt"].Cast<CheckBox>().CurrentValue)
                {
                    var random = EntityManager.Heroes.Enemies.Find(f => f.IsValidTarget(BestTarget())
                    && f.HasBuffOfType(BuffType.Taunt) && !f.IsDead);
                    if (random != null)
                    {
                        if (E.IsReady())
                        {
                            if (random.IsRooted)
                            {
                                var prediction = E.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    E.Cast(prediction.CastPosition);
                                }
                            }
                            else
                            {
                                var prediction = E.GetPrediction(random);
                                if (prediction.HitChance >= HitChance.High)
                                {
                                    E.Cast(prediction.UnitPosition);
                                }
                            }
                        }
                    }
                }
            }
        }
        static float DamageBySlot(Obj_AI_Base e, SpellSlot slot)
        {
            var dmg = 0f;
            if (slot == SpellSlot.Q)
            {
                if (Q.IsReady())
                    dmg += new float[] { 60, 95, 130, 165, 200 }[Player.GetSpell(slot).Level - 1] +
                              0.60f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.E)
            {
                if (E.IsReady())
                    dmg += new float[] { 60, 95, 130, 165, 200 }[Player.GetSpell(slot).Level - 1] +
                              0.50f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.R)
            {
                if (R.IsReady())
                    dmg += new float[] { 180, 265, 350 }[Player.GetSpell(slot).Level - 1] +
                              0.70f * Player.Instance.FlatMagicDamageMod;
            }
            return Player.Instance.CalculateDamageOnUnit(e, DamageType.Magical, dmg);
        }
        static float HPrediction(Obj_AI_Base e, int spellcastdelay)
        {
            return Prediction.Health.GetPrediction(e, spellcastdelay);
        }
        static float GetIgniteDmg(Obj_AI_Base e)
        {
            if (!SummonerSpells.PlayerHas(SummonerSpellsEnum.Ignite)) { return 0; }
            float d = 0;
            if (Ignite.IsReady())
            {
                d = DamageLibrary.GetSummonerSpellDamage(Player.Instance, e, DamageLibrary.SummonerSpells.Ignite);
            }
            return d;
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
    }
}