namespace Galio
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    internal class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Skillshot E;
        public static Spell.Active R;

        public static void InitializeSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Circular, 500, 1300, 120);
            W = new Spell.Targeted(SpellSlot.W, 800);
            E = new Spell.Skillshot(SpellSlot.E, 800, SkillShotType.Linear, 500, 1200, 140);
            R = new Spell.Active(SpellSlot.R, 500);
        }
    }
}
