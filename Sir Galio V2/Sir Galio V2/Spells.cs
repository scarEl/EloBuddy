using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using static Sir_Galio_V2.Program;

namespace Sir_Galio_V2
{
    internal class Spells
    {
        public static Spell.Skillshot Q,E,R;
        public static Spell.Chargeable W;

        public static void InitializeSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 250, 500, 400);
            W = new Spell.Chargeable(SpellSlot.W, 275,450,0);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, rrange,SkillShotType.Circular);
        }
    }
}