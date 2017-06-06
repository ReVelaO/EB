namespace Orianna.Addon.Orb
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using System.Linq;

    public static class Laneclear
    {
        private static AIHeroClient Orianna => Player.Instance;

        public static void Get()
        {
            if (Orianna.ManaPercent < MenuManager.mlane["mmsli"].Cast<Slider>().CurrentValue) return;

            var m = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Orianna.ServerPosition).OrderBy(o => o.Health).Where(c => c.IsInRange(Orianna, SpellManager.Q.Range));

            if (m != null)
            {
                if (MenuManager.mlane["q"].Cast<CheckBox>().CurrentValue)
                {
                    var minhit = MenuManager.mlane["minq"].Cast<Slider>().CurrentValue;

                    if (SpellManager.Q.IsReady())
                    {
                        var p = SpellManager.Q.GetBestCircularCastPosition(m);

                        if (p.HitNumber >= minhit)
                        {
                            SpellManager.Q.Cast(p.CastPosition);
                        }
                    }
                }
                if (MenuManager.mlane["w"].Cast<CheckBox>().CurrentValue)
                {
                    var minhit = MenuManager.mlane["minw"].Cast<Slider>().CurrentValue;

                    if (SpellManager.W.IsReady() && BallManager.IsInFloor)
                    {
                        if (BallManager.WBall.CountEnemyMinionsNear >= minhit)
                        {
                            SpellManager.W.Cast();
                        }
                    }
                }
            }
        }
    }
}