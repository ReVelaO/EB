using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace Quinnsharp.Addon.Orb
{
    public static class Laneclear
    {
        private static AIHeroClient Quinn => Player.Instance;

        public static void Get()
        {
            if (Quinn.ManaPercent < 59 || Quinn.IsDead) return;

            if (SpellHandler.Q.IsReady())
            {
                var m = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Quinn.Position).Where(x => x.IsInRange(Quinn, SpellHandler.Q.Range));
                if (m != null)
                {
                    var p = SpellHandler.Q.GetBestLinearCastPosition(m);

                    if (p.HitNumber <= 2)
                    {
                        SpellHandler.Q.Cast(p.CastPosition);
                    }
                }
            }
        }
    }
}