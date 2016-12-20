using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Quinnsharp.Addon
{
    public static class SpellHandler
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W, R;
        public static Spell.Targeted E;

        public static void Load()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1550, 60);
            W = new Spell.Active(SpellSlot.W, 2100);
            E = new Spell.Targeted(SpellSlot.E, 735);
            R = new Spell.Active(SpellSlot.R);

            Q.AllowedCollisionCount = 0;
        }
    }
}