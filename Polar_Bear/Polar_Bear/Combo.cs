using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using static Polar_Bear.Menus;
using static Polar_Bear.Program;

namespace Polar_Bear
{
    internal class Combo
    {

        public static void ComboExecute()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);

            if ((target == null) || target.IsInvulnerable) return;

            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady())
                {
                    Spells.Q.Cast(target);
                }
            }

            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Spells.E.Range) && Spells.E.IsReady())
                {
                    Spells.E.Cast();
                }
            }

            if (ActivatorMenu["Hydra"].Cast<CheckBox>().CurrentValue)
            {
                if (Spells.Hydra.IsOwned() && Spells.Hydra.IsReady() && target.IsValidTarget(Spells.R.Range))
                {
                    Spells.Hydra.Cast();
                }
                if (Spells.Hydra2.IsOwned() && Spells.Hydra2.IsReady() && target.IsValidTarget(Spells.R.Range))
                {
                    Spells.Hydra2.Cast();
                }
                if (Spells.TitanicHydra.IsOwned() && Spells.TitanicHydra.IsReady() && target.IsValidTarget(Spells.R.Range))
                {
                    Spells.TitanicHydra.Cast();
                }
                if (Spells.Tiamat.IsOwned() && Spells.Tiamat.IsReady() && target.IsValidTarget(Spells.R.Range))
                {
                    Spells.Tiamat.Cast();
                }
                if (Spells.TiamatMelee.IsOwned() && Spells.TiamatMelee.IsReady() && target.IsValidTarget(Spells.R.Range))
                {
                    Spells.TiamatMelee.Cast();
                }
            }

            if (target.HealthPercent <= ComboMenu["Whealth"].Cast<Slider>().CurrentValue)
            {
                if (ComboMenu["W"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && target.IsValidTarget(Spells.W.Range))
                {
                    Spells.W.Cast(target);
                }
            }

            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                var Enemy = EntityManager.Heroes.Enemies.FirstOrDefault(x => x.IsValidTarget(Spells.E.Range) && x.IsValid);

                if (target.IsValidTarget(Spells.R.Range) && Spells.R.IsReady()
                    && Player.Instance.CountEnemiesInRange(250) >= ComboMenu["REnemies"].Cast<Slider>().CurrentValue)
                {
                    if (Enemy.HealthPercent <= ComboMenu["Rtargethealth"].Cast<Slider>().CurrentValue)
                {
                        Spells.R.Cast();
                    }
                }
               }
            }
        }
    }
