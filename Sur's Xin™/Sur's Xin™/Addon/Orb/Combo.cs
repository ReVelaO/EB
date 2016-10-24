using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon.Orb
{
    public class Combo
    {
        static AIHeroClient Xin => ObjectManager.Player;
        public static void Get()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Mixed);
            if (target == null) { return; }
            if (target.IsValidTarget(Spells.E.Range))
            {
                if (Settings.usarE.CurrentValue)
                {
                    if (Settings.usarEE.CurrentValue == true)
                    {
                        if (target.IsInRange(Xin, Xin.GetAutoAttackRange()))
                        {
                            //Do Nothing
                        }
                        else if (!target.IsInRange(Xin, Xin.GetAutoAttackRange())
                            && target.IsInRange(Xin, Spells.E.Range)) { Spells.E.Cast(target); }
                    }
                    if (Settings.usarEE.CurrentValue == false 
                        && Spells.E.IsReady() && target.IsInRange(Xin, Spells.E.Range))
                    {
                        Spells.E.Cast(target);
                    }
                }
                if (Settings.usarW.CurrentValue)
                {
                    if (Spells.W.IsReady() && target.IsInRange(Xin, Xin.GetAutoAttackRange()))
                    {
                        Spells.W.Cast();
                    }
                }
                if (Settings.usarR.CurrentValue)
                {
                    if (target.TotalShieldHealth() < Library.DamageBySlot(target, SpellSlot.R)
                        && target.IsInRange(Xin, Spells.R.Range))
                    {
                        Spells.R.Cast();
                    }
                }
                if (Settings.usarBOTRK.CurrentValue)
                {
                    BOTRK(target);
                }
                if (Settings.usarSABLE.CurrentValue)
                {
                    SABLE(target);
                }
                if (Settings.usarHYDRA.CurrentValue)
                {
                    HYDRA(target);
                }
                if (Settings.usarTIAMAT.CurrentValue)
                {
                    TIAMAT(target);
                }
            }
        }
        static Item botrk = new Item(ItemId.Blade_of_the_Ruined_King, 550);
        static void BOTRK(AIHeroClient enemy)
        {
            if (botrk.IsOwned())
            {
                if (enemy.IsValidTarget(botrk.Range)
                    && !enemy.IsInRange(Xin, Xin.GetAutoAttackRange())
                    && botrk.IsReady()) { botrk.Cast(enemy); }
            }
        }
        static Item sable = new Item(ItemId.Bilgewater_Cutlass, 550);
        static void SABLE(AIHeroClient enemy)
        {
            if (sable.IsOwned())
            {
                if (enemy.IsValidTarget(sable.Range)
                    && !enemy.IsInRange(Xin, Xin.GetAutoAttackRange())
                    && sable.IsReady()) { sable.Cast(enemy); }
            }
        }
        static Item hydra = new Item(ItemId.Ravenous_Hydra, 400);
        static void HYDRA(AIHeroClient enemy)
        {
            if (hydra.IsOwned())
            {
                if (enemy.IsValidTarget(hydra.Range) 
                    && hydra.IsReady()) { hydra.Cast(); }
            }
        }
        static Item tiamat = new Item(ItemId.Tiamat, 400);
        static void TIAMAT(AIHeroClient enemy)
        {
            if (tiamat.IsOwned())
            {
                if (enemy.IsValidTarget(tiamat.Range)
                    && tiamat.IsReady()) { tiamat.Cast(); }
            }
        }
    }
}
