using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Sur_s_Brand_.Addon
{
    public class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;
        public static void Get()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1600, 60);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 5, 1400, 250);
            E = new Spell.Targeted(SpellSlot.E, 640);
            R = new Spell.Targeted(SpellSlot.R, 750);
        }
    }
}
