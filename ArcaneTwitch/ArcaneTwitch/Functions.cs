using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ArcaneTwitch
{
    public static class Functions
    {
        public static void Elixir(AIHeroClient Target)
        {
            var ElixirPred = Program._Elixir.GetPrediction(Target);

            if (Program.mComboMenuCheckBox("WU"))
            {

                if (Target.IsValidTarget(Program._Elixir.Range) && ElixirPred.HitChance >= HitChance.High)
                {
                    if (Target.IsInRange(Program.myHero, Program._Elixir.Range)
                            && Program._Elixir.IsReady()
                            && Program._Elixir.CanCast(Target)
                            && !Program._Elixir.IsOnCooldown)
                    {
                        Program._Elixir.Cast(Target);
                    }
                }
                if (Target.IsRooted)
                {
                    Program._Elixir.Cast(Target.Position);
                }
            }
        }

        public static void Frustation(AIHeroClient Target)
        {
            if (Target.HasBuff("twitchdeadlyvenom") == true)
            {

                if (Program.mComboMenuCheckBox("Ee"))
                {
                    if (Program.mComboMenuComboBox("Lista") == 0)
                    {
                        if (Program.mComboMenuComboBox("ebb") == 0)
                        {
                            if (GetStacksCount(Target) >= Program.mComboMenuSlider("EVAR"))
                            {
                                if (Target.IsValidTarget(Program._Elixir.Range + 150) && Program._Frustation.IsReady())
                                {
                                    Program._Frustation.Cast();
                                }
                            }
                        }
                        if (Program.mComboMenuComboBox("ebb") == 1)
                        {
                            if (GetStacksDamage(Target) >= (Target.TotalShieldHealth() - 5))
                            {
                                if (Target.IsValidTarget(Program._Elixir.Range + 150) && Program._Frustation.IsReady())
                                {
                                    Program._Frustation.Cast();
                                }
                            }
                        }
                    }
                    if (Program.mComboMenuComboBox("Lista")==1)
                    {
                        if (Target.HasBuff("twitchdeadlyvenom"))
                        {
                            if (Target.HealthPercent >= 50
                                && Program.myHero.HealthPercent <= 38
                                && Target.IsInAutoAttackRange(Program.myHero))
                            {
                                if (Target.IsValidTarget(Program._Elixir.Range + 150) && Program._Frustation.IsReady())
                                {
                                    Program._Frustation.Cast();
                                }
                            }
                            else
                            {
                                if (Target.HealthPercent < Program.myHero.HealthPercent)
                                {
                                    if (GetStacksDamage(Target) >= (Target.TotalShieldHealth() - 5))
                                    {
                                        if (Target.IsValidTarget(Program._Elixir.Range + 150) && Program._Frustation.IsReady())
                                        {
                                            Program._Frustation.Cast();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void Activator()
        {
            var Target = TargetSelector.GetTarget(Program.myHero.GetAutoAttackRange(), DamageType.Physical);
            if (Target == null)
            {
                return;
            }
            //
            //BOTRK
            //
            if (Program.mItems("useBOTRK") == 0 && Program._BOTRK.IsOwned())
            {
                if (Program.mItems("modeBOTRK") == 0)
                {
                    if (Target.IsValidTarget(Program.myHero.GetAutoAttackRange())
                        && Program._BOTRK.IsReady()
                        && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                    {
                        Program._BOTRK.Cast(Target);
                    }
                }
                if (Program.mItems("modeBOTRK") == 1)
                {
                    if (Program.myHero.HealthPercent <= Program.mItemsSlider("hpBOTRK")
                        && Target.IsValidTarget(Program.myHero.GetAutoAttackRange())
                        && Program._BOTRK.IsReady()
                        && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                    {
                        Program._BOTRK.Cast(Target);
                    }
                }
            }
            //
            //Bilgewater Cutlass
            //
            if (Program.mItems("useBWC") == 0 && Program._Sable.IsOwned())
            {
                if (Target.IsValidTarget(Program.myHero.GetAutoAttackRange())
                        && Program._Sable.IsReady()
                        && Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                {
                    Program._Sable.Cast(Target);
                }
            }
        }
        public static void BaseCloak()
        {
            if (Program.mBaseCloak == 1)
            {
                return;
            }
            if (Program.myHero.HasBuff("SRHomeguardSpeed")
                && Program.myHero.IsMoving
                && Program.mBaseCloak == 0
                && Program._Cloak.IsReady()
                && !Shop.IsOpen)
            {
                Program._Cloak.Cast();
            }
        }
        public static void StealthRecall()
        {
            if (Program.mCloak
                && Program._Cloak.IsReady())
            {
                Program._Cloak.Cast();
                Program.Recall.Cast();
            }
        }
        //Sida's Code below.
        public static int GetStacksCount(AIHeroClient process)
        {
            var twitchECount = 0;
            for (var i = 1; i < 7; i++)
            {
                if (ObjectManager.Get<Obj_GeneralParticleEmitter>()
                    .Any(e => e.Position.Distance(process.ServerPosition) <= 175 &&
                              e.Name == "twitch_poison_counter_0" + i + ".troy"))
                {
                    twitchECount = i;
                }
            }
            return twitchECount;
        }
        public static float GetStacksDamage(AIHeroClient process)
        {
            if (GetStacksCount(process) == 0) return 0;

            float stacksChamps = GetStacksCount(process);

            float EDamageChamp = new[] { 20, 35, 50, 65, 80 }[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level - 1];

            if (stacksChamps > 1)
            {
                EDamageChamp += (new[] { 15, 20, 25, 30, 35 }[ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level - 1] + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.PhysicalDamageRatio * ObjectManager.Player.FlatPhysicalDamageMod) + (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.SpellDamageRatio * ObjectManager.Player.FlatMagicDamageMod)) * (stacksChamps - 1);
            }

            return ObjectManager.Player.CalculateDamageOnUnit(process, DamageType.Physical, EDamageChamp);
        }
    }
}
