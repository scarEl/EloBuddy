using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;


namespace Xerath
{
    internal class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu DrawingsMenu;


        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu(Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "Template");
            ComboMenu = FirstMenu.AddSubMenu("Combo");
            DrawingsMenu = FirstMenu.AddSubMenu("Drawings");

            ComboMenu.AddGroupLabel("Combo Options");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("R", new CheckBox("Use R"));

            DrawingsMenu.AddGroupLabel("Drawings");
            DrawingsMenu.Add("Q", new CheckBox("Draw Q"));
            DrawingsMenu.Add("W", new CheckBox("Draw W"));
            DrawingsMenu.Add("E", new CheckBox("Draw E"));
            DrawingsMenu.Add("R", new CheckBox("Draw R"));






        }
    }
}