using System;
using EloBuddy;
using EloBuddy.SDK;

namespace Evelynn.Addon
{
    internal class EveDamages
    {
        private static AIHeroClient Evelynn => ObjectManager.Player;

        public static float Smite(Obj_AI_Base enemy)
        {
            return Player.Instance.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Smite);
        }

        public static float Q(Obj_AI_Base enemy)
        {
            float dmg = 0;
            if (EveSpells.Q.IsReady())
            {
                dmg +=
                    new float[] {40, 50, 60, 70, 80}[EveSpells.Q.Level - 1] +
                    new float[] {50, 55, 60, 65, 70}[EveSpells.Q.Level - 1]/100*Evelynn.FlatPhysicalDamageMod +
                    new float[] {35, 40, 45, 50, 55}[EveSpells.Q.Level - 1]/100*Evelynn.FlatMagicDamageMod;
            }
            return Evelynn.CalculateDamageOnUnit(enemy, DamageType.Magical, dmg);
        }

        public static float E(Obj_AI_Base enemy)
        {
            float dmg = 0;
            if (EveSpells.E.IsReady())
            {
                dmg += new float[] {35, 55, 75, 95, 115}[EveSpells.E.Level - 1]
                       + 0.5f*Evelynn.FlatPhysicalDamageMod
                       + 0.5f*Evelynn.FlatMagicDamageMod;
            }
            return Evelynn.CalculateDamageOnUnit(enemy, DamageType.Magical, dmg);
        }

        public static float R(Obj_AI_Base enemy)
        {
            double dmg = 0;
            if (EveSpells.R.IsReady())
            {
                dmg += enemy.Health*
                       (new[] {0.015f, 0.02f, 0.025f}[EveSpells.R.Level - 1] +
                        Math.Floor(Player.Instance.TotalMagicalDamage/100)/100);
            }
            return Evelynn.CalculateDamageOnUnit(enemy, DamageType.Magical, (float)dmg);
        }
    }
}