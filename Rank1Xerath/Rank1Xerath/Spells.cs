using System;
using EloBuddy;
using EloBuddy.SDK;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace Rank1Xerath
{
    public static class Spells
    {
        public static Spell.Chargeable Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }
        public static Spell.Targeted ignt { get; private set; }

        /// <summary>
        /// Declare Spells, remember: Spell.Chargeable Q; always!
        /// </summary>
        public static void InitializeSpells()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 750, 1500, 1500, 500, int.MaxValue, 95);
            Q.AllowedCollisionCount = int.MaxValue;

            W = new Spell.Skillshot(SpellSlot.W, 1100, SkillShotType.Circular, 250, int.MaxValue, 500);
            W.AllowedCollisionCount = int.MaxValue;

            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear, 250, 1400, 60);
            E.AllowedCollisionCount = 1;

            R = new Spell.Skillshot(SpellSlot.R, 3200, SkillShotType.Circular, 500, int.MaxValue, 120);
            R.AllowedCollisionCount = int.MaxValue;

            ignt = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerdot"), 600, DamageType.True);

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        public static bool IsCastingUlt
        {
            get { return Player.Instance.Buffs.Any(b => b.Caster.IsMe && b.IsValid() && b.DisplayName == "XerathR"); }
        }
        public static int LastChargeTime { get; private set; }
        public static Vector3 LastChargePosition { get; private set; }
        public static int MaxCharges
        {
            get { return !R.IsLearned ? 3 : 2 + R.Level; }
        }
        public static int ChargesRemaining { get; private set; }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.SData.Name)
                {
                    // Ult activation
                    case "XerathLocusOfPower2":
                        LastChargePosition = Vector3.Zero;
                        LastChargeTime = 0;
                        ChargesRemaining = MaxCharges;
                        break;
                    // Ult charge usage
                    case "xerathlocuspulse":
                        LastChargePosition = args.End;
                        LastChargeTime = Environment.TickCount;
                        ChargesRemaining--;
                        break;
                }
            }
        }

        /// <summary>
        /// Declare Items for Item usage
        /// </summary>
        public static Item Zhonyas = new Item(ItemId.Zhonyas_Hourglass,0);

        public static List<Item> ItemList = new List<Item>
        {
            Zhonyas
        };
    }
}