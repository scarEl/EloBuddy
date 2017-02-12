using System;
using System.Runtime.Remoting.Messaging;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Spells;
using static Rank1Xerath.Spells;
using static Rank1Xerath.Program;
using static Rank1Xerath.Menus;
using Color = System.Drawing;

namespace Rank1Xerath
{
    public static class Combo
    {
        public static void ComboExecute()
        {
            var targetQ = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var targetW = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            var targetR = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            var targetall = TargetSelector.GetTarget(Q.MaximumRange, DamageType.Magical);
            var ignitetarget = TargetSelector.GetTarget(ignt.Range, DamageType.True);

            if (targetall != null)
            {
                if (ComboMenu["CQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (Q.IsReady())
                    {
                        if (!Q.IsCharging)
                        {
                            Q.StartCharging();
                            return;
                        } // BUG
                        if (Q.Range == Q.MaximumRange)
                        {
                            var prediction = Q.GetPrediction(targetQ);
                            if (prediction.HitChancePercent >= 80)
                            {
                                Q.Cast(prediction.CastPosition);
                            }
                        }
                    }
                }

                if (ComboMenu["CW"].Cast<CheckBox>().CurrentValue)
                {
                    var wpred = W.GetPrediction(targetW);
                    if (wpred.HitChancePercent >= 70 && W.IsReady() && targetW.IsValidTarget(W.Range))
                    {
                        W.Cast(wpred.CastPosition);
                    }
                }

                if (ComboMenu["CE"].Cast<CheckBox>().CurrentValue)
                {
                    var epred = E.GetPrediction(targetE);
                    if (epred.HitChancePercent >= 80 && E.IsReady() && targetE.IsValidTarget(E.Range))
                    {
                        E.Cast(epred.CastPosition);
                    }
                }

                #region Combo Ult Values

                var target = R.GetTarget();
                var rpred = R.GetPrediction(targetR);
                if (ComboMenu["CR"].Cast<CheckBox>().CurrentValue)
                {
                    if (target != null && R.GetRealDamage(target)*Spells.MaxCharges > target.Health && R.IsReady() &&
                        !target.IsInRange(Player.Instance, Q.MaximumRange))
                    {
                        if (rpred.HitChancePercent >= ComboMenu["CRSliderHitchance"].Cast<Slider>().CurrentValue)
                        {
                            R.Cast(rpred.CastPosition);
                        }
                    }
                }

                if (IsCastingUlt)
                {
                    R.Cast(rpred.CastPosition);
                }


                /*if (ActivatorMenu["Ignite"].Cast<CheckBox>().CurrentValue)
                {
                    if (Ignite.IsReady() && ignitetarget.Health < Ignite.GetRealDamage(ignitetarget))
                    {
                        Ignite.Cast(ignitetarget);
                    }
                }*/

                #endregion
            }
        }
    }
}