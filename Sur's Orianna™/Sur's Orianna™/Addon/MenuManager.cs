namespace Orianna.Addon
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    internal class MenuManager
    {
        public static Menu menu, mcombo, mlane, mjungle, mdrawings;
        public static void Load()
        {
            menu = MainMenu.AddMenu("Orianna", "index0");
            menu.AddGroupLabel("[FREE] Orianna Addon by Surprise");

            mcombo = menu.AddSubMenu("Combo");
            mcombo.Add("q", new CheckBox("Use Q"));
            mcombo.Add("w", new CheckBox("Use W"));
            mcombo.Add("r", new CheckBox("Use R"));
            mcombo.Add("re", new CheckBox("Use R [Execute]"));
            mcombo.Add("rblock", new CheckBox("BLOCK [R]"));
            mcombo.AddSeparator(8);
            mcombo.Add("minr", new Slider("Min. Enemies [R]", 2, 1, 5));
            mcombo.AddSeparator(8);
            mcombo.Add("qh", new Slider("Spell Q Hitchance: {0}%", 72, 1));

            mlane = menu.AddSubMenu("Laneclear");
            mlane.Add("q", new CheckBox("Use Q"));
            mlane.Add("w", new CheckBox("Use W"));
            mlane.AddSeparator(8);
            mlane.Add("minq", new Slider("Spell Q: Hit {0} Minions", 3, 1, 4));
            mlane.AddSeparator(8);
            mlane.Add("minw", new Slider("Spell W: Hit {0} Minions", 3, 1, 6));
            mlane.AddSeparator(14);
            mlane.Add("mm", new CheckBox("Enable Mana-Manager"));
            mlane.AddSeparator(8);
            mlane.Add("mmsli", new Slider("Stop at {0}% Mana", 69, 1));

            mjungle = menu.AddSubMenu("Jungleclear");
            mjungle.Add("q", new CheckBox("Use Q"));
            mjungle.Add("w", new CheckBox("Use W"));

            mdrawings = menu.AddSubMenu("Drawings");
            mdrawings.Add("q", new CheckBox("Draw Q"));
            mdrawings.Add("ball", new CheckBox("Draw W/R in Ball"));
        }
    }
}
