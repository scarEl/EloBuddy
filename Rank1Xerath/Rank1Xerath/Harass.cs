using EloBuddy;
using EloBuddy.SDK;
using System;
using EloBuddy.SDK.Menu.Values;
using static Rank1Xerath.Program;
using static Rank1Xerath.Spells;
using static Rank1Xerath.Menus;

namespace Rank1Xerath
{
    public static class Harass
    {
        public static void HarassExecute()
        {
            var targetQ = TargetSelector.GetTarget(Q.MinimumRange, DamageType.Magical);
            var targetW = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            var targetall = TargetSelector.GetTarget(Q.MaximumRange, DamageType.Magical);

            if ((targetall == null) || !targetall.IsInvulnerable) return;

            if (HarassMenu["HQ"].Cast<CheckBox>().CurrentValue)
            {
                if (targetQ.IsValidTarget(Q.Range + 100) && Q.IsReady())
                {
                    if (!Q.IsCharging)
                        Q.StartCharging();
                    return;
                }
                var qpred = Q.GetPrediction(targetQ);
                if (qpred.HitChancePercent >= 80 && Q.Range >= Q.MinimumRange)
                {
                    Q.Cast(qpred.UnitPosition);
                }
            }

            if (HarassMenu["HW"].Cast<CheckBox>().CurrentValue)
            {
                var wpred = W.GetPrediction(targetW);
                if (wpred.HitChancePercent >= 70 && W.IsReady())
                {
                    W.Cast(wpred.UnitPosition);
                }
            }

            if (HarassMenu["HE"].Cast<CheckBox>().CurrentValue)
            {
                var epred = E.GetPrediction(targetE);
                if (epred.HitChancePercent >= 80 && E.IsReady())
                {
                    E.Cast(epred.UnitPosition);
                }
            }
        }
    }
}