using EloBuddy;
using EloBuddy.SDK;

namespace La_Leyenda_de_Caitlyn.Addon
{
    public static class DamageHandler
    {
        private static AIHeroClient Caitlyn => Player.Instance;

        public static float PeaceMaker(Obj_AI_Base t)
        {
            float d = 0;
            if (SpellHandler.Q.IsReady())
            {
                var index = Player.GetSpell(SpellSlot.Q).Level - 1;
                var Base = new float[] { 30, 70, 110, 150, 190 }[index];
                var BonusAD = new float[] { 1.3f, 1.4f, 1.5f, 1.6f, 1.7f }[index] * Caitlyn.FlatPhysicalDamageMod;

                d = Base + BonusAD;
            }
            return Caitlyn.CalculateDamageOnUnit(t, DamageType.Physical, d);
        }

        public static float CaliberNet(Obj_AI_Base t)
        {
            float d = 0;
            if (SpellHandler.E.IsReady())
            {
                var index = Player.GetSpell(SpellSlot.E).Level - 1;
                var Base = new float[] { 70, 110, 150, 190, 230 }[index];
                var BonusAP = 0.80f * Caitlyn.FlatMagicDamageMod;

                d = Base + BonusAP;
            }
            return Caitlyn.CalculateDamageOnUnit(t, DamageType.Magical, d);
        }

        public static float FinalShot(Obj_AI_Base t)
        {
            float d = 0;
            if (SpellHandler.R.IsReady())
            {
                var index = Player.GetSpell(SpellSlot.R).Level - 1;
                var Base = new float[] { 250, 475, 700 }[index];
                var BonusAD = 2f * Caitlyn.FlatPhysicalDamageMod;

                d = Base + BonusAD;
            }
            return Caitlyn.CalculateDamageOnUnit(t, DamageType.Physical, d);
        }
    }
}