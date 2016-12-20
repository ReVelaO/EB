using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace Quinnsharp.Addon.Orb
{
    public static class Jungleclear
    {
        private static AIHeroClient Quinn => Player.Instance;

        public static void Get()
        {
            if (Quinn.ManaPercent < 59 || Quinn.IsDead) return;

            if (SpellHandler.Q.IsReady())
            {
                var m = EntityManager.MinionsAndMonsters.GetJungleMonsters(Quinn.Position).Where(x => x.IsInRange(Quinn, SpellHandler.Q.Range)).FirstOrDefault();

                if (m != null)
                {
                    SpellHandler.Q.Cast(m.Position);
                }
            }

            if (SpellHandler.E.IsReady())
            {
                var m = EntityManager.MinionsAndMonsters.GetJungleMonsters(Quinn.Position).Where(x => x.IsInRange(Quinn, SpellHandler.E.Range)).FirstOrDefault();

                if (m != null)
                {
                    SpellHandler.E.Cast(m);
                }
            }
        }
    }
}