using System;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using static Polar_Bear.Menus;
using static Polar_Bear.Spells;
using static Polar_Bear.Program;
using EloBuddy.SDK.Menu.Values;

namespace Polar_Bear
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

            if (DrawingsMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                Circle.Draw(SharpDX.Color.Crimson, E.Range, Player.Instance);
            }

        }
    }
}
