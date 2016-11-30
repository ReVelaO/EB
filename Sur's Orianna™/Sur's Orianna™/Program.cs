namespace Orianna
{
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Enumerations;
    using Color = System.Drawing.Color;
    using SharpDX;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Menu;

    internal class Program
    {
        static Menu menu;
        static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 1, 1200, 175) { AllowedCollisionCount = -1 };
        static Spell.Active W = new Spell.Active(SpellSlot.W);
        static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 1100);
        static Spell.Active R = new Spell.Active(SpellSlot.R);
        static Vector3 Ball;
        static int NearWBall(bool Hero)
        {
            if (Hero)
            {
                return Ball != null ? Ball.CountEnemyHeroesInRangeWithPrediction(250, 250) : 0;
            }
            if (!Hero)
            {
                return Ball != null ? Ball.CountEnemyMinionsInRangeWithPrediction(250, 250) : 0;
            }
            return 0;
        }
        static int NearRBall => Ball != null ? Ball.CountEnemyHeroesInRangeWithPrediction(410, 750) : 0;
        static bool IsInWBall(Obj_AI_Base e) => e.IsInRange(Ball, 250);
        static bool IsInRBall(Obj_AI_Base e) => e.IsInRange(Ball, 410);
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += olc;
        }
        static void olc(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Orianna)
            {
                Chat.Print("This addon is not for <font color='#FFFFFF'>" + Player.Instance.ChampionName + "</font>");
                return;
            }
            Chat.Print("[<font color='#FFFFFF'>Sur's Orianna™™</font> loaded. <font color='#FFFFFF'>Enjoy</font>!");
            Notifications.Show(new SimpleNotification("Sur's Orianna™", "Welcome back buddy!"), 20000);
            menu = MainMenu.AddMenu("Orianna", "index0");
            menu.AddGroupLabel("[FREE] Orianna Addon by Surprise");
            menu.AddLabel("Actually BETA STAGE");
            menu.AddLabel("Addon have: Combo (Press Orbwalker Key), Laneclear (Press Orbwalker Key), Jungleclear (Press Orbwalker Key), Auto E on AA (Automated)");
            Game.OnTick += Game_OnTick;
            Obj_AI_Base.OnProcessSpellCast += OAB_OnProcessSpellCast;
            Obj_AI_Base.OnBasicAttack += OAB_OnBasicAttack;
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
                Laneclear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Jungleclear();
            }
        }
        static void OAB_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.Slot == SpellSlot.Q)
                {
                    Ball = args.End;
                }
            }
        }
        static void OAB_OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && args.Target.IsMe && sender is AIHeroClient && sender != null)
            {
                if (E.IsReady()) { SManager.TargetedTo(Player.Instance, E); }
            }
        }
        static void Drawing_OnDraw(EventArgs args)
        {
            if (Q.IsReady())
            {
                Q.DrawRange(Color.FromArgb(130, Color.OrangeRed));
            }
        }
        static bool CanBe(Obj_AI_Base enemy)
        {
            return enemy != null && !enemy.IsZombie && !enemy.IsInvulnerable;
        }
        static void Combo()
        {
            var e = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (CanBe(e))
            {
                SManager.SkillShotTo(e, Q, HitChance.High);
                if (NearWBall(true) > 0)
                {
                    SManager.Actived(e, W);
                }
                if (NearRBall >= 2)
                {
                    SManager.Actived(e, R);
                }
            }
        }
        static void Laneclear()
        {
            if (Q.IsReady())
            {
                var mq = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(Q.Range)).OrderBy(o=>o.Health);
                if (mq != null)
                {
                    var p = Q.GetBestCircularCastPosition(mq);
                    if (p.HitNumber >= 3)
                    {
                        Q.Cast(p.CastPosition);
                    }
                }
            }
            if (W.IsReady())
            {
                if (NearWBall(false) >= 2)
                {
                    W.Cast();
                }
            }
        }
        static void Jungleclear()
        {
            var m = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(o => o.MaxHealth).FirstOrDefault(d => d.IsValidTarget(Q.Range));
            if (m != null)
            {
                if (Q.IsReady()) { Q.Cast(m.Position); }
                if (W.IsReady() && IsInWBall(m)) { W.Cast(); }
            }
        }
    }
    internal class SManager
    {
        static bool CanBe(Obj_AI_Base enemy) => enemy != null && !enemy.IsZombie && !enemy.IsInvulnerable;

        public static void SkillShotTo(Obj_AI_Base target, Spell.Skillshot spell, HitChance value)
        {
            if (CanBe(target))
            {
                var p = spell.GetPrediction(target);
                if (p.HitChance >= value && spell.IsReady() && target.IsValidTarget(spell.Range))
                {
                    spell.Cast(p.CastPosition);
                }
            }
            else
            {
                return;
            }
        }
        public static void TargetedTo(Obj_AI_Base target, Spell.Targeted spell)
        {
            if (CanBe(target))
            {
                if (spell.IsReady() && target.IsValidTarget(spell.Range))
                {
                    spell.Cast(target);
                }
            }
        }
        public static void Actived(Obj_AI_Base target, Spell.Active spell)
        {
            if (CanBe(target))
            {
                if (spell.IsReady() && target.IsValidTarget(spell.Range))
                {
                    spell.Cast();
                }
            }
        }
    }
}
