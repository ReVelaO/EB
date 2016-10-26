using EloBuddy;
using EloBuddy.SDK;

namespace Sur_s_Katarina_.Addon
{
    public class Spells
    {
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static void Get()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);
        }
    }
}
