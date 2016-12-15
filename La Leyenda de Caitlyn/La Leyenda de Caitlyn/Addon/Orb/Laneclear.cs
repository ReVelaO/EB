using EloBuddy;
using EloBuddy.SDK;
using System.Linq;

namespace La_Leyenda_de_Caitlyn.Addon.Orb
{
    public static class Laneclear
    {
        private static AIHeroClient Caitlyn => Player.Instance;

        public static void Get()
        {
            if (Caitlyn.IsDead || Caitlyn.HasBuff("recall")) return;

            if (SpellHandler.Q.IsReady() && Caitlyn.ManaPercent > 59)
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Caitlyn.Position).Where(x => x.IsInRange(Caitlyn, SpellHandler.Q.Range));
                if (minions != null)
                {
                    var p = SpellHandler.Q.GetBestLinearCastPosition(minions);
                    if (p.HitNumber > 2)
                    {
                        SpellHandler.Q.Cast(p.CastPosition);
                    }
                }
            }
        }
    }
}