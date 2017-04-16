using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using static Galio_Template.Combo;
using static Galio_Template.LaneClear;

namespace Galio_Template
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        public static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Galio) return;
            //this is optional
            Chat.Print("Your Text"); //this will be print if your game started
            //This is needed! you can use this as a different option:
            //(OnTick is slower than OnUpdate but takes less resources!)
            //Game.OnUpdate += OnUpdate;
            Game.OnTick += OnTick;
            Menus.CreateMenu();
            Spells.InitializeSpells();
            Drawings.CreateDrawings();
        }

        private static void OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ComboExecute();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneExecute();
        }
    }
}
