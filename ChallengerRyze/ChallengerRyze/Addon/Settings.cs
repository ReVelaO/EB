using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ChallengerRyze.Addon
{
    public static class Settings
    {
        public static bool useQ { get { return usarQ.CurrentValue; } }
        public static bool useW { get { return usarW.CurrentValue; } }
        public static bool useE { get { return usarE.CurrentValue; } }
        public static bool forceQ { get { return forzarQ.CurrentValue; } }
        public static bool drawQ { get { return dibujarQ.CurrentValue; } }
        public static bool drawWE { get { return dibujarWE.CurrentValue; } }
        public static int hitchanceQ { get { return porcentajeQ.CurrentValue; } }
        private static CheckBox usarQ;
        private static CheckBox usarW;
        private static CheckBox usarE;
        private static CheckBox forzarQ;
        private static CheckBox dibujarQ;
        private static CheckBox dibujarWE;
        private static Slider porcentajeQ;
        private static Menu menu, ComboMenu, DrawingsMenu, MiscMenu;
        public static void Load()
        {
            menu = MainMenu.AddMenu("Sur's Challenger Ryze™", "ryze");
            menu.AddGroupLabel("[FREE] Ryze Addon");
            menu.AddLabel("Made by Surprise");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            ComboMenu.AddGroupLabel("Addon - Combo Configuration");
            usarQ = ComboMenu.Add("QU", new CheckBox("Use Q"));
            forzarQ = ComboMenu.Add("fq", new CheckBox("Force Q Reset"));
            usarW = ComboMenu.Add("WU", new CheckBox("Use W"));
            usarE = ComboMenu.Add("EU", new CheckBox("Use E"));

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");
            DrawingsMenu.AddGroupLabel("Addon - Drawings Configuration");
            dibujarQ = DrawingsMenu.Add("DE", new CheckBox("Draw Q"));
            dibujarWE = DrawingsMenu.Add("DR", new CheckBox("Draw W/E"));

            MiscMenu = menu.AddSubMenu("Misc", "miscmenu");
            MiscMenu.AddGroupLabel("Addon - Hitchance Configuration.");
            porcentajeQ = MiscMenu.Add("hitchance_q", new Slider("Hitchance {0}% Q", 85, 1));
        }
    }
}
