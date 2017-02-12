using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Rank1Xerath.Menus;
using static Rank1Xerath.Spells;

namespace Rank1Xerath
{
    public static class JungleClear
    {
        public static void JungleExecute()
        {
            var junglemonster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, W.Range);

            if (JungleClearMenu["JQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                foreach (var jungleminion in junglemonster)
                {
                    if (!Q.IsCharging) Q.StartCharging();

                    if (jungleminion.IsValid)
                    {
                        Q.Cast(jungleminion);
                    }
                }
            }

            if (JungleClearMenu["JW"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                foreach (var jungleminion in junglemonster)
                {
                    if (!Q.IsCharging) Q.StartCharging();

                    if (jungleminion.IsValid)
                    {
                        W.Cast(jungleminion);
                    }
                }
            }
        }
    }
}