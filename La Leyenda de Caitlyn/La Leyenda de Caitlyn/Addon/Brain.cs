using EloBuddy;
using EloBuddy.SDK;

namespace La_Leyenda_de_Caitlyn.Addon
{
    public static class Brain
    {
        private static AIHeroClient Caitlyn => Player.Instance;

        public static float RealAARange = 715;

        public static int Time(this Spell.Skillshot s, Obj_AI_Base endpos)
        {
            return (int)((Caitlyn.Distance(endpos.ServerPosition) / s.Speed) * 1000) + s.CastDelay;
        }

        public static int RTime(Obj_AI_Base endpos)
        {
            return (int)((Caitlyn.Distance(endpos.ServerPosition) / R.Speed) * 1000) + R.CastDelay;
        }

        public static bool IsInMinimumRange(this Obj_AI_Base obj, float min, float max)
        {
            return !obj.IsInRange(Player.Instance, min) && obj.IsInRange(Player.Instance, max);
        }

        public static bool IsCCd(this Obj_AI_Base obj)
        {
            if (obj.HasBuffOfType(BuffType.Charm) || obj.HasBuffOfType(BuffType.Fear)
                || obj.HasBuffOfType(BuffType.Stun) || obj.HasBuffOfType(BuffType.Knockup)
                || obj.HasBuffOfType(BuffType.Knockback) || obj.HasBuffOfType(BuffType.Snare)
                || obj.HasBuffOfType(BuffType.Taunt) || obj.HasBuffOfType(BuffType.Suppression)
                || obj.IsRooted || obj.IsStunned)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static class W
        {
            public const int CastDelay = 1100;

            public const int Speed = int.MaxValue;

            public const int Width = 67;
        }

        public static class R
        {
            public static float GetRange()
            {
                var i = 0;

                switch (SpellHandler.R.Level)
                {
                    case 1:
                        i = 2000;
                        break;

                    case 2:
                        i = 2500;
                        break;

                    case 3:
                        i = 3000;
                        break;
                }

                return i;
            }

            public const int CastDelay = 1000;

            public const int Speed = 3200;

            public const int Width = 65;
        }
    }
}