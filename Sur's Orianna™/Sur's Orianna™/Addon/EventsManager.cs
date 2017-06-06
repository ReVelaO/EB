namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;
    using System;
    using Color = System.Drawing.Color;

    public static class EventsManager
    {
        private static AIHeroClient Orianna => Player.Instance;

        public static void Load()
        {
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (MenuManager.mshock["sf"].Cast<KeyBind>().CurrentValue)
            {
                UtilsManager.WillShock = true;
                UtilsManager.Shockwave();
            }
            if (UtilsManager.HasIgnite)
            {
                if (!UtilsManager.Ignite.IsReady()) return;

                if (MenuManager.msum["i"].Cast<CheckBox>().CurrentValue)
                {
                    UtilsManager.AutoIgnite();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Orb.Laneclear.Get();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Orb.Jungleclear.Get();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                if (MenuManager.mflee["w"].Cast<CheckBox>().CurrentValue)
                {
                    if (!SpellManager.W.IsReady()) return;
                    if (Player.Instance.HasBall())
                    {
                        SpellManager.W.Cast();
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Orianna.IsDead || Orianna.IsRecalling()) return;

            if (MenuManager.mdrawings["q"].Cast<CheckBox>().CurrentValue)
            {
                if (SpellManager.Q.IsReady())
                {
                    SpellManager.Q.DrawRange(Color.FromArgb(130, Color.OrangeRed));
                }
            }
            if (MenuManager.mdrawings["ball"].Cast<CheckBox>().CurrentValue)
            {
                if (Orianna.HasBall())
                {
                    BallManager.Ball.DrawCircle(410, SharpDX.Color.MediumPurple);
                    BallManager.Ball.DrawCircle(250, SharpDX.Color.Purple);
                }
                else
                {
                    if (BallManager.Ball != null)
                    {
                        BallManager.Ball.DrawCircle(410, SharpDX.Color.MediumPurple);
                        BallManager.Ball.DrawCircle(250, SharpDX.Color.Purple);
                    }
                }
            }
            if (MenuManager.mshock["sfr"].Cast<CheckBox>().CurrentValue)
            {
                if (UtilsManager.HasFlash)
                {
                    if (UtilsManager.Flash.IsReady())
                    {
                        Circle.Draw(SharpDX.Color.LightYellow, 800, Player.Instance.Position);
                    }
                }
            }
        }

        private static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly || sender.IsMe) return;

            if (!SpellManager.E.IsReady()) return;

            if (sender.IsEnemy && sender != null)
            {
                if (args.Target.IsMe && args.Target != null)
                {
                    if (MenuManager.mshield["b"].Cast<CheckBox>().CurrentValue)
                    {
                        if (sender is AIHeroClient)
                        {
                            SpellManager.E.Cast(Orianna);
                        }
                    }

                    if (MenuManager.mshield["m"].Cast<CheckBox>().CurrentValue)
                    {
                        if (sender.IsMinion && sender.CountEnemyMinionsInRange(433) > 3)
                        {
                            SpellManager.E.Cast(Orianna);
                        }
                    }
                }
                if (args.Target.IsAlly && !args.Target.IsMe && args.Target != null && args.Target.IsInRange(Orianna, SpellManager.E.Range))
                {
                    if (MenuManager.mshield["ba"].Cast<CheckBox>().CurrentValue)
                    {
                        if (sender is AIHeroClient)
                        {
                            var ally = args.Target as Obj_AI_Base;

                            SpellManager.E.Cast(ally);
                        }
                    }
                }
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.R
                && MenuManager.mcombo["rblock"].Cast<CheckBox>().CurrentValue
                && BallManager.RBall.CountEnemyHeroesNear == 0 && !UtilsManager.WillShock)
            {
                args.Process = false;
            }
        }
    }
}