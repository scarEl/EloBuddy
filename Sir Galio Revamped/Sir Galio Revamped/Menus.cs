using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using static Sir_Galio_Revamped.Program;

namespace Sir_Galio_Revamped
{
    internal static class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu Rhelper;
        public static Menu ActivatorMenu;
        public static Menu DrawingsMenu;
        public static Menu LaneClearMenu;
        public static Menu JungleClearMenu;
        //public static Menu Awareness;

        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Sir " + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + " Revamped");
            Rhelper = FirstMenu.AddSubMenu("> R Helper");
            ComboMenu = FirstMenu.AddSubMenu("> Combo Settings");
            LaneClearMenu = FirstMenu.AddSubMenu("> Lane Settings");
            JungleClearMenu = FirstMenu.AddSubMenu("> Jungle Settings");
            ActivatorMenu = FirstMenu.AddSubMenu("> Activator Settings");
            DrawingsMenu = FirstMenu.AddSubMenu("> Drawings Settings");
            //Awareness = FirstMenu.AddSubMenu("Awareness");

            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("qhit", new Slider("Hitchance of Q", 75, 1));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("ehit", new Slider("Hitchance of E", 80, 1));
            //ComboMenu.Add("R", new CheckBox("Use R (Experimental)"));
            ComboMenu.AddLabel("R usage will be added later on.");

            Rhelper.AddGroupLabel("Rhelper");
            Rhelper.Add("cursorLime", new CheckBox("Draw Green Circle around mouse"));
            Rhelper.Add("sliderCursorLime", new Slider("How big should the circle be? {0} Units", 800, 1, 2000));
            Rhelper.AddLabel("this is to cast your ult when X enemies around it");
            Rhelper.Add("rEnemies", new Slider("if greater or equal {0} enemies, cast R (LimeGreen circle)", 3, 1, 5));

            LaneClearMenu.AddGroupLabel("Laneclear");
            LaneClearMenu.Add("Q", new CheckBox("Use Q"));
            LaneClearMenu.Add("qmana", new Slider("Minimum Mana for Q Cast", 30, 1));
            LaneClearMenu.Add("E", new CheckBox("Use E",false));
            LaneClearMenu.Add("emana", new Slider("Minimum Mana for E Cast", 30, 1));

            JungleClearMenu.AddGroupLabel("Jungleclear");
            JungleClearMenu.Add("Q", new CheckBox("Use Q"));
            JungleClearMenu.Add("qmana", new Slider("Minimum Mana for Q Cast", 30, 1));
            JungleClearMenu.Add("E", new CheckBox("Use E",false));
            JungleClearMenu.Add("emana", new Slider("Minimum Mana for E Cast", 30, 1));

            DrawingsMenu.AddGroupLabel("Drawings");
            DrawingsMenu.Add("qdraw", new CheckBox("Draw Q Range"));
            DrawingsMenu.Add("wdraw", new CheckBox("Draw w Range"));
            DrawingsMenu.Add("edraw", new CheckBox("Draw E Range"));
            DrawingsMenu.Add("rdraw", new CheckBox("Draw R Range"));
            //soon tm Bug
            /*Awareness.AddGroupLabel("Awareness");
            Awareness.Add("drawAllTargets", new CheckBox("Draw Lines to all targets in X Range",false));
            Awareness.Add("drawAllTargetsRange", new Slider("The range for the upper function", 1300, 1, 5000));*/

            ActivatorMenu.AddGroupLabel("Activator");
            ActivatorMenu.AddLabel("When i have enough motivation to do this, i'll add it.");
        }
    }
}