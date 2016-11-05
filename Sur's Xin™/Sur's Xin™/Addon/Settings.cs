using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Sur_s_Xin_.Addon
{
    internal class Settings
    {
        private static Menu _menu, _comboMenu, _drawingsMenu, _interrupterMenu, _jungleclearMenu;
        public static CheckBox UsarQ;
        public static CheckBox FixQ;
        public static CheckBox UsarW;
        public static CheckBox UsarE;
        public static CheckBox UsarEe;
        public static CheckBox UsarR;
        public static CheckBox JusarQ;
        public static CheckBox JusarW;
        public static CheckBox JusarE;
        public static CheckBox UsarSmite;
        public static CheckBox DrawSmite;
        public static CheckBox DibujarQ;
        public static CheckBox DibujarW;
        public static CheckBox DibujarE;
        public static CheckBox DibujarR;
        public static CheckBox InterR;
        public static CheckBox UsarBotrk;
        public static CheckBox UsarSable;
        public static CheckBox UsarHydra;
        public static CheckBox UsarTiamat;
        public static CheckBox Autsmite;

        public static void Load()
        {
            _menu = MainMenu.AddMenu("Sur's Xin™", "xinzhao");
            _menu.AddGroupLabel("[FREE] Xin Zhao Addon");
            _menu.AddLabel("Made by Surprise");

            _comboMenu = _menu.AddSubMenu("Combo", "combomenu");
            _comboMenu.AddGroupLabel("Sur's Xin™ - Combo");
            UsarQ = _comboMenu.Add("QU", new CheckBox("Reset AA with Q"));
            FixQ = _comboMenu.Add("QQU", new CheckBox("Fix AA Reset"));
            UsarW = _comboMenu.Add("WU", new CheckBox("Use W"));
            UsarE = _comboMenu.Add("EU", new CheckBox("Use E"));
            UsarEe = _comboMenu.Add("EEU", new CheckBox("BLOCK E in AA Range"));
            UsarR = _comboMenu.Add("RU", new CheckBox("Use R"));
            _comboMenu.AddSeparator(10);
            _comboMenu.AddGroupLabel("Sur's Xin™ - Offensive Items");
            UsarBotrk = _comboMenu.Add("BU", new CheckBox("Use BOTRK"));
            UsarSable = _comboMenu.Add("SU", new CheckBox("Use Bilgewater Cutlass"));
            UsarHydra = _comboMenu.Add("RHU", new CheckBox("Use Ravenous Hydra"));
            UsarTiamat = _comboMenu.Add("TU", new CheckBox("Use Tiamat"));

            _jungleclearMenu = _menu.AddSubMenu("Jungleclear", "jungleclearmenu");
            _jungleclearMenu.AddGroupLabel("Sur's Xin™ - Jungleclear");
            JusarQ = _jungleclearMenu.Add("jq", new CheckBox("Use Q"));
            JusarW = _jungleclearMenu.Add("jw", new CheckBox("Use W"));
            JusarE = _jungleclearMenu.Add("je", new CheckBox("Use E"));
            _jungleclearMenu.AddSeparator(10);
            _jungleclearMenu.AddGroupLabel("Sur's Xin™ - Smite");
            UsarSmite = _jungleclearMenu.Add("jsmite", new CheckBox("Use Smite"));
            Autsmite = _jungleclearMenu.Add("autossmite", new CheckBox("Auto Smite"));
            DrawSmite = _jungleclearMenu.Add("drawSmite", new CheckBox("Draw Smiteable"));
            _jungleclearMenu.AddLabel("Draw Smiteable:", 14);
            _jungleclearMenu.AddLabel("White circle: Is not killable by Smite", 10);
            _jungleclearMenu.AddLabel("Green circle: Is killable by Smite", 10);

            _drawingsMenu = _menu.AddSubMenu("Drawings", "drawingsmenu");
            _drawingsMenu.AddGroupLabel("Sur's Xin™ - Drawings");
            DibujarE = _drawingsMenu.Add("DE", new CheckBox("Draw E"));
            DibujarR = _drawingsMenu.Add("DR", new CheckBox("Draw R"));

            _interrupterMenu = _menu.AddSubMenu("Interrupter", "interruptermenu");
            _interrupterMenu.AddGroupLabel("Sur's Xin™ - Interrupter");
            InterR = _interrupterMenu.Add("intR", new CheckBox("Interrupt with R"));
        }
    }
}