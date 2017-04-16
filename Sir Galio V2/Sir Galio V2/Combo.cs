using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using System.Drawing;
using static Sir_Galio_V2.Menus;
using static Sir_Galio_V2.Spells;
using static Sir_Galio_V2.Program;
using Color = SharpDX.Color;

namespace Sir_Galio_V2
{
    internal class Combo
    {
        public static void ComboExecute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if ((target == null) || target.IsInvulnerable) return;

            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Q.Range) && Q.IsReady())
                {
                    var qhitchance = Q.GetPrediction(target);
                    if (qhitchance.HitChancePercent >= ComboMenu["qhit"].Cast<Slider>().CurrentValue)
                    {
                        Q.Cast(qhitchance.CastPosition);
                    }
                }
            }
            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(E.Range) && E.IsReady())
                {
                    var ehitchance = E.GetPrediction(target);
                    if (ehitchance.HitChancePercent >= ComboMenu["ehit"].Cast<Slider>().CurrentValue)
                    {
                        E.Cast(ehitchance.CastPosition);
                    }
                }
            }

            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
            {
                if (W.IsReady() && target.IsValidTarget(W.Range))
                {
                    W.StartCharging();
                    if (target.IsInRange(Player.Instance, W.MaximumRange)) W.Cast();
                }
            }

            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {
                Game.CursorPos.DrawCircle(800,Color.LimeGreen);

                var enemy =
                    EntityManager.Heroes.Enemies.FirstOrDefault(x => x.IsValidTarget(R.Range) && x.IsValid);

                if (enemy.IsValidTarget(R.Range) && R.IsReady()
                    &&
                    Game.CursorPos.CountAllyChampionsInRange(800) >= FirstMenu["rEnemies"].Cast<Slider>().CurrentValue)
                {
                    R.Cast();
                }
            }
        }
    }
}