using System.Linq;
using EloBuddy.SDK;

namespace Evelynn.Addon.Orb
{
    internal class Jungleclear
    {
        public static void Get()
        {
            foreach (var monster in EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(m => m.IsMonster
                                                                                                    && !m.IsDead))
            {
                if (EveMenu.Jq.CurrentValue)
                    EveSpells.CastQ(monster);
                if (EveMenu.Je.CurrentValue)
                    if (EveSpells.Monsters.Contains(monster.BaseSkinName)) EveSpells.CastE(monster);
                    else if (monster.Health < EveDamages.E(monster)) EveSpells.CastE(monster);
                if (EveMenu.JSm.CurrentValue)
                    EveSpells.CastSmite(monster);
            }
        }
    }
}