using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Katarina_.Addon.Orb
{
    class Portonazo
    {
        static AIHeroClient Kata => ObjectManager.Player;
        public static void Load()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => !x.IsDead 
            && x.IsValidTarget(Spells.E.Range)).OrderByDescending(x => x.TotalShieldHealth()))
            {
                if (enemy.IsValidTarget(Spells.Q.Range) 
                    && Spells.Q.IsReady() 
                    && enemy.TotalShieldHealth() < Library.DamageBySlot(enemy, SpellSlot.Q))
                {
                    Spells.Q.Cast(enemy);
                }
                if (enemy.IsInRange(Kata, Spells.W.Range) 
                    && Spells.W.IsReady() 
                    && enemy.TotalShieldHealth() < Library.DamageBySlot(enemy, SpellSlot.W))
                {
                    Spells.W.Cast();
                }
                if (enemy.IsValidTarget(Spells.E.Range) 
                    && Spells.E.IsReady() 
                    && enemy.TotalShieldHealth() < Library.DamageBySlot(enemy, SpellSlot.E))
                {
                    Spells.E.Cast(enemy);
                }
            }
        }
    }
}
