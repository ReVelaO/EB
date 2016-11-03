using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace ChallengerRyze.Addon
{
    public static class Settings
    {
        private static CheckBox _usarQ;
        private static CheckBox _usarW;
        private static CheckBox _usarE;
        private static CheckBox _forzarQ;
        private static CheckBox _dibujarQ;
        private static CheckBox _dibujarWe;
        private static Slider _porcentajeQ;
        private static CheckBox _autoW;
        private static Menu _menu, _comboMenu, _drawingsMenu, _miscMenu, _laneclearmenu, _jungleclearmenu, _ksmenu;
        public static bool UseQ => _usarQ.CurrentValue;
        public static bool UseW => _usarW.CurrentValue;
        public static bool UseE => _usarE.CurrentValue;
        public static bool ForceQ => _forzarQ.CurrentValue;
        public static bool DrawQ => _dibujarQ.CurrentValue;
        public static bool DrawWe => _dibujarWe.CurrentValue;
        public static int HitchanceQ => _porcentajeQ.CurrentValue;
        public static bool AuttoW => _autoW.CurrentValue;

        public static CheckBox _ignite { get; private set; }
        public static CheckBox _kse { get; private set; }
        public static CheckBox _ksw { get; private set; }
        public static CheckBox _ksq { get; private set; }

        public static void Load()
        {
            _menu = MainMenu.AddMenu("Sur's Ryze™", "ryze");
            _menu.AddGroupLabel("[FREE] Ryze Addon");
            _menu.AddLabel("Made by Surprise");

            _comboMenu = _menu.AddSubMenu("Combo", "combomenu");
            _usarQ = _comboMenu.Add("QU", new CheckBox("Use Q"));
            _forzarQ = _comboMenu.Add("fq", new CheckBox("Force Q Reset"));
            _usarW = _comboMenu.Add("WU", new CheckBox("Use W"));
            _usarE = _comboMenu.Add("EU", new CheckBox("Use E"));

            _ksmenu = _menu.AddSubMenu("Kill Steal", "killmenu");
            _ksq = _ksmenu.Add("ksQ", new CheckBox("Use Q"));
            _ksw = _ksmenu.Add("ksw", new CheckBox("Use W"));
            _kse = _ksmenu.Add("kse", new CheckBox("Use E"));
            _ignite = _ksmenu.Add("uignit", new CheckBox("Use Ignite"));

            _drawingsMenu = _menu.AddSubMenu("Drawings", "drawingsmenu");
            _dibujarQ = _drawingsMenu.Add("DE", new CheckBox("Draw Q"));
            _dibujarWe = _drawingsMenu.Add("DR", new CheckBox("Draw W/E"));

            _miscMenu = _menu.AddSubMenu("Misc", "miscmenu");
            _miscMenu.AddLabel("HitChance: 0 = Low, 1 = Medium, 2 = High", 18);
            _porcentajeQ = _miscMenu.Add("hitchance_q", new Slider("Hitchance Q", 2, 0, 2));
        }
    }
}