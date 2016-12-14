using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using System.Linq;

namespace Twitchsharp.Addon.Orb
{
    public static class Combo
    {
        private static AIHeroClient Twitch => Player.Instance;

        public static void Get()
        {
            if (MenuHandler.combo["q"].Cast<CheckBox>().CurrentValue == true)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsDead).FirstOrDefault(d => d.IsInRange(Twitch, 1200));
                if (enemies != null)
                {
                    if (enemies.IsInMinimumRange(Brain.BaseAA, Brain.ExtendedAA) && SpellHandler.Q.IsReady())
                    {
                        SpellHandler.Q.Cast();
                    }
                }
            }

            if (MenuHandler.combo["w"].Cast<CheckBox>().CurrentValue == true)
            {
                var t = TargetSelector.GetTarget(SpellHandler.W.Range, DamageType.True);
                if (t != null && !t.IsInvulnerable)
                {
                    var p = SpellHandler.W.GetPrediction(t);
                    if (p != null && p.HitChance > HitChance.Medium)
                    {
                        if (t.IsInMinimumRange(Brain.BaseAA, SpellHandler.W.Range) && SpellHandler.W.IsReady())
                        {
                            SpellHandler.W.Cast(t);
                        }
                    }
                }
            }

            if (MenuHandler.combo["e"].Cast<CheckBox>().CurrentValue == true)
            {
                var enemies = EntityManager.Heroes.Enemies.Where(x => x.IsValid && !x.IsInvulnerable && !x.IsDead && x.IsVenom()).FirstOrDefault(d => d.IsInRange(Twitch, SpellHandler.E.Range));
                if (enemies != null)
                {
                    var hp = Prediction.Health.GetPrediction(enemies, 275);
                    if (DamageHandler.SD(enemies) >= hp && SpellHandler.E.IsReady())
                    {
                        SpellHandler.E.Cast();
                    }
                }
            }

            if (MenuHandler.combo["r"].Cast<CheckBox>().CurrentValue == true)
            {
                var t = TargetSelector.GetTarget(Brain.ExtendedAA, DamageType.Physical);
                if (t != null)
                {
                    //Alone Logic
                    if (t.CountAlliesInRange(Brain.BaseAA) == 0)
                    {
                        if (t.IsInMinimumRange(Brain.BaseAA, Brain.ExtendedAA))
                        {
                            if (MenuHandler.combo["nm"].Cast<CheckBox>().CurrentValue == false)
                            {
                                var hp = Prediction.Health.GetPrediction(t, (int)Twitch.AttackCastDelay);
                                var dmglogic = Twitch.GetAutoAttackDamage(t, true) * MenuHandler.combo["multipler"].Cast<Slider>().CurrentValue;

                                if (Twitch.GetAutoAttackDamage(t, true) * 2 > hp) return;

                                if (dmglogic >= hp && SpellHandler.R.IsReady())
                                {
                                    SpellHandler.R.Cast();
                                }
                            }
                            else
                            {
                                if (SpellHandler.R.IsReady()) SpellHandler.R.Cast();
                            }
                        }
                    }
                    //TF Logic
                    if (MenuHandler.combo["tf"].Cast<CheckBox>().CurrentValue)
                    {
                        if (Twitch.CountAlliesInRange(1000) > t.CountEnemiesInRange(1500)
                            && t.CountAlliesInRange(500) > 0)
                        {
                            if (t.IsInMinimumRange(Brain.BaseAA, Brain.ExtendedAA))
                            {
                                if (SpellHandler.R.IsReady()) SpellHandler.R.Cast();
                            }
                        }
                    }
                    else
                    {
                        if (t.CountAlliesInRange(500) > 0
                            && Twitch.CountAlliesInRange(1000) > 1
                            && Twitch.CountEnemiesInRange(1500) > 1)
                        {
                            if (t.IsInMinimumRange(Brain.BaseAA, Brain.ExtendedAA))
                            {
                                if (SpellHandler.R.IsReady()) SpellHandler.R.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}