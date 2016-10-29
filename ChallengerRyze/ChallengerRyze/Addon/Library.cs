using EloBuddy;
using EloBuddy.SDK;

namespace ChallengerRyze.Addon
{
    public class Library
    {
        static AIHeroClient me => ObjectManager.Player;
        public static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            float Damage = 0f;
            switch (slot)
            {
                case SpellSlot.Q:
                    if (Spells.Q.IsReady())
                        Damage += new float[] { 60, 85, 110, 135, 160, 185 }[Player.GetSpell(slot).Level - 1] + (0.45f * me.FlatMagicDamageMod);
                    break;
                case SpellSlot.W:
                    if (Spells.W.IsReady())
                        Damage += new float[] { 80, 100, 120, 140, 160 }[Player.GetSpell(slot).Level - 1] + (0.2f * me.FlatMagicDamageMod);
                    break;
                case SpellSlot.E:
                    if (Spells.E.IsReady())
                        Damage += new float[] { 50, 75, 100, 125, 150 }[Player.GetSpell(slot).Level - 1] + (0.3f * me.FlatMagicDamageMod);
                    break;
            }
            return me.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
    }
}
