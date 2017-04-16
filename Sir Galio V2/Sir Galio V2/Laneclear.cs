using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using static Sir_Galio_V2.Menus;
using static Sir_Galio_V2.Spells;

namespace Sir_Galio_V2
{
    internal class Laneclear
    {
        public static void LaneExecute()
        {
            var m1 = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Q.Range);
            if (m1 != null)
            {
                if (LaneClearMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()) //range missing / Fix
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["qmana"].Cast<Slider>().CurrentValue)
                    {
                        Q.Cast(m1);
                    }
                }
            }
            var m2 = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, E.Range);
            if (m2 != null)
            {
                if (LaneClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady()) //range missing / Fix
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["emana"].Cast<Slider>().CurrentValue)
                    {
                        E.Cast(m2);
                    }
                }
            }
        }
    }
}