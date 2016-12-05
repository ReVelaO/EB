namespace Galio
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using System.Linq;
    using static Menus;
    using static Spells;
    internal class Laneclear
    {
        public static void LaneExecute()
        {
            var m1 = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x=>x.IsValidTarget(Q.Range)).FirstOrDefault();
            if (m1 != null)
            {
                if (LaneClearMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()) //range missing / Fix
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["qmana"].Cast<Slider>().CurrentValue)
                    {
                        Q.Cast(m1);
                    }
                }
            }
            var m2 = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.IsValidTarget(E.Range)).FirstOrDefault();
            if (m2 != null)
            {
                if (LaneClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady()) //range missing / Fix
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
