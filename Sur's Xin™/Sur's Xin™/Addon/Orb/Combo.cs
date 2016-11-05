using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon.Orb
{
    public class Combo
    {
        private static readonly Item botrk = new Item(ItemId.Blade_of_the_Ruined_King, 550);

        private static readonly Item sable = new Item(ItemId.Bilgewater_Cutlass, 550);

        private static readonly Item hydra = new Item(ItemId.Ravenous_Hydra, 400);

        private static readonly Item tiamat = new Item(ItemId.Tiamat, 400);
        private static AIHeroClient Xin => ObjectManager.Player;

        public static void Get()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Mixed);
            if (target == null) return;
            if (!target.IsValidTarget(Spells.E.Range)) return;
            if (Settings.UsarE.CurrentValue)
            {
                if (Settings.UsarEe.CurrentValue)
                    if (target.IsInRange(Xin, Xin.GetAutoAttackRange()))
                    {
                        //Do Nothing
                    }
                    else if (!target.IsInRange(Xin, Xin.GetAutoAttackRange())
                             && target.IsInRange(Xin, Spells.E.Range))
                    {
                        Spells.E.Cast(target);
                    }
                if ((Settings.UsarEe.CurrentValue == false)
                    && Spells.E.IsReady() && target.IsInRange(Xin, Spells.E.Range))
                    Spells.E.Cast(target);
            }
            if (Settings.UsarW.CurrentValue)
                if (Spells.W.IsReady() && target.IsInRange(Xin, Xin.GetAutoAttackRange()))
                    Spells.W.Cast();
            if (Settings.UsarR.CurrentValue)
                if ((target.TotalShieldHealth() < Library.DamageBySlot(target, SpellSlot.R))
                    && target.IsInRange(Xin, Spells.R.Range))
                    Spells.R.Cast();
            if (Settings.UsarBotrk.CurrentValue)
                Botrk(target);
            if (Settings.UsarSable.CurrentValue)
                Sable(target);
            if (Settings.UsarHydra.CurrentValue)
                Hydra(target);
            if (Settings.UsarTiamat.CurrentValue)
                Tiamat(target);
        }

        private static void Botrk(Obj_AI_Base enemy)
        {
            if (botrk.IsOwned())
                if (enemy.IsValidTarget(botrk.Range)
                    && !enemy.IsInRange(Xin, Xin.GetAutoAttackRange())
                    && botrk.IsReady()) botrk.Cast(enemy);
        }

        private static void Sable(Obj_AI_Base enemy)
        {
            if (sable.IsOwned())
                if (enemy.IsValidTarget(sable.Range)
                    && !enemy.IsInRange(Xin, Xin.GetAutoAttackRange())
                    && sable.IsReady()) sable.Cast(enemy);
        }

        private static void Hydra(AttackableUnit enemy)
        {
            if (hydra.IsOwned())
                if (enemy.IsValidTarget(hydra.Range)
                    && hydra.IsReady()) hydra.Cast();
        }

        private static void Tiamat(AIHeroClient enemy)
        {
            if (tiamat.IsOwned())
                if (enemy.IsValidTarget(tiamat.Range)
                    && tiamat.IsReady()) tiamat.Cast();
        }
    }
}