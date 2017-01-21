using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using static Xerath.Menus;
using static Xerath.Spells;
using static Xerath.Program;
using EloBuddy.SDK.Menu.Values;

namespace Xerath
{
    internal class Drawings
    {
        public static Spell.Chargeable Q;

        public static void CreateDrawings()
        {
            Drawing.OnDraw += Drawing_OnDraw;
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            if (DrawingsMenu["Q"].Cast<CheckBox>().CurrentValue && Q.Range == Q.MinimumRange && Q.IsReady())
            {
                Circle.Draw(SharpDX.Color.Orange, Q.MinimumRange, Player.Instance);
            }
            else if (DrawingsMenu["Q"].Cast<CheckBox>().CurrentValue && Q.Range == Q.MaximumRange && Q.IsReady())
            {
                Circle.Draw(SharpDX.Color.Orange, Q.MaximumRange, Player.Instance);
            }

            if (DrawingsMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {

            }

            if (DrawingsMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {

            }

            if (DrawingsMenu["R"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {

            }
        }
    }
}