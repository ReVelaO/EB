using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Sur_s_Brand_.Addon
{
    public class Settings
    {
        static Menu menu, ComboMenu, DrawingsMenu;
        public static void Load()
        {
            menu = MainMenu.AddMenu("Sur's Brand™", "brand");
            menu.AddGroupLabel("[FREE] Brand Addon");
            menu.AddLabel("Made by Surprise");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            ComboMenu.AddGroupLabel("Sur's Brand™ - Combo");
            usarQ = ComboMenu.Add("QU", new CheckBox("Use Q"));
            usarQ1 = ComboMenu.Add("Q1", new ComboBox("Q Mode:", 0, "Fast", "Stun"));
            usarW = ComboMenu.Add("WU", new CheckBox("Use W"));
            usarE = ComboMenu.Add("EU", new CheckBox("Use E"));
            usarR = ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.AddSeparator(10);
            sliderR = ComboMenu.Add("MR", new Slider("Min. Enemies [R]", 1, 1, 5));
            sliderA = ComboMenu.Add("HA", new Slider("Hitchance {0}% Spells", 1, 1));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");
            DrawingsMenu.AddGroupLabel("Sur's Brand™ - Drawings");
            dibujarQ = DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            dibujarW = DrawingsMenu.Add("DW", new CheckBox("Draw W"));
            dibujarE = DrawingsMenu.Add("DE", new CheckBox("Draw E"));
            dibujarR = DrawingsMenu.Add("DR", new CheckBox("Draw R"));
        }
        public static CheckBox usarQ;
        public static ComboBox usarQ1;
        public static CheckBox usarW;
        public static CheckBox usarE;
        public static CheckBox usarR;
        public static Slider sliderR;
        public static Slider sliderA;
        public static CheckBox dibujarQ;
        public static CheckBox dibujarW;
        public static CheckBox dibujarE;
        public static CheckBox dibujarR;
    }
}
