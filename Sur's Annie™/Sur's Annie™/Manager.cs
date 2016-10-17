using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace Sur_s_Annie_
{
    public static class Manager
    {
        private static AIHeroClient me => ObjectManager.Player;
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        private static Menu menu,Combo,Drawings,Farm;
        public static CheckBox MComboQ;
        public static CheckBox MComboW;
        public static CheckBox MComboE;
        public static CheckBox MComboR;
        public static CheckBox MFarmQ;
        public static CheckBox MFarmQ2;
        public static CheckBox MLaneQ;
        public static CheckBox MLaneQ2;
        public static CheckBox MLaneW;
        public static Slider MLaneSW;
        public static Slider MLaneMana;
        public static CheckBox MDrawQ;
        public static CheckBox MDrawW;
        public static CheckBox MDrawR;
        public static int Passive => me.GetBuffCount("pyromania");
        public static bool OnStunPassive => me.HasBuff("pyromania_particle");

        public static void LoadSkills()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Cone);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, 600, SkillShotType.Circular);
        }
        public static void LoadMenu()
        {
            menu = MainMenu.AddMenu("Sur's Annie™","main");
            menu.AddGroupLabel("Sur's Annie™ Addon");
            menu.AddLabel("Enjoy to my Annie Addon.-");
            menu.AddLabel("Made by Surprise & Bik");
            Combo = menu.AddSubMenu("Combo","combomenu");
            Combo.AddGroupLabel("Sur's Annie™ - Combo");
            MComboQ = Combo.Add("useq", new CheckBox("Use Q", true));
            MComboW = Combo.Add("usew", new CheckBox("Use W", true));
            MComboE = Combo.Add("usee", new CheckBox("Use E", true));
            MComboR = Combo.Add("user", new CheckBox("Use R", true));
            Drawings = menu.AddSubMenu("Drawing","drawingmenu");
            Drawings.AddGroupLabel("Sur's Annie™ - Drawings");
            MDrawQ = Drawings.Add("drawq", new CheckBox("Draw Q Range", true));
            MDrawW = Drawings.Add("draww", new CheckBox("Draw W Range", true));
            MDrawR = Drawings.Add("drawr", new CheckBox("Draw R Range", true));
            Farm = menu.AddSubMenu("Farm","farmmenu");
            Farm.AddGroupLabel("Sur's Annie™ - Farm");
            Farm.AddLabel("Settings for Last Hit");
            MFarmQ = Farm.Add("farmq", new CheckBox("LastHit Q", true));
            MFarmQ2 = Farm.Add("farmq2", new CheckBox("BLOCK Last Hit Q at 4 stacks", false));
            Farm.AddSeparator(10);
            Farm.AddLabel("Settings for Laneclear");
            MLaneQ = Farm.Add("laneq", new CheckBox("Use Q", true));
            MLaneQ2 = Farm.Add("laneq2", new CheckBox("Use Q OnLastHit", true));
            MLaneW = Farm.Add("lanew", new CheckBox("Use W", true));
            MLaneSW = Farm.Add("lanesw", new Slider("Minions in range for W usage", 3, 1, 6));
            Farm.AddLabel("Settings for ManaManager");
            MLaneMana = Farm.Add("lanemana", new Slider("Manage at:", 87, 1, 100));

        }
        public static void LoadEvents()
        {
            Game.OnTick += Events.OnTick;
            Drawing.OnDraw += Events.OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Events.OnProcessSpellCast;
        }
        public static void LoadKillSteal()
        {
            foreach (var random in EntityManager.Heroes.Enemies.Where(x => !x.HasBuffOfType(BuffType.Invulnerability) && x.IsValidTarget(Q.Range) && x.IsEnemy && !x.IsDead))
            {
                if (Q.IsReady() && random.IsValidTarget(Q.Range))
                {
                    var PredictHealth = Prediction.Health.GetPrediction(random, Q.CastDelay) < DamageBySpell(random, Q.Slot);
                    if (PredictHealth && Q.IsReady()) { Q.Cast(random); }
                }
                if (W.IsReady() && random.IsValidTarget(W.Range))
                {
                    var PredictHealth = Prediction.Health.GetPrediction(random, W.CastDelay) < DamageBySpell(random, W.Slot);
                    if (PredictHealth && W.IsReady()) { W.Cast(random); }
                }
            }
        }
        public static float DamageBySpell(Obj_AI_Base Enemy, SpellSlot Slot)
        {
            float Damage = 0f;
            switch (Slot)
            {
                case SpellSlot.Q:
                    if (Q.IsReady())
                    {
                        Damage += new float[] { 80, 115, 150, 185, 220}[Player.GetSpell(Slot).Level -1] + (0.8f * me.FlatMagicDamageMod);
                    }
                    break;
                case SpellSlot.W:
                    if (W.IsReady())
                    {
                        Damage += new float[] { 70, 115, 160, 205, 250 }[Player.GetSpell(Slot).Level - 1] + (0.85f * me.FlatMagicDamageMod);
                    }
                    break;
                case SpellSlot.R:
                    if (R.IsReady())
                    {
                        Damage += new float[] { 150, 275, 400 }[Player.GetSpell(Slot).Level - 1] + (0.65f * me.FlatMagicDamageMod);
                    }
                    break;
            }
            return Player.Instance.CalculateDamageOnUnit(Enemy, DamageType.Magical, Damage);
        }

    }
}
