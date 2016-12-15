using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace Twitchsharp.Addon.Orb
{
    public static class Laneclear
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static void Get()
        {
            if (MenuHandler.laneclear["w"].Cast<CheckBox>().CurrentValue == true)
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Twitch.ServerPosition).Where(x => x.IsInRange(Twitch, SpellHandler.W.Range));
                if (minions != null)
                {
                    var p = SpellHandler.W.GetBestCircularCastPosition(minions);
                    if (p.HitNumber >= MenuHandler.laneclear["whit"].Cast<Slider>().CurrentValue)
                    {
                        SpellHandler.W.Cast(p.CastPosition);
                    }
                }
            }

            if (MenuHandler.laneclear["e"].Cast<CheckBox>().CurrentValue == true)
            {
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Twitch.ServerPosition).Where(x => x.IsInRange(Twitch, SpellHandler.E.Range) && x.IsVenom());
                if (minions != null)
                {
                    if (minions.Count(c => Prediction.Health.GetPrediction(c, 275) < DamageHandler.SD(c) && Brain.CountVenom(c) >= MenuHandler.laneclear["estacks"].Cast<Slider>().CurrentValue) >= MenuHandler.laneclear["ehit"].Cast<Slider>().CurrentValue)
                    {
                        if (SpellHandler.E.IsReady()) SpellHandler.E.Cast();
                    }
                    if (minions.Count(c => Brain.CountVenom(c) >= MenuHandler.laneclear["estacks"].Cast<Slider>().CurrentValue) >= MenuHandler.laneclear["ehit"].Cast<Slider>().CurrentValue)
                    {
                        if (SpellHandler.E.IsReady()) SpellHandler.E.Cast();
                    }
                }
            }
        }
    }
}