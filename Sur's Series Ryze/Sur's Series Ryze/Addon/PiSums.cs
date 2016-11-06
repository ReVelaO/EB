using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Spells;

namespace Pitufo.Addon
{
    internal static class PiSums
    {
        public static Spell.Targeted Ignite;
        public static void Load()
        {
            Ignite = new Spell.Targeted(SummonerSpells.Ignite.Slot, 600);
        }

        public static bool IsIgniteReady => Player.CanUseSpell(Ignite.Slot) == SpellState.Ready;
        public static float GetDamage(Obj_AI_Base random, DamageLibrary.SummonerSpells offensivesum)
        {
            return Player.Instance.GetSummonerSpellDamage(random, offensivesum);
        }
        public static float GetHealthPrediction(Obj_AI_Base enemy, int delay)
        {
            return Prediction.Health.GetPrediction(enemy, delay);
        }
    }
}
