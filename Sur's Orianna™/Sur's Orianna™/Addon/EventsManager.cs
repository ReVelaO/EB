namespace Orianna.Addon
{
    using System;

    using EloBuddy;

    using EloBuddy.SDK;

    using Color = System.Drawing.Color;
    using EloBuddy.SDK.Menu.Values;

    internal class EventsManager
    {
        public static void Load()
        {
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
        }

        static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (MenuManager.mlane["mm"].Cast<CheckBox>().CurrentValue)
                {
                    if (Player.Instance.ManaPercent >= MenuManager.mlane["mmsli"].Cast<Slider>().CurrentValue)
                    {
                        Orb.Laneclear.Get();
                    }
                }
                else
                {
                    Orb.Laneclear.Get();
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Orb.Jungleclear.Get();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                if (Player.HasBuff("orianaghostself"))
                {
                    SpellManager.W.Cast();
                }
            }
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (SpellManager.Q.IsReady())
            {
                SpellManager.Q.DrawRange(Color.FromArgb(130, Color.OrangeRed));
            }
            if (MenuManager.mdrawings["ball"].Cast<CheckBox>().CurrentValue)
            {
                if (BallManager.Ball != null && !BallManager.Ball.IsZero 
                    && Player.Instance.Distance(BallManager.Ball) < 1100)
                {
                    BallManager.Ball.DrawCircle(410, SharpDX.Color.MediumPurple);
                    BallManager.Ball.DrawCircle(250, SharpDX.Color.Purple);
                }
            }
        }

        static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && args.Target.IsMe && sender is AIHeroClient && sender != null)
            {
                if (SpellManager.E.IsReady()) { SpellManager.E.Cast(Player.Instance); }
            }
        }
    }
}
