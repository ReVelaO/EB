using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ChallengerRyze.Addon
{
    public static class Spells
    {
        public static bool IsQReady { get { return Player.CanUseSpell(SpellSlot.Q) == SpellState.Ready; } }
        public static bool IsWReady { get { return Player.CanUseSpell(SpellSlot.W) == SpellState.Ready; } }
        public static bool IsEReady { get { return Player.CanUseSpell(SpellSlot.E) == SpellState.Ready; } }
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static void Get()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60);
            W = new Spell.Targeted(SpellSlot.W, 615);
            E = new Spell.Targeted(SpellSlot.E, 615);
        }
    }
}
