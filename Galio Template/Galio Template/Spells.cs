using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Galio_Template
{
    internal static class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Skillshot E;
        public static Spell.Active R;

        public static void InitializeSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Cone);
            W = new Spell.Targeted(SpellSlot.W, 800);
            E = new Spell.Skillshot(SpellSlot.E, 1180, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R, 560);
        }
    }
}
