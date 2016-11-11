using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Polar_Bear.Menus;
using static Polar_Bear.Spells;

namespace Polar_Bear
{
    internal static class JungleClear
    {
        public static void Execute1()
        {
            var junglemonster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, E.Range);

            if (JungleClearMenu["W"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                foreach (var minion in junglemonster)
                {
                    if (minion.IsValidTarget())
                    {
                        W.Cast(minion);
                    }
                }
            }
            if (JungleClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                foreach (var minion in junglemonster)
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
