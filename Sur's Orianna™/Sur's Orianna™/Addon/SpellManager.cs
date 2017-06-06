namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    public static class SpellManager
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W, R;
        public static Spell.Targeted E;

        public static void Load()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 810, SkillShotType.Circular, 1, 1200, 175);
            Q.AllowedCollisionCount = -1;
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 1100);
            R = new Spell.Active(SpellSlot.R);
        }
    }
}