using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace La_Leyenda_de_Caitlyn.Addon
{
    public static class SpellHandler
    {
        public static Spell.Skillshot Q, W, E, E1, RP;

        public static Spell.Targeted R;

        public static void Load()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 625, 2200, 90)
            {
                AllowedCollisionCount = -1,
                MinimumHitChance = HitChance.Medium
            };
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, Brain.W.CastDelay, Brain.W.Speed, Brain.W.Width)
            {
                AllowedCollisionCount = -1,
                MinimumHitChance = HitChance.Medium
            };
            E = new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Linear, 125, 2000, 80)
            {
                AllowedCollisionCount = 0
            };
            E1 = new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Linear, 125, 2000, 80)
            {
                AllowedCollisionCount = -1
            };
            R = new Spell.Targeted(SpellSlot.R, 3000);
            RP = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, Brain.R.CastDelay, Brain.R.Speed, Brain.R.Width)
            {
                AllowedCollisionCount = 0
            };
        }
    }
}