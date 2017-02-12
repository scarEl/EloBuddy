using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System.Drawing;
using EloBuddy.SDK.Enumerations;
using Color = System.Drawing;
using static Rank1Xerath.Program;
using static Rank1Xerath.Menus;
using static Rank1Xerath.Spells;

namespace Rank1Xerath
{
    public static class Drawings
    {
        public static void CreateDrawings()
        {
            Drawing.OnDraw += OnDraw;
            Drawing.OnDraw += OnDraw2;
            Drawing.OnDraw += OnDraw3;
        }


        private static void OnDraw(EventArgs args)
        {
            if (DrawingsMenu["DrawQHitBox"].Cast<CheckBox>().CurrentValue)
            {
                AIHeroClient target = TargetSelector.GetTarget(Q.Range, DamageType.Magical, Player.Instance.Position);
                //var qpred = Prediction.Manager.GetPrediction(QDATA);
                if (Q.IsReady() && target != null)
                {
                    var qpred = Q.GetPrediction(target);
                    Geometry.Polygon.Rectangle prediction2 = new Geometry.Polygon.Rectangle(
                        Player.Instance.Position.To2D(), qpred.CastPosition.To2D(), Q.Width);
                    prediction2.Draw(Color.Color.Yellow, 1);
                }
            }

            if (DrawingsMenu["DrawWHitBox"].Cast<CheckBox>().CurrentValue)
            {
                AIHeroClient target = TargetSelector.GetTarget(W.Range, DamageType.Magical, Player.Instance.Position);
                //var qpred = Prediction.Manager.GetPrediction(QDATA);
                if (W.IsReady() && target != null)
                {
                    Geometry.Polygon.Circle prediction2 = new Geometry.Polygon.Circle(Player.Instance.Position, W.Range);
                    prediction2.Draw(Color.Color.Green, 1);
                }
            }

            if (DrawingsMenu["DrawEHitBox"].Cast<CheckBox>().CurrentValue)
            {
                AIHeroClient target = TargetSelector.GetTarget(E.Range, DamageType.Magical, Player.Instance.Position);
                //var qpred = Prediction.Manager.GetPrediction(QDATA);
                if (E.IsReady() && target != null)
                {
                    var epred = E.GetPrediction(target);
                    Geometry.Polygon.Rectangle prediction2 = new Geometry.Polygon.Rectangle(
                        Player.Instance.Position.To2D(), epred.CastPosition.To2D(), E.Width);
                    prediction2.Draw(Color.Color.Orange, 1);
                }
            }

            if (DrawingsMenu["DrawRHitBox"].Cast<CheckBox>().CurrentValue)
            {
                AIHeroClient target = TargetSelector.GetTarget(R.Range, DamageType.Magical, Player.Instance.Position);
                //var qpred = Prediction.Manager.GetPrediction(QDATA);
                if (IsCastingUlt && target != null)
                {
                    Geometry.Polygon.Circle prediction2 = new Geometry.Polygon.Circle(Player.Instance.Position,R.Range);
                    prediction2.Draw(Color.Color.Red, 1);
                }
            }
        }

        private static void OnDraw2(EventArgs args)
        {

            if (DrawingsMenu["DrawingsDeactivate"].Cast<CheckBox>().CurrentValue) return;

            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue && Q.Range == Q.MinimumRange)
            {
                Circle.Draw(SharpDX.Color.DarkBlue, Q.MinimumRange, Player.Instance);
            }
            else if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue && Q.Range == Q.MaximumRange)
            {
                Circle.Draw(SharpDX.Color.DarkBlue, Q.MaximumRange, Player.Instance);
            }

            if (DrawingsMenu["DW"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SharpDX.Color.DarkOrange, W.Range, Player.Instance);
            }

            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SharpDX.Color.DarkRed, E.Range, Player.Instance);
            }

            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(SharpDX.Color.DarkSeaGreen, E.Range, Player.Instance);
            }
        }


        private static void OnDraw3(EventArgs args)
        {
            AIHeroClient target = TargetSelector.GetTarget(Q.MaximumRange, DamageType.Magical, Player.Instance.Position);
            if (Player.Instance.IsDead || DrawingsMenu["HitchanceDrawingsDeactivate"].Cast<CheckBox>().CurrentValue)
                return;
            //var qpred = Prediction.Manager.GetPrediction(QDATA);

            /*if (DrawingsMenu["HitchanceDrawingsDeactivate"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }*/

            if (DrawingsMenu["DQHitchance"].Cast<CheckBox>().CurrentValue && target != null)
            {
                QDATA.Target = target;
                var qpred = Prediction.Manager.GetPrediction(QDATA);

                Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position).X - 50,
                            Drawing.WorldToScreen(Player.Instance.Position).Y + 10,
                            Color.Color.Yellow,
                            "Hitchance %: ");
                Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position).X + 37,
                                    Drawing.WorldToScreen(Player.Instance.Position).Y + 10,
                                    Color.Color.Green,
                                    qpred.HitChancePercent.ToString());
            }
        }
    }
}