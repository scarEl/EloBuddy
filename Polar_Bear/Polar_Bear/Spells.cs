using EloBuddy;
using EloBuddy.SDK;
using System.Collections.Generic;

namespace Polar_Bear
{
    internal static class Spells
    {
        public static Spell.Active Q;
        public static Spell.Targeted W;
        public static Spell.Active E;
        public static Spell.Active R;

        public static void InitializeSpells()
        {
            Q = new Spell.Active(SpellSlot.Q, 750);
            W = new Spell.Targeted(SpellSlot.W, 395);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Active(SpellSlot.R, 125);
        }

        public static Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only,250);
        public static Item TitanicHydra = new Item(ItemId.Titanic_Hydra,250);
        public static Item Hydra2 = new Item(ItemId.Ravenous_Hydra,250);
        public static Item Tiamat = new Item(ItemId.Tiamat,250);
        public static Item TiamatMelee = new Item (ItemId.Tiamat_Melee_Only,250);

        public static List<Item> ItemList = new List<Item>
        {
            Hydra,
            TitanicHydra,
            Hydra2,
            Tiamat,
            TiamatMelee
        };
    }
}
