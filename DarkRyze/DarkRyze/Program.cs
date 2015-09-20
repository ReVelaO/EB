using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace DarkRyze
{
    internal class Program
    {
        public static Menu ComboMenu, DrawingsMenu, KSMenu, FarmMenu, menu;
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static int Mana { get { return (int) _Player.Mana; } }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Hacks.AntiAFK = true;
            Bootstrap.Init(null);
            TS.init();
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, (int)0.25f, (Int32)1700, (int)50f);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);

            menu = MainMenu.AddMenu("DarkRyze", "DarkRyze");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");

            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("QU", new CheckBox("Use Q"));
            ComboMenu.Add("WU", new CheckBox("Use W"));
            ComboMenu.Add("EU", new CheckBox("Use E"));
            ComboMenu.Add("RU", new CheckBox("Use R"));

            FarmMenu = menu.AddSubMenu("Farm", "farmenu");

            FarmMenu.AddGroupLabel("Last Hit Settings");
            FarmMenu.Add("LHQ", new CheckBox("Use Q"));

            KSMenu = menu.AddSubMenu("Kill Steal (KS)", "ksmenu");

            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.Add("EnableKS", new CheckBox("Enable KS System"));
            KSMenu.Add("KSQ", new CheckBox("Auto Q"));
            KSMenu.Add("KSW", new CheckBox("Auto W"));
            KSMenu.Add("KSE", new CheckBox("Auto E"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");

            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            DrawingsMenu.Add("DWE", new CheckBox("Draw W + E"));

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, 900, System.Drawing.Color.BlueViolet);
            }

            if (DrawingsMenu["DWE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, 600, System.Drawing.Color.BlueViolet);
            }

        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                PegarleAlQlo();
            }
            else if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit)
            {
                LastHit();
            }
            if (KSMenu["EnableKS"].Cast<CheckBox>().CurrentValue)
            {
                RobarWeas();
            }
        }

        public static float IsInRange()
        {
            if (Q.IsReady())
            {
                return Q.Range;
            }
            return _Player.GetAutoAttackRange();
        }

        public static void PegarleAlQlo()
        {
            var QCHECK = ComboMenu["QU"].Cast<CheckBox>().CurrentValue;
            var WCHECK = ComboMenu["WU"].Cast<CheckBox>().CurrentValue;
            var ECHECK = ComboMenu["EU"].Cast<CheckBox>().CurrentValue;
            var RCHECK = ComboMenu["RU"].Cast<CheckBox>().CurrentValue;
            var QREADY = Q.IsReady();
            var WREADY = W.IsReady();
            var EREADY = E.IsReady();
            var RREADY = R.IsReady();
            var target = TS.GetTarget(IsInRange(), DamageType.Magical);

            if (WREADY && WCHECK)
            {
                EloBuddy.SDK.Core.DelayAction(() => { W.Cast(target); }, (int)0.25f);
            }

            var QPred = Q.GetPrediction(target);
            else if (QCHECK && QREADY && target.IsValidTarget(750))
            {
                Q.Cast(QPred.UnitPosition);
            }

            else if (EREADY && ECHECK)
            {
                EloBuddy.SDK.Core.DelayAction(() => { E.Cast(target); }, (int)0.25f);
            }
            else if (RCHECK && RREADY && target.HasBuff("RyzeW"))
            {
                EloBuddy.SDK.Core.DelayAction(() => { R.Cast(); }, (int)0.15f);
            }
        }

        public static void RobarWeas()
        {
            foreach (var target in HeroManager.Enemies.Where(hero => hero.Health <= RyzeCalcs.Q(hero)))
                if (KSMenu["KSQ"].Cast<CheckBox>().CurrentValue && Q.IsReady() && !target.IsDead)
                {
                    Q.Cast(target);
                }

            foreach (var target in HeroManager.Enemies.Where(hero => hero.Health <= RyzeCalcs.W(hero)))
                if (KSMenu["KSW"].Cast<CheckBox>().CurrentValue && W.IsReady() && !target.IsDead)
                {
                    W.Cast(target);
                }

            foreach (var target in HeroManager.Enemies.Where(hero => hero.Health <= RyzeCalcs.E(hero)))
                if (KSMenu["KSE"].Cast<CheckBox>().CurrentValue && E.IsReady() && !target.IsDead)
                {
                    E.Cast(target);
                }
        }

        public static void LastHit()
        {
            var source =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.IsEnemy && a.Distance(_Player) < IsInRange())
                    .OrderBy(a => a.Health)
                    .FirstOrDefault();

            if (FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue && RyzeCalcs.Q(source) > source.Health && !source.IsDead && source.Distance(_Player) < Q.Range)
            {
                Q.Cast(source);
                return;
            }
        }

        public static int GetPassiveBuff
        {
            get
            {
                var data = _Player.Buffs.FirstOrDefault(b => b.DisplayName == "RyzePassiveStack");
                return data != null ? data.Count : 0;
            }
        }
    }
}
