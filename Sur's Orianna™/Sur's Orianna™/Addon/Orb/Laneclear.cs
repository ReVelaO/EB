namespace Orianna.Addon.Orb
{
    using System.Linq;

    using EloBuddy.SDK;

    using EloBuddy.SDK.Menu.Values;
    using EloBuddy;

    internal class Laneclear
    {
        public static void Get()
        {
            if (MenuManager.mlane["method"].Cast<ComboBox>().CurrentValue == 0)
            {
                var m = EntityManager.MinionsAndMonsters.GetLaneMinions().OrderBy(o => o.Health).Where(c => c.IsValidTarget(SpellManager.Q.Range));
                if (m != null)
                {
                    if (MenuManager.mlane["q"].Cast<CheckBox>().CurrentValue)
                    {
                        if (SpellManager.Q.IsReady())
                        {
                            var p = SpellManager.Q.GetBestCircularCastPosition(m);
                            if (p.HitNumber >= MenuManager.mlane["minq"].Cast<Slider>().CurrentValue)
                            {
                                SpellManager.Q.Cast(p.CastPosition);
                            }
                        }
                    }
                    if (MenuManager.mlane["w"].Cast<CheckBox>().CurrentValue)
                    {
                        if (SpellManager.W.IsReady())
                        {
                            if (BallManager.WBall.CountEnemyMinionsNear >= MenuManager.mlane["minw"].Cast<Slider>().CurrentValue)
                            {
                                SpellManager.W.Cast();
                            }
                        }
                    }
                }
            }
            else if (MenuManager.mlane["method"].Cast<ComboBox>().CurrentValue == 1)
            {
                var m = EntityManager.MinionsAndMonsters.GetLaneMinions().OrderByDescending(o => o.MaxHealth).Where(c => c.IsValidTarget(SpellManager.Q.Range));
                if (m != null)
                {
                    if (MenuManager.mlane["q"].Cast<CheckBox>().CurrentValue)
                    {
                        if (SpellManager.Q.IsReady())
                        {
                            var p = SpellManager.Q.GetBestCircularCastPosition(m);
                            if (p.HitNumber >= MenuManager.mlane["minq"].Cast<Slider>().CurrentValue)
                            {
                                SpellManager.Q.Cast(p.CastPosition);
                            }
                        }
                    }
                    if (MenuManager.mlane["w"].Cast<CheckBox>().CurrentValue)
                    {
                        if (SpellManager.W.IsReady())
                        {
                            if (BallManager.WBall.CountEnemyMinionsNear >= MenuManager.mlane["minw"].Cast<Slider>().CurrentValue)
                            {
                                SpellManager.W.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}
