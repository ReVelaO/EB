namespace Orianna.Addon
{
    using EloBuddy;

    using EloBuddy.SDK;

    internal class DamageManager
    {
        public static float HPrediction(Obj_AI_Base e, int d = 250) => Prediction.Health.GetPrediction(e, d);

        public static float Q(Obj_AI_Base e)
        {
            float d = 0;
            if (SpellManager.Q.IsReady())
            {
                d = new float[] { 60, 90, 120, 150, 180 }[Player.GetSpell(SpellSlot.Q).Level - 1] + (0.5f * Player.Instance.FlatMagicDamageMod);
            }
            return Player.Instance.CalculateDamageOnUnit(e, DamageType.Magical, d);
        }

        public static float W(Obj_AI_Base e)
        {
            float d = 0;
            if (SpellManager.Q.IsReady())
            {
                d = new float[] { 75, 115, 160, 205, 250 }[Player.GetSpell(SpellSlot.Q).Level - 1] + (0.7f * Player.Instance.FlatMagicDamageMod);
            }
            return Player.Instance.CalculateDamageOnUnit(e, DamageType.Magical, d);
        }

        public static float R(Obj_AI_Base e)
        {
            float d = 0;
            if (SpellManager.R.IsReady())
            {
                d = new float[] { 150, 225, 300 }[Player.GetSpell(SpellSlot.R).Level - 1] + (0.7f * Player.Instance.FlatMagicDamageMod);
            }
            return Player.Instance.CalculateDamageOnUnit(e, DamageType.Magical, d);
        }
    }
}
