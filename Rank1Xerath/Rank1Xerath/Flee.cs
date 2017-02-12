using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Rank1Xerath.Menus;
using static Rank1Xerath.Spells;
using static Rank1Xerath.Program;

namespace Rank1Xerath
{
    public static class Flee
    {
        public static void FleeExecute()
        {
            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Magical, Player.Instance.Position);
            var epred = E.GetPrediction(targetE);

            if (JungleClearMenu["JQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                if (targetE.IsValid)
                {
                    E.Cast(epred.UnitPosition);
                }
            }
        }
    }
}