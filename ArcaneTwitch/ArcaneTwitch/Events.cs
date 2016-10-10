using EloBuddy;
using EloBuddy.SDK;

namespace ArcaneTwitch
{
    class Events
    {
        public static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot == SpellSlot.R && sender.Owner.IsMe)
            {
                if (Item.HasItem(ItemId.Youmuus_Ghostblade))
                {
                    Item.UseItem(ItemId.Youmuus_Ghostblade);
                }
            }
        }
    }
}
