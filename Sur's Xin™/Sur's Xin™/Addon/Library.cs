using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon
{
    public class Library
    {
        public static readonly string[] MonstersNames =
        {
            "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air", "SRU_Dragon_Elder", "Sru_Crab",
            "SRU_Baron", "SRU_RiftHerald",
            "SRU_Red", "SRU_Blue", "SRU_Krug", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "TT_Spiderboss", "TTNGolem", "TTNWolf", "TTNWraith"
        };

        private static AIHeroClient Me => ObjectManager.Player;

        public static float GetHealthPrediction(Obj_AI_Base enemy, int delay)
        {
            return Prediction.Health.GetPrediction(enemy, delay);
        }

        public static float DamageBySlot(Obj_AI_Base enemy, SpellSlot slot)
        {
            var damage = 0f;
            if (slot == SpellSlot.E)
            {
                if (Spells.E.IsReady())
                    damage += new float[] {70, 110, 150, 190, 230}[Player.GetSpell(slot).Level - 1] +
                              0.6f*Me.FlatMagicDamageMod;
            }
            else if (slot == SpellSlot.R)
            {
                if (Spells.R.IsReady())
                    damage += new float[] {75, 175, 275}[Player.GetSpell(slot).Level - 1] +
                              1f*Me.FlatPhysicalDamageMod + enemy.Health*0.15f;
            }
            return Me.CalculateDamageOnUnit(enemy, DamageType.Magical, damage);
        }
    }
}