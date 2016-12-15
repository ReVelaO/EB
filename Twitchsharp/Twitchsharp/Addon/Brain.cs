using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System.Linq;

namespace Twitchsharp.Addon
{
    public static class Brain
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static readonly string[] buffs = { "SRU_Red", "SRU_Blue" };

        public static readonly string[] exclusive = { "SRU_Baron", "SRU_RiftHerald", "SRU_Dragon_Fire", "SRU_Dragon_Water", "SRU_Dragon_Earth", "SRU_Dragon_Air", "SRU_Dragon_Elder" };

        public static bool IsVenom(this Obj_AI_Base obj)
        {
            return obj.HasBuff("TwitchDeadlyVenom");
        }

        public static bool IsInMinimumRange(this Obj_AI_Base obj, float min, float max)
        {
            return !obj.IsInRange(Twitch, min) && obj.IsInRange(Twitch, max);
        }

        public static int CountEnemiesInLine(this Obj_AI_Base obj, Vector3 endpos, float width, float range)
        {
            var line = new Geometry.Polygon.Rectangle(obj.Position, endpos, width);
            var enemies = EntityManager.Heroes.Enemies.Where(x => line.IsInside(x) && x.IsValidTarget() && x.IsInRange(obj, range)).Count();
            return enemies;
        }

        public static bool IsAAExtended(this Obj_AI_Base obj)
        {
            return obj.HasBuff("TwitchFullAutomatic");
        }

        public static int GetMyTeam(float range)
        {
            return EntityManager.Heroes.Allies.Where(x => !x.IsDead && x.IsInRange(Twitch, range)).Count();
        }

        public static int GetAllies(float range)
        {
            return EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsDead && x.IsValid && x.IsInRange(Twitch, range)).Count();
        }

        public static int GetEnemies(float range)
        {
            return EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead && x.IsInRange(Twitch, range)).Count();
        }

        public static float BaseAA => 615;

        public static float ExtendedAA => BaseAA + 300;

        public static float GetRealAA()
        {
            float aa = 615;
            if (Twitch.IsAAExtended())
            {
                aa = aa + 300;
            }
            return aa;
        }

        public static int CountVenom(Obj_AI_Base obj)
        {
            var i = 0;
            if (obj.IsInRange(Twitch, SpellHandler.E.Range))
            {
                var particle = ObjectManager.Get<Obj_GeneralParticleEmitter>().Where(x => x.Name.Contains("twitch_poison_counter")).FirstOrDefault();

                if (particle == null) i = 0;

                switch (particle.Name)
                {
                    case "twitch_poison_counter_01.troy":
                        i = 1;
                        break;

                    case "twitch_poison_counter_02.troy":
                        i = 2;
                        break;

                    case "twitch_poison_counter_03.troy":
                        i = 3;
                        break;

                    case "twitch_poison_counter_04.troy":
                        i = 4;
                        break;

                    case "twitch_poison_counter_05.troy":
                        i = 5;
                        break;

                    case "twitch_poison_counter_06.troy":
                        i = 6;
                        break;
                }
            }
            else
            {
                i = 0;
            }
            return i;
        }
    }
}