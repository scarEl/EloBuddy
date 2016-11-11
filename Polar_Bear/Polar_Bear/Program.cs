using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using static Polar_Bear.Combo;
using static Polar_Bear.JungleClear;
using static Polar_Bear.Spells;
using static Polar_Bear.Menus;
using static Polar_Bear.Drawings;

namespace Polar_Bear
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }


        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Volibear ) return;
            Chat.Print("Be the Jungle King!");
            Game.OnUpdate += Game_OnUpdate;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;

            Menus.CreateMenu();
            Spells.InitializeSpells();
            Drawings.CreateDrawings();
        }

        private static void Game_OnUpdate(EventArgs args)
        {

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ComboExecute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) JungleExecute();
        }

        static void Gapcloser_OnGapcloser(Obj_AI_Base sender, Gapcloser.GapcloserEventArgs args)
        {
            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);

            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(Player.Instance) < Spells.E.Range && Spells.E.IsReady() && MiscMenu["EGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                Spells.E.Cast(targetE);
            }

            var targetQ = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(Player.Instance) < Spells.Q.Range && Spells.Q.IsReady() && MiscMenu["QGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                Spells.Q.Cast(targetQ);
            }
        }
    }
}
