using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace DarkRyze
{
    class StateManager
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static float IsInRange()
        {
            if (Program.W.IsReady())
            {
                return Program.W.Range;
            }
            if (Program.Q.IsReady())
            {
                return Program.Q.Range;
            }
            if (Program.E.IsReady())
            {
                return Program.E.Range;
            }
            return _Player.GetAutoAttackRange();
        }

        public static void Combo()
        {
            var target = TargetSelector2.GetTarget(IsInRange(), DamageType.Magical);

            if (Program.ComboMenu["WU"].Cast<CheckBox>().CurrentValue && Program.W.IsReady())
            {
                Program.W.Cast(target);
            }

            else if (Program.ComboMenu["QU"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
            {
                Program.Q.Cast(target);
            }

            else if (Program.ComboMenu["EU"].Cast<CheckBox>().CurrentValue && Program.E.IsReady())
            {
                Program.E.Cast(target);
            }
            else if (Program.ComboMenu["RU"].Cast<CheckBox>().CurrentValue && Program.R.IsReady())
            {
                Program.R.Cast();
            }
        }
    }
}
