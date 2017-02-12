using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.Sandbox;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using static Rank1Xerath.Menus;
using static Rank1Xerath.Spells;
using static Rank1Xerath.Drawings;
using static Rank1Xerath.Combo;
using static Rank1Xerath.JungleClear;
using static Rank1Xerath.LaneClear;
using static Rank1Xerath.Flee;
using static Rank1Xerath.Harass;
using static Rank1Xerath.DamageIndicator;

namespace Rank1Xerath
{
    public static class Program
    {
        #region QWER Data

        public static Prediction.Manager.PredictionInput QDATA = new Prediction.Manager.PredictionInput
        {
            Delay = 0.25f,
            Radius = Q.Width,
            Range = Q.Range,
            Speed = Q.Speed,
            Type = SkillShotType.Linear,
            CollisionTypes =
            {
                EloBuddy.SDK.Spells.CollisionType.AiHeroClient,
                EloBuddy.SDK.Spells.CollisionType.ObjAiMinion,
                EloBuddy.SDK.Spells.CollisionType.YasuoWall
            }
        };

        public static Prediction.Manager.PredictionInput WDATA = new Prediction.Manager.PredictionInput
        {
            Delay = W.CastDelay,
            Radius = W.Width,
            Range = W.Range,
            Speed = W.Speed,
            Type = SkillShotType.Linear,
            CollisionTypes =
            {
                EloBuddy.SDK.Spells.CollisionType.AiHeroClient,
                EloBuddy.SDK.Spells.CollisionType.ObjAiMinion,
                EloBuddy.SDK.Spells.CollisionType.YasuoWall
            }
        };

        public static Prediction.Manager.PredictionInput EDATA = new Prediction.Manager.PredictionInput
        {
            Delay = E.CastDelay,
            Radius = E.Width,
            Range = E.Range,
            Speed = E.Speed,
            Type = SkillShotType.Linear,
            CollisionTypes =
            {
                EloBuddy.SDK.Spells.CollisionType.AiHeroClient,
                EloBuddy.SDK.Spells.CollisionType.ObjAiMinion,
                EloBuddy.SDK.Spells.CollisionType.YasuoWall
            }
        };

        public static Prediction.Manager.PredictionInput RDATA = new Prediction.Manager.PredictionInput
        {
            Delay = R.CastDelay,
            Radius = R.Width,
            Range = R.Range,
            Speed = R.Speed,
            Type = SkillShotType.Linear,
            CollisionTypes =
            {
                EloBuddy.SDK.Spells.CollisionType.AiHeroClient,
                EloBuddy.SDK.Spells.CollisionType.ObjAiMinion,
                EloBuddy.SDK.Spells.CollisionType.YasuoWall
            }
        };

        #endregion

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }


        public static void Loading_OnLoadingComplete(EventArgs args)
        {

            /*List<string> EBAllowedUsers = new List<string>();
            EBAllowedUsers.Add("Ouija");
            EBAllowedUsers.Add("FrenchStoleMyElo");
            EBAllowedUsers.Add("Chaos");
            EBAllowedUsers.Add("Haxory");
            EBAllowedUsers.Add("Rottefar");
            EBAllowedUsers.Add("x auf der brust");
            EBAllowedUsers.Add("lofex");*/


            /*if (EBAllowedUsers.Any(a => SandboxConfig.Username.Contains(a)) || SandboxConfig.IsBuddy)
            {*/
            if (Player.Instance.Hero != Champion.Xerath)
            {
                Chat.Print(Player.Instance.ChampionName + " this Champion is not supported", Color.IndianRed);
                return;
            }
            Chat.Print("Hello " + SandboxConfig.Username + " have a fun game!");

            Game.OnTick += Game_OnUpdate;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterrupt;

            Menus.CreateMenu();
            Spells.InitializeSpells();
            Drawings.CreateDrawings();
            if (ignt.Slot != SpellSlot.Unknown)
            {
                Ignite();
                Chat.Print("<font color='#E25822'>Loading Ignite usage</font>");
            }
            else
            {
                Chat.Print("<font color='#E25822'>Unloading Ignite usage</font>");
            }
            Zhonyas();
            {
                Chat.Print("<font color='#33B2FF'>Zhonyas is Experimental!</font>");
            }
            if (MiscMenu["DamageIndicator"].Cast<CheckBox>().CurrentValue)
            {
                DamageIndicator.Initialize(Damages.GetTotalDamage);
                DamageIndicator.DrawingColor = System.Drawing.Color.CadetBlue;
            }
        }


        private static void Ignite()
        {
            //Toyota7's Ignite usage
            var target = TargetSelector.GetTarget(700, DamageType.True, Player.Instance.Position);

            float IgniteDMG = 50 + (20*Player.Instance.Level);

            if (target != null)
            {
                float HP5 = target.HPRegenRate*5;

                if (ActivatorMenu["Ignite"].Cast<CheckBox>().CurrentValue && ignt.IsReady() &&
                    target.IsValidTarget(ignt.Range) &&
                    (IgniteDMG > (target.TotalShieldHealth() + HP5)))
                {
                    ignt.Cast(target);
                }
            }
        }

        private static void Zhonyas()
        {
            if (Player.Instance.HasItem(ItemId.Zhonyas_Hourglass) &&
                ActivatorMenu["Zhonyas"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.Instance.CountEnemyChampionsInRange(Q.MinimumRange) >= 2 &&
                    Player.Instance.CountAllyChampionsInRange(Q.MinimumRange) >= 2 && Player.Instance.HealthPercent < 10)
                {
                    Spells.Zhonyas.Cast();
                }
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Orbwalker.DisableMovement = IsCastingUlt;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ComboExecute();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) JungleExecute();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneExecute();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) FleeExecute();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) HarassExecute();

        }

        private static
            void OnGapcloser
            (Obj_AI_Base sender, EventArgs args)
        {
            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (sender.IsEnemy && sender is AIHeroClient && sender.Distance(Player.Instance) < E.Range &&
                E.IsReady() && MiscMenu["GE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(targetE);
            }
        }

        private static
            void OnInterrupt
            (Obj_AI_Base heroBase, EventArgs args)
        {
            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (heroBase.IsEnemy && heroBase is AIHeroClient && heroBase.Distance(Player.Instance) < E.Range &&
                E.IsReady() && MiscMenu["IE"].Cast<CheckBox>().CurrentValue)
            {
                E.Cast(targetE);
            }
        }
    }
}
