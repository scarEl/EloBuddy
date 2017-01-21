using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.Sandbox;

namespace Xerath
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;



        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Xerath) return;
            Spells.InitializeSpells();
            Menus.CreateMenu();
            //Added by Ouija
            Drawings.CreateDrawings();
            //Added by Ouija
            Chat.Print("<font color = #000099>Xerath Loaded</font>");
            //this prints my shit on the chat
            Game.OnTick += OnTick;
            //uses ontick







        }

        private static void OnTick(EventArgs args)
        {

            if (SandboxConfig.Username.Contains("eggbrother"))
            {
                Chat.Say("Hi my name is eggbrother and im a scripter. PLZ REPORT ME");


            }


            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ExecuteCombo();
        }

        private static void ExecuteCombo()
        {

        }



    }
}
//**paona is the master-pleb**

//report melanie for doxing me
//Wanna swap name yorik100?
//nice reputation eggbrother kappa
//hi ouija nice guide
//fuck my life got doxed by melanie kappa123
//chaos teach me some of ur coding skillz
//why are u checking out my spaghetti Zaloopa?
//www.hesabot.com
