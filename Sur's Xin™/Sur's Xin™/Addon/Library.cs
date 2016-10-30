using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Xin_.Addon
{
    public class Library
    {
        static AIHeroClient me => ObjectManager.Player;
        public readonly static string[] MonstersNames =
        {
            "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air", "SRU_Dragon_Elder", "Sru_Crab", "SRU_Baron", "SRU_RiftHerald",
            "SRU_Red", "SRU_Blue",  "SRU_Krug", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "TT_Spiderboss", "TTNGolem", "TTNWolf", "TTNWraith",
        };
        public static float GetHealthPrediction(Obj_AI_Base enemy, int Delay) { return Prediction.Health.GetPrediction(enemy, Delay); }
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
                        Damage += new float[] { 75, 175, 275 }[Player.GetSpell(slot).Level - 1] + (1f * me.FlatPhysicalDamageMod) + (enemy.Health * 0.15f);
                    }
                    break;
            }
            return me.CalculateDamageOnUnit(enemy, DamageType.Magical, Damage);
        }
    }
}
