using System;
using EloBuddy;
using EloBuddy.SDK;
using Color = System.Drawing.Color;

namespace Sur_s_Katarina_.Addon
{
    public class Events
    {
        private static AIHeroClient Kata => ObjectManager.Player;
        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            Spellbook.OnCastSpell += OnCastSpell;
            Obj_AI_Base.OnBuffLose += endR;
            Player.OnIssueOrder += OnIssueOrder;
        }
        static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }
            Orb.Portonazo.Load();
        }
        static void OnDraw(EventArgs args)
        {
            if (Settings.DrawQ && Spells.Q.IsReady()) { Spells.Q.DrawRange(Color.LightPink); }
            if (Settings.DrawW && Spells.W.IsReady()) { Spells.W.DrawRange(Color.DarkRed); }
            if (Settings.DrawE && Spells.E.IsReady()) { Spells.E.DrawRange(Color.MediumVioletRed); }
            if (Settings.DrawR && Spells.R.IsReady()) { Spells.R.DrawRange(Color.DarkRed); }
        }
        public static bool ultimate = false;
        static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (ultimate == true)
            {
                args.Process = false;
            }
            if (ultimate == true)
            {

            }
            else if (ultimate == false)
            {
                args.Process = true;
            }
        }
        static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe)
            {
                if (args.Slot == SpellSlot.R)
                {
                    ultimate = true;
                }
            }
        }
        static void endR(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.Buff.Name.ToLower() == "katarinarsound")
                {
                    ultimate = false;
                }
            }
        }
    }
}
