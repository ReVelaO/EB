using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Spells;

namespace Evelynn.Addon
{
    internal class EveSpells
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Smite;

        public static readonly string[] Monsters =
        {
            "SRU_Dragon_Water", "SRU_Dragon_Fire", "SRU_Dragon_Earth", "SRU_Dragon_Air", "SRU_Dragon_Elder", "Sru_Crab",
            "SRU_Baron", "SRU_RiftHerald",
            "SRU_Red", "SRU_Blue", "SRU_Krug", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak",
            "TT_Spiderboss", "TTNGolem", "TTNWolf", "TTNWraith"
        };

        public static void Load()
        {
            Q = new Spell.Active(SpellSlot.Q, 500);
            W = new Spell.Active(SpellSlot.W, 10000);
            E = new Spell.Targeted(SpellSlot.E, 285);
            R = new Spell.Skillshot(SpellSlot.R, 650, SkillShotType.Circular, 250, int.MaxValue, 350);
            Smite = new Spell.Targeted(SummonerSpells.Smite.Slot, 570);
        }

        public static void CastQ(Obj_AI_Base random)
        {
            if (random.IsInRange(Player.Instance, Q.Range)
                && Q.CanCast(random)
                && Q.IsReady()) Q.Cast();
        }

        public static void CastW(Obj_AI_Base random)
        {
            if (!random.IsInRange(Player.Instance, Q.Range)
                && W.IsReady() && W.CanCast(random)) W.Cast();
            else if (random.IsInRange(Player.Instance, 650)
                     && E.CanCast(random)
                     && W.IsReady()
                     && W.CanCast(random)) W.Cast();
            else if (!random.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()) 
                && random.HealthPercent < 20 && Player.Instance.HealthPercent > 80) W.Cast();
        }

        public static void CastE(Obj_AI_Base random)
        {
            if (random.IsInRange(Player.Instance, E.Range)
                && E.CanCast(random)
                && E.IsReady()) E.Cast(random);
        }

        public static void CastSmite(Obj_AI_Base random)
        {
            if (random.IsInRange(Player.Instance, Smite.Range)
                && Smite.CanCast(random)
                && Smite.IsReady()
                && Monsters.Contains(random.BaseSkinName)
                && (GetHealthPrediction(random, Smite.CastDelay) < EveDamages.Smite(random))) Smite.Cast(random);
        }

        public static float GetHealthPrediction(Obj_AI_Base enemy, int delay)
        {
            return Prediction.Health.GetPrediction(enemy, delay);
        }
    }
}