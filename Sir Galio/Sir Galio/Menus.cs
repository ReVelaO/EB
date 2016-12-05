namespace Galio
{
    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    internal static class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu WMenu;
        public static Menu ActivatorMenu;
        public static Menu MiscMenu;
        public static Menu DrawingsMenu;
        public static Menu LaneClearMenu;
        public static Menu JungleClearMenu;

        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Sir" + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower());
            ComboMenu = FirstMenu.AddSubMenu("Combo Settings");
            WMenu = FirstMenu.AddSubMenu("W Settings");
            ActivatorMenu = FirstMenu.AddSubMenu("Activator Settings");
            MiscMenu = FirstMenu.AddSubMenu("Misc Settings");
            DrawingsMenu = FirstMenu.AddSubMenu("Drawings Settings");
            LaneClearMenu = FirstMenu.AddSubMenu("Lane Settings");
            JungleClearMenu = FirstMenu.AddSubMenu("Jungle Settings");

            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("qhit", new Slider("Hitchance of Q", 60, 1));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("ehit", new Slider("Hitchance of E", 60, 1));
            ComboMenu.Add("R", new CheckBox("Use R"));
            ComboMenu.Add("REnemies", new Slider("Enemies in Range to use R", 3, 1, 5));

            WMenu.AddGroupLabel("W Configuration");
            WMenu.Add("one", new CheckBox("Protect Me"));
            WMenu.AddSeparator(8);
            WMenu.Add("onesli", new Slider("Protect at {0}% HP", 60, 1));
            WMenu.AddSeparator(14);
            WMenu.Add("two", new CheckBox("Protect Ally"));
            WMenu.AddSeparator(8);
            WMenu.Add("twosli", new Slider("Protect at {0}% HP", 60, 1));

            LaneClearMenu.AddGroupLabel("Laneclear");
            LaneClearMenu.Add("Q", new CheckBox("Use Q"));
            LaneClearMenu.Add("qmana", new Slider("Minimum Mana for Q Cast", 30, 1, 99));
            LaneClearMenu.Add("E", new CheckBox("Use E"));
            LaneClearMenu.Add("emana", new Slider("Minimum Mana for E Cast", 30, 1, 99));

            JungleClearMenu.AddGroupLabel("Jungleclear");
            JungleClearMenu.Add("Q", new CheckBox("Use Q"));
            JungleClearMenu.Add("qmana", new Slider("Minimum Mana for Q Cast", 30, 1, 99));
            JungleClearMenu.Add("E", new CheckBox("Use E"));
            JungleClearMenu.Add("emana", new Slider("Minimum Mana for E Cast", 30, 1, 99));

            DrawingsMenu.AddGroupLabel("Drawings");
            DrawingsMenu.Add("qdraw", new CheckBox("Draw Q Range"));
            DrawingsMenu.Add("edraw", new CheckBox("Draw E Range"));
            DrawingsMenu.Add("rdraw", new CheckBox("Draw R Range"));
        }
    }
}
