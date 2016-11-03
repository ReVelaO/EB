using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerRyze.Addon.Orb
{
    public static class Jungleclear
    {
        private static AIHeroClient Ryze => ObjectManager.Player;

        private static bool IsFluxed(Obj_AI_Base random) => random.HasBuff("RyzeE");

        public static void Get()
        {
            foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                && m.IsValidTarget(1000) 
                                                                                                && m.IsEnemy))
            {
                if (Spells.Q.IsReady()) { Spells.CastQ(monster);}
                if (!Spells.Q.IsReady() && Spells.E.IsReady()) { Spells.CastE(monster);}
                if (!Spells.Q.IsReady() && !Spells.E.IsReady()) { Spells.CastW(monster);}
            }
        }
    }
}
