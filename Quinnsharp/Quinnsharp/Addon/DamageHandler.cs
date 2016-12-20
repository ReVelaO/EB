using EloBuddy;

namespace Quinnsharp.Addon
{
    public static class DamageHandler
    {
        private static AIHeroClient Quinn => Player.Instance;

        public static float GetQDamage(this Obj_AI_Base obj)
        {
            if (SpellHandler.Q.IsReady())
            {
                int index = Player.GetSpell(SpellSlot.Q).Level - 1;
                var Base = new float[] { 20, 45, 70, 95, 120 }[index];
                var BonusAD = new float[] { 0.80f, 0.90f, 1f, 1.10f, 1.20f }[index] * Quinn.FlatPhysicalDamageMod;
                var BonusAP = 0.50f * Quinn.FlatMagicDamageMod;

                return Base + BonusAD + BonusAP;
            }

            return 0;
        }

        public static float GetEDamage(this Obj_AI_Base obj)
        {
            if (SpellHandler.E.IsReady())
            {
                int index = Player.GetSpell(SpellSlot.E).Level - 1;
                var Base = new float[] { 40, 70, 100, 130, 160 }[index];
                var BonusAD = 0.20f * Quinn.FlatPhysicalDamageMod;

                return Base + BonusAD;
            }

            return 0;
        }
    }
}