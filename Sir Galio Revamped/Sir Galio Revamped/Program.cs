using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;
using System.Net;
using EloBuddy.SDK.Enumerations;
using static Sir_Galio_Revamped.Menus;
using static Sir_Galio_Revamped.Damagehandler;

namespace Sir_Galio_Revamped
{
    class Program
    {
        public static Spell.Skillshot Q, E, R;
        public static Spell.Chargeable W;

        public static uint rrange = 1;
        static readonly string localVersion = "7.7";

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            //UpdateChecks();
            if (Player.Instance.Hero == Champion.Galio)
                Chat.Print("Sir Galio Revamped: Sucessfully Loaded!" + "Version: " + localVersion, Color.IndianRed);
            else if (Player.Instance.Hero != Champion.Galio)
            {
                Chat.Print("Sir Galio Revamped: Loading failed!", Color.Red);
                return;
            }
            CreateMenu();
            InitializeSpells();
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            DamageHandlerLoad(); // kek
            Chat.Print(rrange);
        }

// Drawings

        private static void OnDraw(EventArgs args)
        {
            //var targetdrawings = TargetSelector.GetTarget(5000, DamageType.Magical);

            /*if (Awareness["drawAllTargets"].Cast<CheckBox>().CurrentValue)
            {
                if (targetdrawings.IsInRange(Player.Instance.Position, Awareness["drawAllTargetsRange"].Cast<Slider>().CurrentValue))
                {
                    Drawing.DrawLine(Player.Instance.Position.X,Player.Instance.Position.Y,targetdrawings.Position.X,targetdrawings.Position.Y,2,Color.Chartreuse);
                }
            }*/

            if (Rhelper["cursorLime"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                Game.CursorPos.DrawCircle(Rhelper["sliderCursorLime"].Cast<Slider>().CurrentValue, SharpDX.Color.LimeGreen);
                Drawing.DrawText(Game.CursorPos.WorldToScreen().X,Game.CursorPos.WorldToScreen().Y-20,Color.LimeGreen, Rhelper["rEnemies"].Cast<Slider>().CurrentValue.ToString());
            }

            if (DrawingsMenu["qdraw"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                Q.DrawRange(Color.Orange);
            }

            if (DrawingsMenu["wdraw"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, W.MinimumRange, Color.Teal);
            }
            if (DrawingsMenu["wdraw"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                Drawing.DrawCircle(Player.Instance.Position, W.MaximumRange, Color.Teal);
            }

            if (DrawingsMenu["edraw"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                E.DrawRange(Color.Crimson);
            }

            if (DrawingsMenu["rdraw"].Cast<CheckBox>().CurrentValue && R.IsReady())
            {
                R.DrawRange(Color.LawnGreen);
            }
        }

        private static void InitializeSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 250, 500, 400);
            W = new Spell.Chargeable(SpellSlot.W, 275, 450, 0);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Linear, 250, 300, 150)
            {
                AllowedCollisionCount = 1
            };
            R = new Spell.Skillshot(SpellSlot.R, 4000, SkillShotType.Circular);
        }

        private static void OnTick(EventArgs args)
        {
            //rRangeLevelHandler();
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                ComboExecute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneExecute();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleExecute();
            }
        }

// Combo

        private static void ComboExecute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if ((target == null) || target.IsInvulnerable) return;

            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(Q.Range) && Q.IsReady())
                {
                    var qhitchance = Q.GetPrediction(target);
                    if (qhitchance.HitChancePercent >= ComboMenu["qhit"].Cast<Slider>().CurrentValue)
                    {
                        Q.Cast(qhitchance.CastPosition);
                    }
                }
            }
            if (ComboMenu["E"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsValidTarget(E.Range) && E.IsReady())
                {
                    var ehitchance = E.GetPrediction(target);
                    if (ehitchance.HitChancePercent >= ComboMenu["ehit"].Cast<Slider>().CurrentValue)
                    {
                        E.Cast(ehitchance.CastPosition);
                    }
                }
            }

            if (ComboMenu["W"].Cast<CheckBox>().CurrentValue)
            {
                if (W.IsReady() && target.IsValidTarget(W.Range) && W.IsInRange(target) && Q.IsOnCooldown && E.IsOnCooldown)
                {
                    W.StartCharging();
                }
            }

            if (ComboMenu["R"].Cast<CheckBox>().CurrentValue)
            {

                var ally = EntityManager.Heroes.Allies.FirstOrDefault(hero => !hero.IsMe && !hero.IsInShopRange() && !hero.IsZombie && hero.Distance(Player.Instance) <= R.Range);

                if (ally.IsValidTarget(Rhelper["sliderCursorLime"].Cast<Slider>().CurrentValue) && R.IsReady()
                    &&
                    Game.CursorPos.CountEnemyChampionsInRange(Rhelper["sliderCursorLime"].Cast<Slider>().CurrentValue) >= Rhelper["rEnemies"].Cast<Slider>().CurrentValue)
                {
                    R.Cast(ally);
                }
            }
        }

// Lane

        private static void LaneExecute()
        {
            var laneminionQ = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range);
            var laneminionE = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range);

            if (LaneClearMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()) //range missing / Fix
            {
                foreach (var minionQ in laneminionQ)
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["qmana"].Cast<Slider>().CurrentValue)
                    {
                        Q.Cast(minionQ);
                    }
                }
            }
            if (LaneClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady()) //range missing / Fix
            {
                foreach (var minionE in laneminionE)
                {
                    if (Player.Instance.ManaPercent >= LaneClearMenu["emana"].Cast<Slider>().CurrentValue)
                    {
                        E.Cast(minionE);
                    }
                }
            }
        }

// Jungle

        private static void JungleExecute()
        {
            if (JungleClearMenu["Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()) //range missing
            {
                var jungleMonsterQ = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, Q.Range);
                if (jungleMonsterQ != null)
                {
                    foreach (var junglemonsterq in jungleMonsterQ)
                    {
                        if (Player.Instance.ManaPercent >= LaneClearMenu["qmana"].Cast<Slider>().CurrentValue)
                        {
                            Q.Cast(junglemonsterq);
                        }
                    }
                }
            }

            if (JungleClearMenu["E"].Cast<CheckBox>().CurrentValue && E.IsReady()) //range missing
            {
                var jungleMonsterE = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, E.Range);
                if (jungleMonsterE != null)
                {
                    foreach (var junglemonstere in jungleMonsterE)
                    {
                        if (Player.Instance.ManaPercent >= LaneClearMenu["emana"].Cast<Slider>().CurrentValue)
                        {
                            E.Cast(junglemonstere);
                        }
                    }
                }
            }
        }

        private static void UpdateChecks()
        {
            Chat.Print("--------------------------------------------------------------------------------");
            WebClient client = new WebClient();
            string version =
                client.DownloadString("https://raw.githubusercontent.com/Ouija01/EloBuddyVersions/master/SirGalio");
            if (version.Remove(4).Equals(localVersion))
            {
                Chat.Print("Your version of Sir Galio Revamped is up to date! Version: "+localVersion);
            }
            else
            {
                Chat.Print("!!Sir Galio Revamped needs to be updated, Press F6 first, then F5, thanks!!", Color.Red);
            }
        }
    }
}
