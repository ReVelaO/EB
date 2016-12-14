using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Linq;

namespace Twitchsharp.Addon.Orb
{
    public static class Update
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static void Get(EventArgs args)
        {
            if (MenuHandler.killsteal["aa"].Cast<CheckBox>().CurrentValue)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead && !x.IsInvulnerable).FirstOrDefault(d => d.IsInRange(Twitch, Brain.GetRealAA()));
                if (enemies != null && Orbwalker.CanAutoAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, enemies);
                }
            }

            if (MenuHandler.killsteal["e"].Cast<CheckBox>().CurrentValue)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead && !x.IsInvulnerable && x.IsVenom() && Prediction.Health.GetPrediction(x, 275) < DamageHandler.SD(x)).FirstOrDefault(d => d.IsInRange(Twitch, SpellHandler.E.Range));
                if (enemies != null && SpellHandler.E.IsReady())
                {
                    SpellHandler.E.Cast();
                }
            }
        }
    }
}