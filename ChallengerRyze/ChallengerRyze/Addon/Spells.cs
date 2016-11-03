using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Spells;
using EloBuddy.SDK.Enumerations;

namespace ChallengerRyze.Addon
{
    public static class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Targeted Ignite;

        public static void Get()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60);
            W = new Spell.Targeted(SpellSlot.W, 615);
            E = new Spell.Targeted(SpellSlot.E, 615);
            Ignite = new Spell.Targeted(SummonerSpells.Ignite.Slot, 600);

            Q.AllowedCollisionCount = 0;
        }

        private static HitChance hitchanceq()
        {
            var mode = Settings.HitchanceQ;
            switch (mode)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
            }
            return HitChance.Medium;
        }
        public static void CastQ(Obj_AI_Base enemy)
        {
            var prediction = Q.GetPrediction(enemy);
            if (Q.IsReady() 
                && enemy.IsValidTarget(1000)
                && prediction.HitChance >= hitchanceq())
                Q.Cast(prediction.CastPosition);
        }

        public static void CastW(Obj_AI_Base enemy)
        {
            if (W.IsReady() 
                && enemy.IsValidTarget(615))
                W.Cast(enemy);
        }

        public static void CastE(Obj_AI_Base enemy)
        {
            if (E.IsReady() 
                && enemy.IsValidTarget(615))
                E.Cast(enemy);
        }
        public static float GetHealthPrediction(Obj_AI_Base enemy, int delay)
        {
            return Prediction.Health.GetPrediction(enemy, delay);
        }
    }
}