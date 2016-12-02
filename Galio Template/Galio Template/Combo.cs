using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Galio_Template.Menus;

namespace Galio_Template
{
    internal class Combo
    {
       public static void ComboExecute()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);

            if ((target == null) || target.IsInvulnerable) return;
            
            //First method, without Prediction
            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady())
                {
                    Spells.Q.Cast();
                }
            }

            //Second method, with Prediction
            //You cant use 2 different Spell Usages on 1 ComboBox! either it will fail a lot or wont even work!
            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady())
                {
                    var qhitchance = Spells.Q.GetPrediction(target);
                    if (qhitchance.HitChancePercent >= 80)
                    {
                        Spells.Q.Cast(qhitchance.CastPosition);
                    }
                }
            }

            //R usage, with enemies in range
            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Spells.R.Range) && Spells.R.IsReady()
                    && Player.Instance.CountEnemiesInRange(600) == ComboMenu["REnemies"].Cast<Slider>().CurrentValue)
                {
                    Spells.R.Cast();
                }
            }
        }
    }
}
