using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Pitufo.Addon.Orb
{
    internal class Jungleclear
    {
        private static AIHeroClient Ryze => ObjectManager.Player;
        private static readonly Random random = new Random();
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
                        Core.DelayAction(() => PiSkills.Q2.Cast(monster), random.Next(50, 250));
                    }
                }
                if (PiMenu.LaneE)
                {
                    if (!PiSkills.Q.IsReady() && PiSkills.E.IsReady())
                    {
                        Core.DelayAction(() => PiSkills.E.Cast(monster), random.Next(50, 250));
                    }
                }
                if (PiMenu.LaneQ)
                {
                    if (PiSkills.Q.IsReady())
                    {
                        Core.DelayAction(() => PiSkills.Q2.Cast(monster), random.Next(50, 250));
                    }
                }
                if (PiMenu.LaneW)
                {
                    if (!PiSkills.Q.IsReady() && !PiSkills.E.IsReady())
                    {
                        Core.DelayAction(() => PiSkills.W.Cast(monster), random.Next(50, 250));
                    }
                }
            }
        }
    }
}
