using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Twitchsharp.Addon
{
    public static class MenuHandler
    {
        public static Menu main, combo, laneclear, jungleclear, killsteal, drawings, misc;

        public static void Load()
        {
            main = MainMenu.AddMenu("Twitch#", "index");

            combo = main.AddSubMenu("Combo");
            combo.Add("q", new CheckBox("Use Q"));
            combo.Add("w", new CheckBox("Use W"));
            combo.Add("e", new CheckBox("Use E"));
            combo.Add("r", new CheckBox("Use R"));
            combo.AddSeparator(8);
            combo.AddLabel("R Adjust (Alone)");
            combo.Add("multipler", new Slider("AA Multipler", 4, 3, 6));
            combo.AddLabel("This will check if killable by aa dmg * aa times in extended range.");
            combo.Add("nm", new CheckBox("Disable Multipler", false));
            /*combo.AddSeparator(8);
            combo.Add("allowa", new Slider("Allies Allowed (Alone)", 0, 0, 4));
            combo.Add("allowe", new Slider("Enemies Allies Allowed (Alone)", 0, 0, 4));*/
            combo.AddSeparator(8);
            combo.AddLabel("R Adjust (Teamfights)");
            combo.Add("tf", new CheckBox("My Team > Enemy Team"));

            laneclear = main.AddSubMenu("Laneclear");
            laneclear.Add("amc", new CheckBox("Automated Mana Control"));
            laneclear.Add("sm", new Slider("Save {0}% Mana", 60, 1));
            laneclear.AddSeparator(14);
            laneclear.Add("w", new CheckBox("Use W"));
            laneclear.Add("whit", new Slider("Hit {0} units", 4, 1, 8));
            laneclear.AddSeparator(14);
            laneclear.Add("e", new CheckBox("Use E"));
            laneclear.Add("ehit", new Slider("Hit {0} units", 4, 1, 8));
            laneclear.Add("estacks", new Slider("At {0} stacks", 3, 1, 4));

            jungleclear = main.AddSubMenu("Jungleclear");
            jungleclear.Add("w", new CheckBox("Use W"));
            jungleclear.Add("e", new CheckBox("Use E"));
            jungleclear.AddSeparator(8);
            jungleclear.Add("erb", new CheckBox("Steal Red & Blue"));
            jungleclear.Add("ebd", new CheckBox("Steal Baron & Dragons"));

            killsteal = main.AddSubMenu("Kill Steal");
            killsteal.Add("aa", new CheckBox("Auto AA"));
            killsteal.Add("e", new CheckBox("Auto E"));

            drawings = main.AddSubMenu("Drawings");
            drawings.Add("aa", new CheckBox("Draw Real AA Range"));
            drawings.AddSeparator(8);
            drawings.Add("w", new CheckBox("Draw W"));
            drawings.Add("e", new CheckBox("Draw E"));
            drawings.AddSeparator(8);
            drawings.Add("di", new CheckBox("Draw Damage Indicator [E Damage]"));
            
            misc = main.AddSubMenu("Misc");
            misc.AddLabel("B with Q");
            misc.Add("QRecall", new CheckBox("Active"));
            misc.Add("QRecall2", new KeyBind("Q Recall Key", false, KeyBind.BindTypes.HoldActive, 'B'));
        }
    }
}
