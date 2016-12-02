using System;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using static Galio_Template.Menus;
using static Galio_Template.Spells;
using EloBuddy.SDK.Menu.Values;

namespace Galio_Template
{
    internal class Drawings
    {
        public static void CreateDrawings()
        {
            Drawing.OnDraw += Drawing_OnDraw;
        }


        private static void Drawing_OnDraw(EventArgs args)
        {

            if (DrawingsMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                Circle.Draw(SharpDX.Color.Orange, Q.Range, Player.Instance);
            }

            if (DrawingsMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                Circle.Draw(SharpDX.Color.Crimson, W.Range, Player.Instance);
            }

            if (DrawingsMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                Circle.Draw(SharpDX.Color.Beige, E.Range, Player.Instance);
            }
            if (DrawingsMenu["R"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                Circle.Draw(SharpDX.Color.Aqua, R.Range, Player.Instance);
            }

        }
    }
}
