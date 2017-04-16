using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;
using System.Net;
using static Sir_Galio_V2.Combo;
using static Sir_Galio_V2.Laneclear;
using static Sir_Galio_V2.Jungleclear;
using static Sir_Galio_V2.Damagehandler;

namespace Sir_Galio_V2
{
    class Program
    {
        public static uint rrange = 1;
        static readonly string localVersion = "7.4.1";

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (Player.Instance.Hero == Champion.Galio)
                Chat.Print("Sir Galio V2: Sucessfully Loaded!" + "Version: " + localVersion, Color.IndianRed);
            //UpdateChecks();
            Menus.CreateMenu();
            Spells.InitializeSpells();
            Game.OnTick += OnTick;
            DamageHandlerLoad(); // kek
            Drawings.CreateDrawings();
            Chat.Print(rrange);

            if (Player.Instance.Hero != Champion.Galio)
            {
                Chat.Print("Sir Galio V2: Loading failed!", Color.Red);
                return;
            }
        }

        private static void OnTick(EventArgs args)
        {
            //rRangeLevelHandler();
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                ComboExecute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneExecute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleExecute();
            }
        }

        private static void rRangeLevelHandler()
        {
            if (Player.Instance.Level == 6)
            {
                rrange = 4000;
                //Chat.Print("[R] Range set to [" + rrange + "] !");
            }
            else if (Player.Instance.Level == 11)
            {
                rrange = 4750;
                //Chat.Print("[R] Range set to [" + rrange + "] !");

            }
            else if (Player.Instance.Level == 16)
            {
                rrange = 5500;
                //Chat.Print("[R] Range set to [" + rrange + "] !");
            }
        }

        private static void UpdateChecks()
        {
            Chat.Print("--------------------------------------------------------------------------------");
            WebClient client = new WebClient();
            string version =
                client.DownloadString("https://raw.githubusercontent.com/Ouija01/Elobuddy/master/Sir Galio V2/Version");
            if (!version.Remove(4).Equals(localVersion))
            {
                Chat.Print("!!Sir Galio V2 needs to be updated, Press F6 first, then F5, thanks!!", Color.Red);
            }
        }
    }
}
