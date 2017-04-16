using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using static Sir_Galio_V2.Menus;
using static Sir_Galio_V2.Spells;

namespace Sir_Galio_V2
{
    internal class Drawings
    {
        public static void CreateDrawings()
        {
            Drawing.OnDraw += Drawing_OnDraw;
        }
        static void Drawing_OnDraw(EventArgs args)
        {

            if (DrawingsMenu["qdraw"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                Q.DrawRange(System.Drawing.Color.Orange);
            }

            if (DrawingsMenu["wdraw"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, W.MinimumRange, Color.AliceBlue);
            }
            if (DrawingsMenu["wdraw"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, W.MaximumRange, Color.AliceBlue);
            }

            if (DrawingsMenu["edraw"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                E.DrawRange(System.Drawing.Color.Crimson);
            }

            if (DrawingsMenu["rdraw"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                R.DrawRange(System.Drawing.Color.LawnGreen);
            }

        }
    }
}