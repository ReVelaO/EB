using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon
{
    public class Library
    {
        static AIHeroClient me => ObjectManager.Player;
        public static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            float Damage = 0f;
            switch (slot)
            {
                case SpellSlot.E:
                    if (Spells.E.IsReady())
                    {
                        Damage += new float[] { 70, 110, 150, 190, 230 }[Player.GetSpell(slot).Level - 1] + (0.6f * me.FlatMagicDamageMod);
                    }
                    break;
                case SpellSlot.R:
                    if (Spells.R.IsReady())
                    {
                        Damage += new float[] { 75, 175, 275 }[Player.GetSpell(slot).Level - 1] + (1f * me.FlatPhysicalDamageMod);
                    }
                    break;
            }
            return me.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
    }
}
