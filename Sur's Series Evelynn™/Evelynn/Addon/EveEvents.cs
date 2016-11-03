using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Evelynn.Addon.Orb;

namespace Evelynn.Addon
{
    internal class EveEvents
    {
        private string PotionJ => "itemCrystalFlask";
        private const float SmiteableCircle = 250;
        private static AIHeroClient Evelynn => ObjectManager.Player;

        public static void Load()
        {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                Combo.Get();
            else if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.JungleClear))
                Jungleclear.Get();
            foreach (var random in EntityManager.Heroes.Enemies.Where(w => w.IsInRange(Evelynn, 650)))
            {
                if (EveSpells.R.CanCast(random)
                    && (random.TotalShieldHealth() < EveDamages.R(random))) EveSpells.R.Cast(random.Position);
                if (EveSpells.Q.CanCast(random)
                    && (random.TotalShieldHealth() < EveDamages.Q(random))) EveSpells.Q.Cast();
                if (EveSpells.E.CanCast(random)
                    && (random.TotalShieldHealth() < EveDamages.E(random))) EveSpells.E.Cast(random);
            }
            if (EveMenu.autoR.CurrentValue)
            {
                var max =
                    EntityManager.Heroes.Enemies.Where(w => w.IsValidTarget(EveSpells.R.Range));
                var prediction = EveSpells.R.GetBestCircularCastPosition(max, 85, EveSpells.R.CastDelay);
                if (max != null)
                {
                    if (prediction.HitNumber >= 4
                        && EveSpells.R.IsReady() 
                        && Evelynn.CountAlliesInRange(1000) >= 3 
                        && Evelynn.CountEnemiesInRange(1000) >= 3)
                    {
                        EveSpells.R.Cast(prediction.CastPosition);
                    }
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (EveMenu.Dq.CurrentValue) { EveSpells.Q.DrawRange(System.Drawing.Color.MediumPurple);}
            if (EveMenu.De.CurrentValue && EveSpells.E.IsReady()) { EveSpells.E.DrawRange(System.Drawing.Color.LightCoral); }
            if (EveMenu.Dr.CurrentValue && EveSpells.R.IsReady()) { EveSpells.R.DrawRange(System.Drawing.Color.BlueViolet); }
            if (EveMenu.DSt.CurrentValue)
            {
                if (EveSpells.Smite.IsReady())
                    foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                            && !m.IsDead
                                                                                                            &&
                                                                                                            EveSpells
                                                                                                                .Monsters
                                                                                                                .Contains
                                                                                                                (
                                                                                                                    m
                                                                                                                        .BaseSkinName) &&
                                                                                                            m.IsInRange(
                                                                                                                Player
                                                                                                                    .Instance,
                                                                                                                1000)))
                    {
                        if (EveSpells.GetHealthPrediction(monster, EveSpells.Smite.CastDelay) >
                            EveDamages.Smite(monster))
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, System.Drawing.Color.Azure);
                        if (EveSpells.GetHealthPrediction(monster, EveSpells.Smite.CastDelay) <
                            EveDamages.Smite(monster))
                            Drawing.DrawCircle(monster.Position, SmiteableCircle, System.Drawing.Color.Lime);
                    }
            }
        }
    }
}