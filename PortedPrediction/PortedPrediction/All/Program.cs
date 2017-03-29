using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace PortedPrediction.All
{
    public class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }


        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            Chat.Print("PortedPrediction loaded! (Ported from Sebby)", Color.Aqua);
        }
    }
}