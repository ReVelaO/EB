namespace Orianna.Addon.Orb
{
    using System.Linq;

    using EloBuddy.SDK;

    using EloBuddy.SDK.Enumerations;

    using EloBuddy.SDK.Menu.Values;

    internal class Jungleclear
    {
        public static void Get()
        {
            var m = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(o => o.MaxHealth).FirstOrDefault(d => d.IsValidTarget(SpellManager.Q.Range));
            if (m != null)
            {
                if (MenuManager.mjungle["q"].Cast<CheckBox>().CurrentValue)
                {
                    if (SpellManager.Q.IsReady())
                    {
                        var p = SpellManager.Q.GetPrediction(m);
                        if (p.HitChance >= HitChance.Medium)
                        {
                            SpellManager.Q.Cast(p.CastPosition);
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
