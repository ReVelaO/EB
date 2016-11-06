using EloBuddy;
using EloBuddy.SDK;

namespace Pitufo.Addon
{
    internal class PiDamages
    {
        public static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            var damage = 0f;
            if (slot == SpellSlot.Q)
            {
                if (PiSkills.Q.IsReady())
                    damage += new float[] { 60, 85, 110, 135, 160, 185 }[Player.GetSpell(slot).Level - 1] +
                              0.45f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.W)
            {
                if (PiSkills.W.IsReady())
                    damage += new float[] { 80, 100, 120, 140, 160 }[Player.GetSpell(slot).Level - 1] +
                              0.2f * Player.Instance.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.E)
            {
                if (PiSkills.E.IsReady())
                    damage += new float[] { 50, 75, 100, 125, 150 }[Player.GetSpell(slot).Level - 1] +
                              0.3f * Player.Instance.FlatMagicDamageMod;
            }
            return Player.Instance.CalculateDamageOnUnit(enemy, DamageType.Magical, damage);
        }
    }
}
