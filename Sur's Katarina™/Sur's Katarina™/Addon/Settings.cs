using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Sur_s_Katarina_.Addon
{
    public class Settings
    {
        static Menu menu, ComboMenu, DrawingsMenu;
        public static void Load()
        {
            menu = MainMenu.AddMenu("Sur's Katarina™", "xinzhao");
            menu.AddGroupLabel("[FREE] Katarina Addon");
            menu.AddLabel("Made by Surprise");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            ComboMenu.AddGroupLabel("Sur's Katarina™ - Combo");
            usarQ = ComboMenu.Add("QU", new CheckBox("Use Q"));
            usarW = ComboMenu.Add("WU", new CheckBox("Use W"));
            usarE = ComboMenu.Add("EU", new CheckBox("Use E"));
            usarR = ComboMenu.Add("RU", new CheckBox("Use R"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");
            DrawingsMenu.AddGroupLabel("Sur's Katarina™ - Drawings");
            dibujarQ = DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            dibujarW = DrawingsMenu.Add("DW", new CheckBox("Draw W"));
            dibujarE = DrawingsMenu.Add("DE", new CheckBox("Draw E"));
            dibujarR = DrawingsMenu.Add("DR", new CheckBox("Draw R"));

        }
        static CheckBox usarQ;
        public static bool useQ { get { return usarQ.CurrentValue; } }
        static CheckBox usarW;
        public static bool useW { get { return usarW.CurrentValue; } }
        static CheckBox usarE;
        public static bool useE { get { return usarE.CurrentValue; } }
        static CheckBox usarR;
        public static bool useR { get { return usarR.CurrentValue; } }
        static CheckBox dibujarQ;
        public static bool DrawQ { get { return dibujarQ.CurrentValue; } }
        static CheckBox dibujarW;
        public static bool DrawW { get { return dibujarW.CurrentValue; } }
        static CheckBox dibujarE;
        public static bool DrawE { get { return dibujarE.CurrentValue; } }
        static CheckBox dibujarR;
        public static bool DrawR { get { return dibujarR.CurrentValue; } }
    }
}
