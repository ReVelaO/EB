using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Evelynn.Addon
{
    internal class EveMenu
    {
        public static CheckBox Cq;
        public static CheckBox Cw;
        public static CheckBox Ce;
        public static CheckBox Cr;
        public static CheckBox Jq;
        public static CheckBox Je;
        public static CheckBox JSm;
        public static CheckBox Dq;
        public static CheckBox De;
        public static CheckBox Dr;
        public static CheckBox DSt;
        public static CheckBox DSr;
        public static CheckBox autoR;
        private static Menu _menu;
        

        public static void Load()
        {
            _menu = MainMenu.AddMenu("Evelynn", "evestart");
            _menu.AddLabel("Sur's Series: Evelynn", 24);
            _menu.AddLabel("[FREE] Evelynn Addon & made by Surprise.", 16);
            _menu.AddGroupLabel("Combo");
            Cq = _menu.Add("ccq", new CheckBox("Use Q"));
            Cw = _menu.Add("ccw", new CheckBox("Use W"));
            Ce = _menu.Add("cce", new CheckBox("Use E"));
            Cr = _menu.Add("ccr", new CheckBox("Use R"));
            _menu.AddSeparator(10);
            _menu.AddGroupLabel("Jungle");
            Jq = _menu.Add("wwq", new CheckBox("Use Q"));
            Je = _menu.Add("wwe", new CheckBox("Use E"));
            JSm = _menu.Add("usmite", new CheckBox("Use Smite"));
            _menu.AddSeparator(10);
            _menu.AddGroupLabel("Drawings");
            Dq = _menu.Add("drawq", new CheckBox("Draw Q"));
            De = _menu.Add("drawe", new CheckBox("Draw E"));
            Dr = _menu.Add("drawr", new CheckBox("Draw R"));
            DSt = _menu.Add("dsmite", new CheckBox("Draw Smiteable"));
            _menu.AddSeparator(10);
            _menu.AddGroupLabel("Misc");
            autoR = _menu.Add("autoiautr", new CheckBox("Auto R Max-Shield"));

        }
    }
}