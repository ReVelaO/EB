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
        public static CheckBox AutoR;
        private static Menu _menu;

        public static CheckBox Lq { get; private set; }
        public static CheckBox Le { get; private set; }
        public static CheckBox Lmm { get; private set; }
        public static Slider LmmS { get; private set; }
        public static CheckBox Llane { get; private set; }
        public static CheckBox Kq { get; private set; }
        public static CheckBox Ke { get; private set; }
        public static CheckBox Ki { get; private set; }

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
            _menu.AddSeparator(18);
            _menu.AddGroupLabel("Jungle");
            Jq = _menu.Add("wwq", new CheckBox("Use Q"));
            Je = _menu.Add("wwe", new CheckBox("Use E"));
            JSm = _menu.Add("usmite", new CheckBox("Use Smite"));
            _menu.AddSeparator(18);
            _menu.AddGroupLabel("Laneclear");
            Llane = _menu.Add("logicl", new CheckBox("Smart clear"));
            Lq = _menu.Add("llq", new CheckBox("Use Q"));
            Le = _menu.Add("lle", new CheckBox("Use E"));
            Lmm = _menu.Add("llmm", new CheckBox("Use Manamanager"));
            LmmS = _menu.Add("llmmS", new Slider("Stop at {0}% mana", 65, 1));
            _menu.AddSeparator(18);
            _menu.AddGroupLabel("Kill Steal");
            Kq = _menu.Add("kkkq", new CheckBox("Auto Q"));
            Ke = _menu.Add("kkke", new CheckBox("Auto E"));
            Ki = _menu.Add("kkkki", new CheckBox("Use Ignite"));
            _menu.AddSeparator(18);
            _menu.AddGroupLabel("Drawings");
            Dq = _menu.Add("drawq", new CheckBox("Draw Q"));
            De = _menu.Add("drawe", new CheckBox("Draw E"));
            Dr = _menu.Add("drawr", new CheckBox("Draw R"));
            DSt = _menu.Add("dsmite", new CheckBox("Draw Smiteable"));
            _menu.AddSeparator(18);
            _menu.AddGroupLabel("Misc");
            AutoR = _menu.Add("autoiautr", new CheckBox("Auto R Max-Shield"));

        }
    }
}