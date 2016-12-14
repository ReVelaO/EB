using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace Twitchsharp.Addon.Orb
{
    public static class Jungleclear
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static void Get()
        {
            if (MenuHandler.jungleclear["w"].Cast<CheckBox>().CurrentValue == true)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Twitch.ServerPosition).Where(x => x.IsInRange(Twitch, 550)).FirstOrDefault();
                if (monster != null && SpellHandler.W.IsReady())
                {
                    SpellHandler.W.Cast(monster.ServerPosition);
                }
            }

            if (MenuHandler.jungleclear["e"].Cast<CheckBox>().CurrentValue == true)
            {
                var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Twitch.ServerPosition).Where(x => x.IsInRange(Twitch, SpellHandler.E.Range) && x.IsVenom()).FirstOrDefault();
                if (monster != null && SpellHandler.E.IsReady())
                {
                    if (MenuHandler.jungleclear["erb"].Cast<CheckBox>().CurrentValue == true)
                    {
                        if (Brain.buffs.Contains(monster.BaseSkinName))
                        {
                            var hp = Prediction.Health.GetPrediction(monster, 275);
                            if (DamageHandler.SD(monster) >= hp)
                            {
                                SpellHandler.E.Cast();
                            }
                        }
                    }
                    if (MenuHandler.jungleclear["ebd"].Cast<CheckBox>().CurrentValue == true)
                    {
                        if (Brain.exclusive.Contains(monster.BaseSkinName))
                        {
                            var hp = Prediction.Health.GetPrediction(monster, 275);
                            if (DamageHandler.SD(monster) >= hp)
                            {
                                SpellHandler.E.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}