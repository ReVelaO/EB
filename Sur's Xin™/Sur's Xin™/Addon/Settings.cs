using EloBuddy;
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
            usarR = ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.AddSeparator(10);

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
        public static CheckBox usarR;
        public static CheckBox dibujarQ;
        public static CheckBox dibujarW;
        public static CheckBox dibujarE;
        public static CheckBox dibujarR;
        public static CheckBox interR;
    }
}
