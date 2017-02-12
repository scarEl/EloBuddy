using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Rank1Xerath.Menus;
using static Rank1Xerath.Spells;

namespace Rank1Xerath
{
    internal class LaneClear
    {
        public static void LaneExecute()
        {
            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Q.MaximumRange, false).ToArray();

            if (LaneClearMenu["LQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                if (Player.Instance.ManaPercent >= LaneClearMenu["LMana"].Cast<Slider>().CurrentValue)
                {
                    foreach (var minion in lanemonster)
                    {
                        if (!Q.IsCharging)
                        {
                            Q.StartCharging();
                            return;
                        }

                        if (minion.IsValidTarget())
                        {
                            Q.Cast(minion);
                        }
                    }
                }
            }

            if (LaneClearMenu["LW"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                if (Player.Instance.ManaPercent >= LaneClearMenu["LMana"].Cast<Slider>().CurrentValue)
                {
                    foreach (var minion in lanemonster)
                    {
                        if (minion.IsValidTarget())
                        {
                            W.Cast(W.GetBestCircularCastPosition(lanemonster).CastPosition);
                        }
                    }
                }
            }
        }
    }
}