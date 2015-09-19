using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace DarkRyze
{

    class RyzeCalcs
    {
        private static AIHeroClient yo { get { return ObjectManager.Player; } }
        public static double Q(Obj_AI_Base target)
        {
            return yo.CalculateDamageOnUnit(target, DamageType.Magical,
                (float)(new double[] { 60, 85, 110, 135, 160 }[Program.Q.Level] + 0.55 * yo.FlatMagicDamageMod + new double[] { 2, 2.5, 3.0, 3.5, 4.0 }[Program.Q.Level] /100 * yo.MaxMana));                  
        }

        public static float W(Obj_AI_Base target)
        {
            return yo.CalculateDamageOnUnit(target, DamageType.Magical,
                (float) (new [] { 80, 100, 120, 140, 160 }[Program.W.Level] + (0.4 * yo.FlatMagicDamageMod) + (0.02 * yo.MaxMana)));                    
        }

        public static float E(Obj_AI_Base target)
        {
            return yo.CalculateDamageOnUnit(target, DamageType.Magical,
                (float) (new [] { 36, 52, 68, 84, 100 }[Program.E.Level] + (0.2 * yo.FlatMagicDamageMod) + (0.025 * yo.MaxMana)));                     
        }
    }
}
