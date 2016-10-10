using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
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

namespace ArcaneTwitch
{
    public class Program
    {
        static Menu ComboMenu, ItemsMenu, ExtraMenu, menu;
        public static Spell.Active _Cloak;
        public static Spell.Skillshot _Elixir;
        public static Spell.Active _Frustation;
        public static Spell.Active _Love;
        public static Item _BOTRK;
        public static Item _Sable;
        public static Spell.Active Recall;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += LoadUP;
        }
        public static void LoadUP(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Twitch)
            {
                Chat.Print("<font color='#FFFFFF'>[You Champion: " + Player.Instance.ChampionName + "<font color='#FFFFFF'> is not supported</font><font color='#FFFFFF'>]</font>");
                return;
            }
            Chat.Print("<font color='#FFFFFF'>[Addon Twitch: </font><font color='#34BFD6'>Loaded</font>]");
            _Cloak = new Spell.Active(SpellSlot.Q);
            _Elixir = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 250, 1550, 275);
            _Frustation = new Spell.Active(SpellSlot.E);
            _Love = new Spell.Active(SpellSlot.R);
            _BOTRK = new Item(ItemId.Blade_of_the_Ruined_King);
            _Sable = new Item(ItemId.Bilgewater_Cutlass);
            Recall = new Spell.Active(SpellSlot.Recall);

            menu = MainMenu.AddMenu("Arcane Twitch", "drt");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");

            ComboMenu.AddGroupLabel("Function Combo");
            ComboMenu.Add("WU", new CheckBox("Execute W"));
            ComboMenu.Add("Ee", new CheckBox("Execute E", true));
            ComboMenu.AddLabel("Select you E Mode");
            ComboMenu.Add("Lista", new ComboBox("E Mode:", 0, "Normal", "Smart-Algorithm"));
            ComboMenu.AddLabel("UN-BYPASS: Will Check Packs for Execute. BYPASS: Will Calc DMG of Poison");
            ComboMenu.Add("ebb", new ComboBox("Stacks Mode", 0, "UN-BYPASS", "BYPASS"));
            ComboMenu.AddLabel("If UN-BYPASS mode, please choose stacks.");
            ComboMenu.Add("EVAR", new Slider("Stacks to execute", 4, 1, 6));

            ItemsMenu = menu.AddSubMenu("Activator","itemenu");

            ItemsMenu.AddGroupLabel("Function Activator");
            ItemsMenu.AddLabel("¿Use Bilgewater Cutlass?");
            ItemsMenu.Add("useBWC", new ComboBox("BWC", 0, "YES", "NO"));
            ItemsMenu.AddLabel("¿Use BOTRK (Blade of The Ruined King)?");
            ItemsMenu.Add("useBOTRK", new ComboBox("BOTRK",0,"YES", "NO"));
            ItemsMenu.Add("modeBOTRK", new ComboBox("BOTRK Mode:",0,"Combo","Smart HP"));
            ItemsMenu.Add("hpBOTRK", new Slider("Use if hp is below than HP%", 92, 1, 100));

            ExtraMenu = menu.AddSubMenu("Misc", "extamenu");

            ExtraMenu.AddGroupLabel("Function Misc");
            ExtraMenu.AddLabel("¿Auto invisibility Cloak when base speed boost is on?");
            ExtraMenu.Add("box4", new ComboBox("Invisibility Cloak", 0,"YES","NO"));
            ExtraMenu.Add("box3", new KeyBind("Stealth Recall", false, KeyBind.BindTypes.HoldActive, 'T'));
            Game.OnTick += OnTick;
            Spellbook.OnCastSpell += Events.Spellbook_OnCastSpell;
        }

        static void OnTick(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Modes.Combo();
                break;
            }
            Menu();
            Functions.Activator();
            Functions.BaseCloak();
            Functions.StealthRecall();
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
            if (ItemsMenu["modeBOTRK"].Cast<ComboBox>().CurrentValue == 0)
            {
                ItemsMenu["hpBOTRK"].Cast<Slider>().IsVisible = false;
            }
            if (ItemsMenu["modeBOTRK"].Cast<ComboBox>().CurrentValue == 1)
            {
                ItemsMenu["hpBOTRK"].Cast<Slider>().IsVisible = true;
            }
        }
        //
        // Variables Below.-
        //
        public static int mItems(string text)
        {
            return ItemsMenu[text].Cast<ComboBox>().CurrentValue;
        }
        public static int mItemsSlider(string text)
        {
            return ItemsMenu[text].Cast<Slider>().CurrentValue;
        }
        public static bool mCloak
        {
            get { return ExtraMenu["box3"].Cast<KeyBind>().CurrentValue; }
        }
        public static int mBaseCloak
        {
            get { return ExtraMenu["box4"].Cast<ComboBox>().CurrentValue; }
        }
        public static bool mComboMenuCheckBox(string text)
        {
            return ComboMenu[text].Cast<CheckBox>().CurrentValue;
        }
        public static int mComboMenuComboBox(string text)
        {
            return ComboMenu[text].Cast<ComboBox>().CurrentValue;
        }
        public static int mComboMenuSlider(string text)
        {
            return ComboMenu[text].Cast<Slider>().CurrentValue;
        }
    }
}
