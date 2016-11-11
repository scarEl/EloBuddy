using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Polar_Bear
{
    internal class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu JungleClearMenu;
        public static Menu ActivatorMenu;
        public static Menu MiscMenu;
        public static Menu SmiteMenu;


        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Polar"+ Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower() + "Bear" );
            ComboMenu = FirstMenu.AddSubMenu("Combo Settings");
            JungleClearMenu = FirstMenu.AddSubMenu("Jungle Settings");
            ActivatorMenu = FirstMenu.AddSubMenu("Activator Settings");

            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddLabel("Big Thanks to Wladi0");
            ComboMenu.Add("Q", new CheckBox("Use Q"));
            ComboMenu.Add("W", new CheckBox("Use W"));
            ComboMenu.Add("E", new CheckBox("Use E"));
            ComboMenu.Add("R", new CheckBox("Use R"));
            ComboMenu.Add("Whealth", new Slider("Enemys health in % to use W", 15, 0, 100));
            ComboMenu.Add("REnemies", new Slider("Enemies in Range to use R", 2, 1, 5));
            ComboMenu.Add("Rtargethealth", new Slider("Enemys Health in % to use R", 40, 0, 100));

            MiscMenu.AddGroupLabel("Misc Settings");
            MiscMenu.Add("QGapCloser", new CheckBox("Use Q on GapClose"));
            MiscMenu.Add("EGapCloser", new CheckBox("Use E on GapClose"));

            JungleClearMenu.AddGroupLabel("JungleClear Settings");
            JungleClearMenu.Add("W", new CheckBox("Use W"));
            JungleClearMenu.Add("E", new CheckBox("Use E"));

            SmiteMenu.AddGroupLabel("Smite Settings");
            //SmiteMenu.Add("Smite", new CheckBox("Use Smite (useless atm)")); //todo
            SmiteMenu.AddLabel("Currently not Supported, Use a extra Smite Addon."); //useless stuff

            ActivatorMenu.AddGroupLabel("Activator Settings");
            ActivatorMenu.AddLabel("Supports only most used Items for example: Hydra");
            ActivatorMenu.Add("Hydra", new CheckBox("Use Hydra(all)"));

        }
    }
}
