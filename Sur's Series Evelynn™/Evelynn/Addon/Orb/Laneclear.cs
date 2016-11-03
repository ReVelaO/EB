using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace Evelynn.Addon.Orb
{
    internal class Laneclear
    {
        private static AIHeroClient Evelynn => ObjectManager.Player;
        public static void Get()
        {
            if (EveMenu.Llane.CurrentValue)
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderBy(x => x.Health)
                        .FirstOrDefault(x => x.IsValidTarget(EveSpells.Q.Range)
                                             && x.IsEnemy && x.IsMinion);
                if (minions != null)
                {
                    if (EveMenu.Lq.CurrentValue)
                    {
                        if (!minions.IsInRange(Evelynn, Evelynn.GetAutoAttackRange()))
                        {
                            EveSpells.CastQ(minions);
                        }
                        else if (minions.Health < EveDamages.Q(minions))
                        {
                            EveSpells.CastQ(minions);
                        }
                    }
                    if (EveMenu.Le.CurrentValue)
                    {
                        if (Evelynn.TotalAttackDamage > 300)
                        {
                            EveSpells.CastE(minions);
                        }
                        else if (Evelynn.TotalAttackDamage < 225)
                        {
                            if (minions.Health < EveDamages.E(minions))
                            {
                                EveSpells.CastE(minions);
                            }
                        }
                        if (minions.BaseSkinName.ToLower().Contains("siege"))
                        {
                            EveSpells.CastE(minions);
                        }
                    } 
                }
            }
            else if (!EveMenu.Llane.CurrentValue)
            {
                foreach (var minions in EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x=>x.IsEnemy 
                && x.IsMinion && !x.IsDead && x.IsValidTarget(EveSpells.Q.Range)))
                {
                    if (minions != null)
                    {
                        if (EveMenu.Lq.CurrentValue) { EveSpells.CastQ(minions);}
                        if (EveMenu.Le.CurrentValue) { EveSpells.CastE(minions);}
                    }
                }
            }
        }
    }
}
