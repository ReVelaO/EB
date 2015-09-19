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
    class RyzeFunctions
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static float IsInRange()
        {
            if (Program.Q.IsReady())
            {
                return Program.Q.Range;
            }
            return _Player.GetAutoAttackRange();
        }

        public static void PegarleAlQlo()
        {
            var QCHECK = Program.ComboMenu["QU"].Cast<CheckBox>().CurrentValue;
            var WCHECK = Program.ComboMenu["WU"].Cast<CheckBox>().CurrentValue;
            var ECHECK = Program.ComboMenu["EU"].Cast<CheckBox>().CurrentValue;
            var RCHECK = Program.ComboMenu["RU"].Cast<CheckBox>().CurrentValue;
            var QREADY = Program.Q.IsReady();
            var WREADY = Program.W.IsReady();
            var EREADY = Program.E.IsReady();
            var RREADY = Program.R.IsReady();
            var target = TS.GetTarget(IsInRange(), DamageType.Magical);

                if (WCHECK && WREADY)
                {
                    Program.W.Cast(target);
                }

                else if (QCHECK && QREADY && target.IsValidTarget(Program.Q.Range))
                {
                    Program.Q.Cast(target);
                }

                else if (ECHECK && EREADY)
                {
                    Program.E.Cast(target);
                }
                else if (RCHECK && RREADY)
                {
                    Program.R.Cast();
                }
        }

        public static void RobarWeas()
        {
            foreach (var target in HeroManager.Enemies.Where(hero => hero.Health <= RyzeCalcs.Q(hero)))
            if (Program.KSMenu["KSQ"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && !target.IsDead)
            {
                Program.Q.Cast(target);
            }

            foreach (var target in HeroManager.Enemies.Where(hero => hero.Health <= RyzeCalcs.W(hero)))
            if (Program.KSMenu["KSW"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && !target.IsDead)
            {
                Program.W.Cast(target);
            }

            foreach (var target in HeroManager.Enemies.Where(hero => hero.Health <= RyzeCalcs.E(hero)))
            if (Program.KSMenu["KSE"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && !target.IsDead)
            {
                    Program.E.Cast(target);
            }
        }

        public static void LastHit()
        {
            var source =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.IsEnemy && a.Distance(_Player) < IsInRange())
                    .OrderBy(a => a.Health)
                    .FirstOrDefault();

            if (Program.FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue && RyzeCalcs.Q(source) > source.Health && source.Distance(_Player) < Program.Q.Range && !source.IsDead)
            {
                Program.Q.Cast(source);
                return;
            }

            //if (Program.FarmMenu["LHQ"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady())
            //{
            //    var minion = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(a => a.IsEnemy && a.Health <= RyzeCalcs.Q(a));
            //    if (minion == null) return;
            //    Program.Q.Cast(minion);
            //}
        }

    }
}
