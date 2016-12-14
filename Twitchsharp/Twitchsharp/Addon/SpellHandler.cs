using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Twitchsharp.Addon
{
    public static class SpellHandler
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static Spell.Active Q, E, R;

        public static Spell.Skillshot W;

        public static void Load()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, 1400, 280)
            {
                AllowedCollisionCount = -1,
                MinimumHitChance = HitChance.Medium
            };
            E = new Spell.Active(SpellSlot.E, 1200);
            R = new Spell.Active(SpellSlot.R);
        }
    }
}