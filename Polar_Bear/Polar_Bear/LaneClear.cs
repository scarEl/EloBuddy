using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Polar_Bear.Menus;
using static Polar_Bear.Spells;
using static Polar_Bear.Program;

namespace Polar_Bear
{
    internal static class LaneClear
    {
        public static void LaneExecute()
        {
            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, E.Range);

            if (LaneClearMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                if (Player.Instance.ManaPercent <= LaneClearMenu["ManaW"].Cast<Slider>().CurrentValue)
                {
                    foreach (var minion in lanemonster)
                    {
                        if (minion.IsValidTarget())
                        {
                            W.Cast(minion);
                        }
                    }
                }
            }

            if (LaneClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                if (Player.Instance.CountEnemyMinionsInRange(E.Range) <= LaneClearMenu["E"].Cast<Slider>().CurrentValue)
                {
                    foreach (var minion in lanemonster)
                    {
                        if (minion.IsValidTarget())
                        {
                            E.Cast();
                        }
                    }
                }
            }
        }
    }
}
