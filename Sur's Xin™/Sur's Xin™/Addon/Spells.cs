using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Spells;
namespace Sur_s_Xin_.Addon
{
    public class Spells
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Spell.Targeted Smite;
        public static void Get()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Active(SpellSlot.R, 500);
            Smite = new Spell.Targeted(SummonerSpells.Smite.Slot, 570);
        }
        public static bool IsSmiteReady { get { return Player.CanUseSpell(Smite.Slot) == SpellState.Ready; } }
    }
}
