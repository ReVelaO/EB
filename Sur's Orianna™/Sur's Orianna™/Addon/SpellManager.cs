namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    internal class SpellManager
    {
        public static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 1, 1200, 175) { AllowedCollisionCount = -1 };

        public static Spell.Active W = new Spell.Active(SpellSlot.W);

        public static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 1100);

        public static Spell.Active R = new Spell.Active(SpellSlot.R);

        public static uint ActiveRange = Q.Range + 275;
    }
}