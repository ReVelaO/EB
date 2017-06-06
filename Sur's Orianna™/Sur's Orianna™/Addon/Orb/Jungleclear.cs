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
            var m = EntityManager.MinionsAndMonsters
                .GetJungleMonsters(Orianna.ServerPosition)
                    .OrderByDescending(o => o.MaxHealth)
                        .FirstOrDefault(x => x.IsInRange(Orianna, SpellManager.Q.Range));

            if (m != null && m.IsValidTarget())
            {
                if (MenuManager.mjungle["q"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.Q.IsReady())
                    {
                        SpellManager.Q.Cast(SpellManager.Q.GetPrediction(m).CastPosition);
                    }
                }
                if (MenuManager.mjungle["w"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.W.IsReady() && BallManager.IsInFloor)
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