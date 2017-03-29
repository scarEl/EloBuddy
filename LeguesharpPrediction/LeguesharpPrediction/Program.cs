using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;
using Color = System.Drawing;

namespace LeaguesharpPrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Onload;
        }

        private static void Onload(EventArgs args)
        {
            Chat.Print(">> Leguesharp Prediction loaded. kek.", Color.Color.BlueViolet);
        }
    }
}
