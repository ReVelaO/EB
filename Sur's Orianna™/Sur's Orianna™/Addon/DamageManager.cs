namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;

    public static class DamageManager
    {
        private static AIHeroClient Orianna => Player.Instance;

        public static float HPrediction(Obj_AI_Base e, int d = 250) => Prediction.Health.GetPrediction(e, d);

        public static float Q(Obj_AI_Base e)
        {
            float d = 0;

            if (SpellManager.Q.IsReady())
            {
                var index = Player.GetSpell(SpellSlot.Q).Level - 1;
                var Base = new float[] { 60, 90, 120, 150, 180 }[index];
                var BonusAP = 0.5f * Orianna.FlatMagicDamageMod;

                d = Base + BonusAP;
            }

            return Orianna.CalculateDamageOnUnit(e, DamageType.Magical, d);
        }

        public static float W(Obj_AI_Base e)
        {
            float d = 0;

            if (SpellManager.W.IsReady())
            {
                var index = Player.GetSpell(SpellSlot.W).Level - 1;
                var Base = new float[] { 75, 115, 160, 205, 250 }[index];
                var BonusAP = 0.7f * Orianna.FlatMagicDamageMod;

                d = Base + BonusAP;
            }

            return Orianna.CalculateDamageOnUnit(e, DamageType.Magical, d);
        }

        public static float R(Obj_AI_Base e)
        {
            float d = 0;

            if (SpellManager.R.IsReady())
            {
                var index = Player.GetSpell(SpellSlot.R).Level - 1;
                var Base = new float[] { 150, 225, 300 }[index];
                var BonusAP = 0.7f * Orianna.FlatMagicDamageMod;

                d = Base + BonusAP;
            }

            return Orianna.CalculateDamageOnUnit(e, DamageType.Magical, d);
        }
    }
}