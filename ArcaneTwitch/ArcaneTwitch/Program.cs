using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;
using System.Drawing;
using Color = System.Drawing.Color;

namespace ChallengerTwitch
{
    class Program
    {
        static Menu ComboMenu, ItemsMenu, ExtraMenu, menu;
        static Spell.Active _Cloak;
        static Spell.Skillshot _Elixir;
        static Spell.Active _Frustation;
        static Spell.Active _Love;
        static Item _Destruction;
        static Spell.Active Recall;
        static AIHeroClient myHero { get { return ObjectManager.Player; } }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadUP;
        }


        static void LoadUP(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Twitch)
            {
                return;
            }
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Chat.Print("[" + "<font color = '#f42222'>Arcane</font><font color = '#22f45a'> Twitch</font>:<font color = '#FFFFFF'> Loaded</font>" + "] -> <font color = '#FFFFFF'>" + version + "</font>");
            _Cloak = new Spell.Active(SpellSlot.Q);
            _Elixir = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 250, 1550, 275);
            _Frustation = new Spell.Active(SpellSlot.E);
            _Love = new Spell.Active(SpellSlot.R);
            _Destruction = new Item(ItemId.Blade_of_the_Ruined_King);
            Recall = new Spell.Active(SpellSlot.Recall);

            menu = MainMenu.AddMenu("Arcane Twitch", "drt");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");

            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("WU", new CheckBox("Use W"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("Ee", new CheckBox("Use E", true));
            ComboMenu.AddLabel("Select you E Mode");
            ComboMenu.Add("Lista", new ComboBox("E Mode:", 0, "Normal", "Smart-Algorithm"));
            ComboMenu.AddLabel("UN-BYPASS: Will Check Packs for Execute. BYPASS: Will Calc DMG of Poison");
            ComboMenu.Add("ebb", new ComboBox("Stacks Mode", 0, "UN-BYPASS", "BYPASS"));
            ComboMenu.AddLabel("If UN-BYPASS mode, please choose stacks.");
            ComboMenu.Add("EVAR", new Slider("Stacks to execute", 4, 1, 6));

            ItemsMenu = menu.AddSubMenu("Items","itemenu");
            ItemsMenu.AddLabel("¿Use BOTRK (Blade of The Ruined King)?");
            ItemsMenu.Add("box1", new ComboBox("BOTRK",0,"YES", "NO"));
            ItemsMenu.Add("vidabox", new ComboBox("BOTRK Mode:",0,"Combo","Smart HP"));
            ItemsMenu.Add("vidasmart", new Slider("Use if hp is below than HP%", 92, 1, 100));

            ExtraMenu = menu.AddSubMenu("Extra", "extamenu");
            ExtraMenu.AddGroupLabel("Extra features");
            ExtraMenu.Add("box4", new ComboBox("¿Auto invisibility Cloak if base speed boost is on?", 0,"YES","NO"));
            ExtraMenu.Add("box3", new KeyBind("Stealth Recall", false, KeyBind.BindTypes.HoldActive, 'T'));
            Game.OnTick += OnTick;
        }

        static void OnTick(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Play();
                break;
            }
            Menu();
            BaseCloak();
            Items();
            StealthRecall();
        }

        static void Menu()
        {
            if (ComboMenu["Lista"].Cast<ComboBox>().CurrentValue == 0)
            {
                ComboMenu["ebb"].Cast<ComboBox>().IsVisible = true;
                ComboMenu["EVAR"].Cast<Slider>().IsVisible = true;
                ComboMenu["If UN-BYPASS mode, please choose stacks."].IsVisible = true;
                ComboMenu["UN-BYPASS: Will Check Packs for Execute. BYPASS: Will Calc DMG of Poison"].IsVisible = true;
            }
            if (ComboMenu["Lista"].Cast<ComboBox>().CurrentValue == 1)
            {
                ComboMenu["ebb"].Cast<ComboBox>().IsVisible = false;
                ComboMenu["EVAR"].Cast<Slider>().IsVisible = false;
                ComboMenu["If UN-BYPASS mode, please choose stacks."].IsVisible = false;
                ComboMenu["UN-BYPASS: Will Check Packs for Execute. BYPASS: Will Calc DMG of Poison"].IsVisible = false;
            }
            if (ItemsMenu["vidabox"].Cast<ComboBox>().CurrentValue == 0)
            {
                ItemsMenu["vidasmart"].Cast<Slider>().IsVisible = false;
            }
            if (ItemsMenu["vidabox"].Cast<ComboBox>().CurrentValue == 1)
            {
                ItemsMenu["vidasmart"].Cast<Slider>().IsVisible = true;
            }
        }

        static void Play()
        {
            var process = TargetSelector.GetTarget(_Elixir.Range, DamageType.Physical);
            if (process == null)
            {
                return;
            }
            Elixir(process);
            Frustation(process);
        }

        /*static void Cloak()
        {
            string pepas = "HideInShadows";
        }*/

        static void Elixir(AIHeroClient process)
        {
            var ElixirPred = _Elixir.GetPrediction(process);

            if (ComboMenu["WU"].Cast<CheckBox>().CurrentValue)
            {


                if (process.IsValidTarget(_Elixir.Range) && ElixirPred.HitChance >= HitChance.High)
                {
                    if (process.IsInRange(myHero, _Elixir.Range)
                            && _Elixir.IsReady()
                            && _Elixir.CanCast(process)
                            && !_Elixir.IsOnCooldown)
                    {
                        _Elixir.Cast(process);
                    }
                }
                if (process.IsRooted)
                {
                    _Elixir.Cast(process.Position);
                }
            }
        }

        static void Frustation(AIHeroClient process)
        {
            if (process.HasBuff("twitchdeadlyvenom") == true)
            {

                if (ComboMenu["Ee"].Cast<CheckBox>().CurrentValue)
                {
                    if (ComboMenu["Lista"].Cast<ComboBox>().CurrentValue == 0)
                    {
                        if (ComboMenu["ebb"].Cast<ComboBox>().CurrentValue == 0)
                        {
                            if (GetFrustationAlgorithm(process) >= ComboMenu["EVAR"].Cast<Slider>().CurrentValue)
                            {
                                if (process.IsValidTarget(_Elixir.Range + 150) && _Frustation.IsReady())
                                {
                                    _Frustation.Cast();
                                }
                            }
                        }
                        if (ComboMenu["ebb"].Cast<ComboBox>().CurrentValue == 1)
                        {
                            if (GetFrustationDMG(process) >= (process.TotalShieldHealth() - 5))
                            {
                                if (process.IsValidTarget(_Elixir.Range + 150) && _Frustation.IsReady())
                                {
                                    _Frustation.Cast();
                                }
                            }
                        }
                    }
                    if (ComboMenu["Lista"].Cast<ComboBox>().CurrentValue == 1)
                    {
                        if (process.HasBuff("twitchdeadlyvenom"))
                        {
                            if (process.HealthPercent >= 50
                                && myHero.HealthPercent <= 38
                                && process.IsInAutoAttackRange(myHero))
                            {
                                if (process.IsValidTarget(_Elixir.Range + 150) && _Frustation.IsReady())
                                {
                                    _Frustation.Cast();
                                }
                            }
                            else
                            {
                                if (process.HealthPercent < myHero.HealthPercent)
                                {
                                    if (GetFrustationDMG(process) >= (process.TotalShieldHealth() - 5))
                                    {
                                        if (process.IsValidTarget(_Elixir.Range + 150) && _Frustation.IsReady())
                                        {
                                            _Frustation.Cast();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static void Love()
        {

        }

        static void Items()
        {
            var process = TargetSelector.GetTarget(myHero.GetAutoAttackRange(), DamageType.Physical);
            if (process == null || !_Destruction.IsOwned())
            {
                return;
            }
            if (ItemsMenu["box1"].Cast<ComboBox>().CurrentValue == 1)
            {
                return;
            }
            if (ItemsMenu["box1"].Cast<ComboBox>().CurrentValue == 0)
            {
                if (ItemsMenu["vidabox"].Cast<ComboBox>().CurrentValue == 0)
                {
                    if (process.IsValidTarget(myHero.GetAutoAttackRange())
                        && _Destruction.IsReady()
                        && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                    {
                        _Destruction.Cast(process);
                    }
                }
                if (ItemsMenu["vidabox"].Cast<ComboBox>().CurrentValue == 1)
                {
                    if (myHero.HealthPercent <= ItemsMenu["vidasmart"].Cast<Slider>().CurrentValue
                        && process.IsValidTarget(myHero.GetAutoAttackRange())
                        && _Destruction.IsReady()
                        && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                    {
                        _Destruction.Cast(process);
                    }
                }
            }
        }

        static void BaseCloak()
        {
            if (ExtraMenu["box4"].Cast<ComboBox>().CurrentValue == 1)
            {
                return;
            }
            if (myHero.HasBuff("SRHomeguardSpeed")
                && myHero.IsMoving
                && ExtraMenu["box4"].Cast<ComboBox>().CurrentValue == 0 
                && _Cloak.IsReady() 
                && !Shop.IsOpen)
            {
                _Cloak.Cast();
            }
        }

        static void StealthRecall()
        {
            if (ExtraMenu["box3"].Cast<KeyBind>().CurrentValue 
                && _Cloak.IsReady())
            {
                _Cloak.Cast();
                Recall.Cast();
            }
        }
        //Sida's code here ( ͡° ͜ʖ ͡°)
        static int GetFrustationAlgorithm(AIHeroClient process)
        {
            var twitchECount = 0;
            for (var i = 1; i < 7; i++)
            {
                if (ObjectManager.Get<Obj_GeneralParticleEmitter>()
                    .Any(e => e.Position.Distance(process.ServerPosition) <= 175 &&
                              e.Name == "twitch_poison_counter_0" + i + ".troy"))
                {
                    twitchECount = i;
                }
            }
            return twitchECount;
        }
        static float GetFrustationDMG(AIHeroClient process)
        {
                if (GetFrustationAlgorithm(process) == 0) return 0;

                float stacksChamps = GetFrustationAlgorithm(process);

                float EDamageChamp = new[] { 20, 35, 50, 65, 80 }[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level - 1];

                if (stacksChamps > 1)
                {
                    EDamageChamp += (new[] { 15, 20, 25, 30, 35 }[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level - 1] + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.PhysicalDamageRatio * ObjectManager.Player.FlatPhysicalDamageMod) + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.SpellDamageRatio * ObjectManager.Player.FlatMagicDamageMod)) * (stacksChamps - 1);
                }

                return ObjectManager.Player.CalculateDamageOnUnit(process, DamageType.Physical, EDamageChamp);
        }
    }
}
