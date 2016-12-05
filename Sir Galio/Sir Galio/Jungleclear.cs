namespace Galio
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using System.Linq;
    using static Menus;
    using static Spells;
    internal class Jungleclear
    {
        public static void JungleExecute()
        {
            if (JungleClearMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()) //range missing
            {
                var m1 = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x=>x.IsValidTarget(Q.Range)).OrderByDescending(o=>o.MaxHealth).FirstOrDefault();
                if (m1 != null)
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["qmana"].Cast<Slider>().CurrentValue)
                    {
                        Q.Cast(m1);
                    }
                }
            }

            if (JungleClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady()) //range missing
            {
                var m2 = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x => x.IsValidTarget(E.Range)).OrderByDescending(o => o.MaxHealth).FirstOrDefault();
                if (m2 != null)
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["emana"].Cast<Slider>().CurrentValue)
                    {
                        E.Cast(m2);
                    }
                }
            }
        }
    }
}
