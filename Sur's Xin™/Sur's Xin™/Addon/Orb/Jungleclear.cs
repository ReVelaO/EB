using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon.Orb
{
    public static class Jungleclear
    {
        private static AIHeroClient Xin => ObjectManager.Player;

        public static void Get()
        {
            foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                    && !m.IsDead))
            {
                if (Settings.JusarE.CurrentValue)
                    if (Spells.E.IsReady()
                        && monster.IsValidTarget(Spells.E.Range)) Spells.E.Cast(monster);
                if (Settings.JusarW.CurrentValue)
                    if (Spells.W.IsReady()
                        && monster.IsInRange(Xin, Xin.GetAutoAttackRange())) Spells.W.Cast();
                if (Settings.UsarSmite.CurrentValue)
                    if (Spells.Smite.IsReady() && monster.IsValidTarget(Spells.Smite.Range))
                        if (Library.MonstersNames.Contains(monster.BaseSkinName)
                            &&
                            (Library.GetHealthPrediction(monster, Spells.Smite.CastDelay) <
                             Xin.GetSummonerSpellDamage(monster, DamageLibrary.SummonerSpells.Smite)))
                            Spells.Smite.Cast(monster);
            }
        }
    }
}