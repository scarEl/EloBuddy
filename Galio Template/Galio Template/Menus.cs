using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Galio_Template
{
    internal class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu DrawingsMenu;

        public static void CreateMenu()
        {
            // this will print (if you press shift) a big "Galio Template" on your Menu
            FirstMenu = MainMenu.AddMenu(Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "Template");

            // these 2 will show if you press Shift and on your name, which would be "Galio Template" for me, 
            // it will show those 2 texts between " "
            ComboMenu = FirstMenu.AddSubMenu("Combo name since this will be our Combo");
            DrawingsMenu = FirstMenu.AddSubMenu("Drawings name example");

            //examples
            ComboMenu.AddGroupLabel("Combo Settings"); //Combo Settings will be on top of the actual menu name which is a SubMenu
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W")); // we wont add the W usage since its a bit more complex
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("R", new CheckBox("Use R"));
            ComboMenu.Add("REnemies", new Slider("Min enemies for R cast"));

            //drawings
            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("Q", new CheckBox("Draw Q"));
            DrawingsMenu.Add("W", new CheckBox("Draw W"));
            DrawingsMenu.Add("E", new CheckBox("Draw E"));
            DrawingsMenu.Add("R", new CheckBox("Draw R"));
        }
    }
}