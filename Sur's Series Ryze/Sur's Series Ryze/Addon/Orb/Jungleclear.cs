using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Pitufo.Addon.Orb
{
    internal class Jungleclear
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        public static void Get()
        {
            foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                && m.IsValidTarget(1000)
                                                                                                && m.IsEnemy
                                                                                                && m.Distance(Ryze) < 1000))
            {
                if (PiMenu.LaneQ)
                {
                    if (PiSkills.Q.IsReady())
                    {
                        PiSkills.Q.Cast(monster.Position);
                    }
                }
                if (PiMenu.LaneE)
                {
                    if (!PiSkills.Q.IsReady() && PiSkills.E.IsReady())
                    {
                        PiSkills.E.Cast(monster);
                    }
                }
                if (PiMenu.LaneW)
                {
                    if (!PiSkills.Q.IsReady() && !PiSkills.E.IsReady())
                    {
                        PiSkills.W.Cast(monster);
                    }
                }
            }
        }
    }
}
