using System;
using System.Diagnostics;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using static Xerath.Program;
using static Xerath.Menus;
using static Xerath.Spells;

namespace Xerath
{
    internal class Combo
    {
        public static Spell.Chargeable Q;

        public static void ComboExecute()
        {
            //added targetselector for every spell, with range
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var Wtarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            var Etarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            var Rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            //use this q,w,e,rpreds like i did in Q, actually it's necessary but well.. just do it!
            var qpred = Q.GetPrediction(Qtarget);
            var wpred = W.GetPrediction(Wtarget);
            var epred = E.GetPrediction(Etarget);
            var rpred = R.GetPrediction(Rtarget);

            if (target.IsInvulnerable || (target == null)) return;

            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Q.Range) && Q.IsReady())
                {
                    if (!Q.IsCharging)
                        Q.StartCharging();
                    return;
                }
                if (Q.Range == Q.MaximumRange)
                {
                    Q.Cast(qpred.CastPosition);
                }
            }

            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
            {
                
            }

            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {

            }

            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {

            }
        }
    }
}