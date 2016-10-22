using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;

namespace Sur_s_Brand_.Addon
{
    public class Events
    {
        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
        }
        public static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Orb.Combo.Get();
            }
            KillSteal();
        }
        public static void OnDraw(EventArgs args)
        {
            if (Settings.dibujarQ.CurrentValue && Spells.Q.IsReady()) Spells.Q.DrawRange(Color.Orange);
            if (Settings.dibujarW.CurrentValue && Spells.W.IsReady()) Spells.W.DrawRange(Color.Orange);
            if (Settings.dibujarE.CurrentValue && Spells.E.IsReady()) Spells.E.DrawRange(Color.Orange);
            if (Settings.dibujarR.CurrentValue && Spells.R.IsReady()) Spells.R.DrawRange(Color.Red);
        }
        private static AIHeroClient Brand => ObjectManager.Player;
        private static void KillSteal()
        {
            foreach (
    var enemy in
    EntityManager.Heroes.Enemies.Where(
        x =>
            !x.HasBuffOfType(BuffType.Invulnerability) 
            && x.IsValidTarget(Spells.Q.Range) 
            && x.IsEnemy 
            &&!x.IsDead))
            {
                var qPred = Prediction.Position.PredictLinearMissile(enemy, Spells.Q.Range, Spells.Q.Radius, Spells.Q.CastDelay, Spells.Q.Speed, int.MaxValue, Brand.Position);
                var wPred = Prediction.Position.PredictCircularMissile(enemy, Spells.W.Range, Spells.W.Radius, Spells.W.CastDelay, Spells.W.Speed, Brand.Position);

                if (enemy.IsInRange(Brand, Spells.Q.Range)
                        && (Library.DamageBySlot(enemy, SpellSlot.Q) > enemy.TotalShieldHealth())
                        && (qPred.HitChance >= HitChance.High)
                        && !enemy.IsDead) Spells.Q.Cast(enemy);
                    if (enemy.IsInRange(Brand, Spells.W.Range)
                        && (Library.DamageBySlot(enemy, SpellSlot.W) > enemy.TotalShieldHealth())
                        && (wPred.HitChance >= HitChance.High)
                        && !enemy.IsDead) Spells.W.Cast(wPred.CastPosition);
                    if (enemy.IsInRange(Brand, Spells.R.Range)
                        && (Library.DamageBySlot(enemy, SpellSlot.E) > enemy.TotalShieldHealth())
                        && !enemy.IsDead) Spells.E.Cast(enemy);
            }
        }
    }
}
