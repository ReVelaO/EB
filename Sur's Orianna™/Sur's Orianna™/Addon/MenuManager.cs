namespace Orianna.Addon
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class MenuManager
    {
        public static Menu menu, mcombo, mshield, mlane, mjungle, mflee, mshock, mdrawings, msum;

        public static void Load()
        {
            menu = MainMenu.AddMenu("Orianna", "index0");
            menu.AddGroupLabel("[FREE] Orianna Addon by Surprise");

            mflee = menu.AddSubMenu("Free");
            mflee.Add("w", new CheckBox("Use W"));

            mcombo = menu.AddSubMenu("Combo");
            mcombo.Add("q", new CheckBox("Use Q"));
            mcombo.Add("w", new CheckBox("Use W"));
            mcombo.Add("r", new CheckBox("Use R"));
            mcombo.Add("re", new CheckBox("Use R [Execute]"));
            mcombo.Add("rblock", new CheckBox("BLOCK [R]", false));
            mcombo.AddSeparator(8);
            mcombo.Add("minr", new Slider("Min. Enemies [R]", 2, 1, 5));

            mshield = menu.AddSubMenu("Shield [E]");
            mshield.AddLabel("On me Settings");
            mshield.Add("b", new CheckBox("Shield me against AA by Enemy Heroes"));
            mshield.AddSeparator(8);
            mshield.Add("m", new CheckBox("Shield me against AA by Enemy Minions"));
            mshield.AddSeparator(8);
            mshield.AddLabel("On Ally Settings");
            mshield.Add("ba", new CheckBox("Shield ally against AA by Enemy Heroes"));

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

            msum = menu.AddSubMenu("Summoners");
            if (UtilsManager.HasIgnite)
            {
                msum.Add("i", new CheckBox("Auto Ignite"));
            }

            mshock = menu.AddSubMenu("Shockwave Flash");
            mshock.AddLabel("Will check for the most closests enemy near orianna.");
            mshock.AddSeparator(8);
            mshock.Add("sf", new KeyBind("Flash R", false, KeyBind.BindTypes.HoldActive, 'N'));
            mshock.AddSeparator(5);
            mshock.Add("sfr", new CheckBox("Draw Flash R"));
        }
    }
}