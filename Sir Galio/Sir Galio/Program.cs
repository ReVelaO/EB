namespace Galio
{
    using System;
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu.Values;
    using static Combo;
    using static DamageHandler;
    using static Laneclear;
    using static Jungleclear;
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoading;
        }

        public static void OnLoading(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Galio) return;
            Chat.Print("Show them Fear");

            //Events
            Menus.CreateMenu();
            Spells.InitializeSpells();
            Game.OnTick += OnTick;
            DamageHandlerLoad();
            Smite.LoadSmite();
            Drawings.CreateDrawings();
        }

        static void OnTick(EventArgs args)
        {
            Save();
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                ComboExecute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneExecute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleExecute();
            }
        }
        static void Save()
        {
            if (Menus.WMenu["one"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance.IsInDanger(Menus.WMenu["onesli"].Cast<Slider>().CurrentValue) && Spells.W.IsReady())
                {
                    Spells.W.Cast(Player.Instance);
                }
            }
            if (Menus.WMenu["two"].Cast<CheckBox>().CurrentValue)
            {
                var ally = GetAllyInDanger(Spells.W.Range, Menus.WMenu["twosli"].Cast<Slider>().CurrentValue);
                if (ally != null)
                {
                    if (Spells.W.IsReady())
                    {
                        Spells.W.Cast(ally);
                    }
                }
            }
        }
    }
}
