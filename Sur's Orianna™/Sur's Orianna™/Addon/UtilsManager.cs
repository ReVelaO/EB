namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Spells;
    using System.Linq;

    public static class UtilsManager
    {
        private static AIHeroClient Orianna => Player.Instance;

        public static Spell.Skillshot Flash = new Spell.Skillshot(SummonerSpells.Flash.Slot, 425, SkillShotType.Linear)
        {
            AllowedCollisionCount = -1
        };

        public static Spell.Targeted Ignite = new Spell.Targeted(SummonerSpells.Ignite.Slot, 600);
        public static bool HasFlash => SummonerSpells.PlayerHas(SummonerSpellsEnum.Flash);
        public static bool HasIgnite => SummonerSpells.PlayerHas(SummonerSpellsEnum.Ignite);
        public static bool WillShock;

        public static void Shockwave()
        {
            if (WillShock)
            {
                if (HasFlash && Flash.IsReady() && SpellManager.R.IsReady())
                {
                    var t = EntityManager.Heroes.Enemies.FirstOrDefault(d => d.IsInRange(Orianna, 800));
                    if (t.IsValidTarget())
                    {
                        if (Orianna.HasBall())
                        {
                            var e = Orianna.Position.Extend(t, Flash.Range);
                            SpellManager.R.Cast();
                            Flash.Cast(e.To3D());
                            WillShock = false;
                        }
                    }
                }
            }
        }

        public static void AutoIgnite()
        {
            if (HasIgnite && Ignite.IsReady())
            {
                var target = EntityManager.Heroes.Enemies.FirstOrDefault(d => d.IsValidTarget(Ignite.Range));
                if (target != null && !target.IsInvulnerable)
                {
                    if (Orianna.GetAutoAttackDamage(target, true) > DamageManager.HPrediction(target, (int)Orianna.AttackCastDelay)) return;

                    if (DamageManager.HPrediction(target, Ignite.CastDelay) < DamageLibrary.GetSummonerSpellDamage(Player.Instance, target, DamageLibrary.SummonerSpells.Ignite))
                    {
                        Ignite.Cast(target);
                    }
                }
            }
        }
    }
}