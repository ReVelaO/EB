namespace Orianna.Addon.Orb
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu.Values;
    using System.Linq;

    public static class Jungleclear
    {
        private static AIHeroClient Orianna => Player.Instance;

        public static void Get()
        {
            var m = EntityManager.MinionsAndMonsters.GetJungleMonsters(Orianna.ServerPosition).OrderByDescending(o => o.MaxHealth).Where(x => x.IsInRange(Orianna, SpellManager.Q.Range)).FirstOrDefault();
            if (m.IsValidTarget())
            {
                if (MenuManager.mjungle["q"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.Q.IsReady())
                    {
                        var p = SpellManager.Q.GetPrediction(m);
                        if (p.HitChance > HitChance.Low)
                        {
                            SpellManager.Q.Cast(m);
                        }
                    }
                }
                if (MenuManager.mjungle["w"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.W.IsReady())
                    {
                        if (BallManager.WBall.IsInBall(m))
                        {
                            SpellManager.W.Cast();
                        }
                    }
                }
            }
        }
    }
}