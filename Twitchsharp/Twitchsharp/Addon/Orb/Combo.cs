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
                    if (t.CountAllyChampionsInRange(Brain.BaseAA) == 0)
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
                    //Situation Logic 0
                    if (Twitch.CountEnemiesInLine(t.Position, 65, Brain.ExtendedAA + 185) >= 2)
                    {
                        if (SpellHandler.R.IsReady()) SpellHandler.R.Cast();
                    }
                    //TF Logic
                    if (MenuHandler.combo["tf"].Cast<CheckBox>().CurrentValue)
                    {
                        if (Twitch.CountAllyChampionsInRange(1000) > t.CountEnemyChampionsInRange(1500)
                            && t.CountAllyChampionsInRange(500) > 0)
                        {
                            if (t.IsInMinimumRange(Brain.BaseAA, Brain.ExtendedAA))
                            {
                                if (SpellHandler.R.IsReady()) SpellHandler.R.Cast();
                            }
                        }
                    }
                    else
                    {
                        if (t.CountAllyChampionsInRange(500) > 0
                            && Twitch.CountAllyChampionsInRange(1000) > 1
                            && Twitch.CountEnemyChampionsInRange(1500) > 1)
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
        //QBack Safe invisible logic
        private static void auto_baseQ(Spellbook sender, SpellbookCastSpellEventArgs eventArgs)
            {
            if (eventArgs.Slot != SpellSlot.Recall || SpellHandler.Q.IsReady() || MenuHandler.misc["QRecall"].Cast<CheckBox>().CurrentValue || MenuHandler.misc["QRecall2"].Cast<KeyBind>().CurrentValue) return;
                        SpellHandler.Q.Cast();
                        Core.DelayAction(() => ObjectManager.Player.Spellbook.CastSpell(SpellSlot.Recall), SpellHandler.Q.CastDelay + 300);
                        eventArgs.Process = false;
        }
    }
}
