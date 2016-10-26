using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Katarina_.Addon
{
    public class Library
    {
        static AIHeroClient Kata => ObjectManager.Player;
        public static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            float Damage = 0f;
            switch (slot)
            {
                case SpellSlot.Q:
                    if (Spells.Q.IsReady())
                    Damage += new float[] { 60, 85, 110, 135, 160 }[Player.GetSpell(slot).Level - 1] + (0.45f * Kata.FlatMagicDamageMod);
                    break;
                case SpellSlot.W:
                    if (Spells.W.IsReady())
                    Damage += new float[] { 40, 75, 110, 145, 180 }[Player.GetSpell(slot).Level - 1] + (0.6f * Kata.FlatPhysicalDamageMod) + (0.25f * Kata.FlatMagicDamageMod);
                    break;
                case SpellSlot.E:
                    if (Spells.E.IsReady())
                    Damage += new float[] { 40, 70, 100, 130, 160 }[Player.GetSpell(slot).Level - 1] + (0.25f * Kata.FlatMagicDamageMod);
                    break;
                case SpellSlot.R:
                    if (Spells.R.IsReady())
                    Damage += new float[] { 350, 550, 750 }[Player.GetSpell(slot).Level - 1] + (3.75f * Kata.FlatPhysicalDamageMod) + (2.5f * Kata.FlatMagicDamageMod);
                    break;
            }
            return Kata.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
    }
}
