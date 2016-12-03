using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static Galio_Template.Menus;

namespace Galio_Template
{
    internal class LaneClear
    {
        public static void LaneExecute()
        {
            var Qlanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.Q.Range);
            var Elanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Spells.E.Range);

            if (LaneMenu["Q"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady())
            {
                if (Player.Instance.ManaPercent >= LaneMenu["MinManaQ"].Cast<Slider>().CurrentValue)
                {
                    foreach (var minion in Qlanemonster)
                    {
                        if (minion.IsValidTarget())
                        {
                            Spells.Q.Cast(minion);
                        }
                    }
                }
            }

            if (LaneMenu["E"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady())
            {
                foreach (var minion in Elanemonster)
                {
                    if (minion.IsValidTarget())
                    {
                        Spells.E.Cast(minion);
                    }
                }
            }
        }
    }
}
