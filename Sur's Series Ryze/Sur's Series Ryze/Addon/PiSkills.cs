using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Pitufo.Addon
{
    internal class PiSkills
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot Q2;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.SimpleSkillshot R;
        public static void Load()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60)
            {
                AllowedCollisionCount = 0
            };
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60)
            {
                AllowedCollisionCount = -1
            };
            W = new Spell.Targeted(SpellSlot.W, 615);
            E = new Spell.Targeted(SpellSlot.E, 615);
            R = new Spell.SimpleSkillshot(SpellSlot.R, 3000);

        }
    }
}
