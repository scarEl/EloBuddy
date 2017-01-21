using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Xerath
{
    internal static class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;


        public static void InitializeSpells()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 750, 1500, 1500, 500, int.MaxValue, 95) { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Skillshot(SpellSlot.W, 1100, SkillShotType.Circular, 250, int.MaxValue, 500) { AllowedCollisionCount = int.MaxValue };
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear, 250, 1400, 60);
            R = new Spell.Skillshot(SpellSlot.R, 3200, SkillShotType.Circular, 500, int.MaxValue, 120) { AllowedCollisionCount = int.MaxValue };

        }
    }
}