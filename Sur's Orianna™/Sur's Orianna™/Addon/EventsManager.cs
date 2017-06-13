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
                if (UtilsManager.Ignite.IsReady() 
                    && MenuManager.msum["i"].Cast<CheckBox>().CurrentValue)
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
                    if (SpellManager.W.IsReady() && Player.Instance.HasBall())
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
            var shieldMenu = MenuManager.mshield;

            if (args.Target.IsMe && sender.IsEnemy)
            {
                if (sender is Obj_AI_Turret && shieldMenu["tt"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.E.IsReady())
                    {
                        SpellManager.E.Cast(Player.Instance);
                    }
                }
                if (sender is AIHeroClient && shieldMenu["b"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.E.IsReady())
                    {
                        SpellManager.E.Cast(Player.Instance);
                    }
                }
                if (sender.IsMonster && shieldMenu["m"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.E.IsReady())
                    {
                        SpellManager.E.Cast(Player.Instance);
                    }
                }
            }

            if (args.Target.IsAlly && !args.Target.IsMe)
            {
                var ally = (AIHeroClient)args.Target;

                if (sender is Obj_AI_Turret && shieldMenu["tta"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.E.IsReady() 
                        && ally.IsInRange(Player.Instance, SpellManager.E.Range))
                    {
                        SpellManager.E.Cast(ally);
                    }
                }
                if (sender is AIHeroClient && shieldMenu["ba"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.E.IsReady()
                        && ally.IsInRange(Player.Instance, SpellManager.E.Range))
                    {
                        SpellManager.E.Cast(ally);
                    }
                }
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && args.Slot == SpellSlot.R)
            {
                if (MenuManager.mcombo["rblock"].Cast<CheckBox>().CurrentValue && !UtilsManager.WillShock)
                {
                    if (BallManager.RBall.CountEnemyHeroesNear == 0)
                    {
                        args.Process = false;
                    }
                }
            }
        }
    }
}