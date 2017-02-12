using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Rank1Xerath
{
    public static class Menus
    {
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu JungleClearMenu;
        public static Menu ActivatorMenu;
        public static Menu MiscMenu;
        public static Menu DrawingsMenu;
        public static Menu LaneClearMenu;
        public static Menu FleeMenu;


        public static void CreateMenu()
        {
            FirstMenu = MainMenu.AddMenu("Rank1" + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower());
            ComboMenu = FirstMenu.AddSubMenu("> Combo");
            HarassMenu = FirstMenu.AddSubMenu("> Harass");
            LaneClearMenu = FirstMenu.AddSubMenu("> Lane");
            JungleClearMenu = FirstMenu.AddSubMenu("> Jungle");
            FleeMenu = FirstMenu.AddSubMenu("> Flee");
            DrawingsMenu = FirstMenu.AddSubMenu("> Drawings");
            ActivatorMenu = FirstMenu.AddSubMenu("> Activator");
            MiscMenu = FirstMenu.AddSubMenu("> Misc");

            FirstMenu.AddGroupLabel("> Base Hitchances (Combo) <");
            FirstMenu.AddLabel("> Q = 80%");
            FirstMenu.AddLabel("> W = 70%");
            FirstMenu.AddLabel("> E = 80%");
            FirstMenu.AddLabel("> R = see Mode 1-5");
            FirstMenu.AddLabel("If you want changes, report it to Ouija");

            ComboMenu.AddGroupLabel("> Combo <");
            ComboMenu.Add("CQ", new CheckBox("Use Q in Combo"));
            ComboMenu.Add("CW", new CheckBox("Use W in Combo"));
            ComboMenu.Add("CE", new CheckBox("Use E in Combo"));
            ComboMenu.Add("CR", new CheckBox("Use R in Combo"));
            ComboMenu.Add("CRSliderHitchance", new Slider("R Hitchance",defaultValue:80,minValue:0,maxValue:99));
            ComboMenu.AddLabel("Default Value is 80 because at 99% it wont cast the R often");
            /*ComboMenu.AddSeparator();
            ComboMenu.AddLabel("Mode 1: Inhuman - 99%" + " Hitchance");
            ComboMenu.AddLabel("Mode 2: Good -  90%" + " Hitchance" + " Recommended!");
            ComboMenu.AddLabel("Mode 3: OK   -  80%" + " Hitchance");
            ComboMenu.AddLabel("Mode 4: 70%" + " Hitchance");
            ComboMenu.AddLabel("Mode 5: 60%" + " Hitchance");*/

            HarassMenu.AddGroupLabel("> Harass <");
            HarassMenu.Add("HQ", new CheckBox("Use Q in Harass"));
            HarassMenu.Add("HW", new CheckBox("Use W in Harass"));
            HarassMenu.Add("HE", new CheckBox("Use E in Harass",false));
            HarassMenu.AddLabel("Usage for E is deactivated because of mana");

            LaneClearMenu.AddGroupLabel("> Lane <");
            LaneClearMenu.Add("LMana", new Slider("If mana is under [{0}%] do not Laneclear", defaultValue:30,minValue:0,maxValue:99));
            LaneClearMenu.AddLabel("Default Value -> 30");
            LaneClearMenu.Add("LQ", new CheckBox("Use Q in Laneclear"));
            LaneClearMenu.Add("LW", new CheckBox("Use W in Laneclear"));

            JungleClearMenu.AddGroupLabel("> Jungle <");
            JungleClearMenu.Add("JMana", new Slider("If mana is under [{0}%] do not Jungleclear", defaultValue: 30, minValue: 0, maxValue: 99));
            JungleClearMenu.AddLabel("Default Value -> 30");
            JungleClearMenu.Add("JQ", new CheckBox("Use Q in Jungleclear"));
            JungleClearMenu.Add("JW", new CheckBox("Use W in Jungleclear"));

            FleeMenu.AddGroupLabel("> Flee <");
            FleeMenu.Add("FE", new CheckBox("Use E in Flee"));
            FleeMenu.AddLabel("This will Stun an enemy while you are running away");

            DrawingsMenu.AddGroupLabel("> HitBoxes <");
            DrawingsMenu.Add("DrawQHitBox", new CheckBox("Draw Q Hit Box"));
            //DrawingsMenu.Add("DrawWHitBox", new CheckBox("Draw W Hit Box",false));
            DrawingsMenu.Add("DrawEHitBox", new CheckBox("Draw E Hit Box"));
            //DrawingsMenu.Add("DrawRHitBox", new CheckBox("Draw R Hit Box",false));
            DrawingsMenu.AddLabel("All these are Experimental!");
            DrawingsMenu.AddSeparator();
            DrawingsMenu.AddGroupLabel("> Drawings <");
            DrawingsMenu.Add("DrawingsDeactivate", new CheckBox("Deactivate all Drawings",false));
            DrawingsMenu.Add("DQ", new CheckBox("Draw Q range"));
            DrawingsMenu.Add("DW", new CheckBox("Draw W range"));
            DrawingsMenu.Add("DE", new CheckBox("Draw E range"));
            DrawingsMenu.Add("DR", new CheckBox("Draw R range"));
            DrawingsMenu.AddSeparator();
            DrawingsMenu.AddGroupLabel("> Hitchance Drawings <");
            DrawingsMenu.Add("HitchanceDrawingsDeactivate", new CheckBox("Deactivate Hitchance Drawings",false));
            DrawingsMenu.Add("DQHitchance", new CheckBox("Draw Possible Hitchance on Target"));

            ActivatorMenu.AddGroupLabel("> Activator <");
            ActivatorMenu.Add("Zhonyas", new CheckBox("Use Zhonyas",false));
            ActivatorMenu.AddLabel("For now there is only Zhonyas (Experimental)");
            ActivatorMenu.Add("Ignite", new CheckBox("Use Ignite"));

            MiscMenu.AddGroupLabel("> Misc <");
            MiscMenu.Add("IE", new CheckBox("Use E to Stun an enemy while he is casting"));
            MiscMenu.AddSeparator();
            MiscMenu.Add("GE", new CheckBox("Use E to Stun an enemy while he runs towards you"));
            MiscMenu.Add("DamageIndicator", new CheckBox("Use Damage Indicator"));
            //MiscMenu.Add("DrawHealth", new CheckBox("Draw on the enemy hpbar"));
            //MiscMenu.AddLabel("after Deactivating/Activating the Damage Indicator, Press F5");
        }
    }
}