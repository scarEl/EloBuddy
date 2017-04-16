using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Linq;
using static Sir_Galio_V2.Menus;
using static Sir_Galio_V2.Spells;

namespace Sir_Galio_V2
{
    internal class Jungleclear
    {
        public static void JungleExecute()
        {
            if (JungleClearMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()) //range missing
            {
                var target = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, Q.Range);
                if (Player.Instance.ManaPercent >= LaneClearMenu["qmana"].Cast<Slider>().CurrentValue)
                {
                    Q.Cast(target);
                }
            }

            if (JungleClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady()) //range missing
            {
                var m2 = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, E.Range);
                if (m2 != null)
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