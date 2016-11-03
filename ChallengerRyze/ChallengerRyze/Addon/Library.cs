using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerRyze.Addon
{
    public class Library
    {
        private static AIHeroClient Me => ObjectManager.Player;

        public static float Ignite(Obj_AI_Base enemy)
        {
            return Player.Instance.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite);
        }

        public static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            var Damage = 0f;
            if (slot == SpellSlot.Q)
            {
                if (Spells.Q.IsReady())
                    Damage += new float[] {60, 85, 110, 135, 160, 185}[Player.GetSpell(slot).Level - 1] +
                              0.45f*Me.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.W)
            {
                if (Spells.W.IsReady())
                    Damage += new float[] {80, 100, 120, 140, 160}[Player.GetSpell(slot).Level - 1] +
                              0.2f*Me.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.E)
            {
                if (Spells.E.IsReady())
                    Damage += new float[] {50, 75, 100, 125, 150}[Player.GetSpell(slot).Level - 1] +
                              0.3f*Me.FlatMagicDamageMod;
            }
            return Me.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
    }
}