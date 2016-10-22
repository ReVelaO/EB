using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Brand_.Addon
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
                    {
                        Damage += new float[] { 80, 110, 140, 170, 200 }[Player.GetSpell(slot).Level - 1] + (0.55f * me.FlatMagicDamageMod);
                    }
                    break;
                case SpellSlot.W:
                    if (Spells.W.IsReady())
                    {
                        Damage += new float[] { 75, 120, 165, 210, 255 }[Player.GetSpell(slot).Level - 1] + (0.6f * me.FlatMagicDamageMod);
                    }
                    break;
                case SpellSlot.E:
                    if (Spells.E.IsReady())
                    {
                        Damage += new float[] { 70, 90, 110, 130, 150 }[Player.GetSpell(slot).Level - 1] + (0.35f * me.FlatMagicDamageMod);
                    }
                    break;
                case SpellSlot.R:
                    if (Spells.R.IsReady())
                    {
                        Damage += new float[] { 100, 200, 300 }[Player.GetSpell(slot).Level - 1] + (0.25f * me.FlatMagicDamageMod);
                    }
                    break;
            }
            return me.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
    }
}
