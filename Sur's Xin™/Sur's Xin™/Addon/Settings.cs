﻿using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Sur_s_Xin_.Addon
{
    public class Settings
    {
        static Menu menu, ComboMenu, DrawingsMenu, InterrupterMenu;
        static AIHeroClient Xin => ObjectManager.Player;
        public static void Load()
        {
            menu = MainMenu.AddMenu("Sur's Xin™", "xinzhao");
            menu.AddGroupLabel("[FREE] Xin Zhao Addon");
            menu.AddLabel("Made by Surprise");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            ComboMenu.AddGroupLabel("Sur's Xin™ - Combo");
            usarQ = ComboMenu.Add("QU", new CheckBox("Reset AA with Q"));
            usarW = ComboMenu.Add("WU", new CheckBox("Use W"));
            usarE = ComboMenu.Add("EU", new CheckBox("Use E"));
            usarEE = ComboMenu.Add("EEU", new CheckBox("BLOCK E in AA Range"));
            usarR = ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.AddSeparator(10);
            ComboMenu.AddGroupLabel("Sur's Xin™ - Offensive Items");
            usarBOTRK = ComboMenu.Add("BU", new CheckBox("Use BOTRK"));
            usarSABLE = ComboMenu.Add("SU", new CheckBox("Use Bilgewater Cutlass"));
            usarHYDRA = ComboMenu.Add("RHU", new CheckBox("Use Ravenous Hydra"));
            usarTIAMAT = ComboMenu.Add("TU", new CheckBox("Use Tiamat"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");
            DrawingsMenu.AddGroupLabel("Sur's Xin™ - Drawings");
            dibujarE = DrawingsMenu.Add("DE", new CheckBox("Draw E"));
            dibujarR = DrawingsMenu.Add("DR", new CheckBox("Draw R"));

            InterrupterMenu = menu.AddSubMenu("Interrupter", "interruptermenu");
            InterrupterMenu.AddGroupLabel("Sur's Xin™ - Interrupter");
            interR = InterrupterMenu.Add("intR", new CheckBox("Interrupt with R"));

        }
        public static CheckBox usarQ;
        public static CheckBox usarW;
        public static CheckBox usarE;
        public static CheckBox usarEE;
        public static CheckBox usarR;
        public static CheckBox dibujarQ;
        public static CheckBox dibujarW;
        public static CheckBox dibujarE;
        public static CheckBox dibujarR;
        public static CheckBox interR;
        public static CheckBox usarBOTRK;
        public static CheckBox usarSABLE;
        public static CheckBox usarHYDRA;
        public static CheckBox usarTIAMAT;
    }
}
