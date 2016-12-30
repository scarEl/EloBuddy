using System;
using System.Linq;
using System.Net;
using SharpDX;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace AFK_RIFT_by_DNKappa
{
    internal static class Program
    {
        private static int _LastOnUpdateTick;

        private static Vector3 PlayerPosition
        {
            get
            {
                switch (Game.MapId)
                {
                    //case GameMapId.CrystalScar:
                    //    break;
                    //case GameMapId.TwistedTreeline:
                    //    break;
                    case GameMapId.SummonersRift:
                        switch (ObjectManager.Player.Team)
                        {
                            //case GameObjectTeam.Unknown:
                            //    break;
                            case GameObjectTeam.Order:
                                return new Vector3(8572, 3258, 54.43658f);
                            case GameObjectTeam.Chaos:
                                return new Vector3(6424, 11656, 54.83006f);
                            //case GameObjectTeam.Neutral:
                            //    break;
                            //default:
                            //    break;
                        }
                        break;
                    //case GameMapId.HowlingAbyss:
                    //    break;
                    //default:
                    //    break;
                }

                return Vector3.Zero;
            }
        }

        private static Vector3 WardPosition
        {
            get
            {
                switch (Game.MapId)
                {
                    //case GameMapId.CrystalScar:
                    //    break;
                    //case GameMapId.TwistedTreeline:
                    //    break;
                    case GameMapId.SummonersRift:
                        switch (ObjectManager.Player.Team)
                        {
                            //case GameObjectTeam.Unknown:
                            //    break;
                            case GameObjectTeam.Order:
                                return new Vector3(8476, 2908, 51.13f);
                            case GameObjectTeam.Chaos:
                                return new Vector3(6412, 11998, 56.4768f);
                            //case GameObjectTeam.Neutral:
                            //    break;
                            //default:
                            //    break;
                        }
                        break;
                    //case GameMapId.HowlingAbyss:
                    //    break;
                    //default:
                    //    break;
                }

                return Vector3.Zero;
            }
        }

        private static void Main(string[] args)
        {
            Hacks.AntiAFK = true;
            //Hacks.GameObjectCache = true;
            Game.OnLoad += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
            if (Game.MapId != GameMapId.SummonersRift)
            {
                Chat.Print(
                    "<font color = \"#00D8FF\"><b>AFK Bot for summoners rift:</b></font> You have to be in Summoner's rift to use this assembly.");
                return;
            }

            if (!ObjectManager.Player.IsRanged)
            {
                Chat.Print(
                    "<font color = \"#00D8FF\"><b>AFK Bot for summoners rift:</b></font> You have to be a ranged champion to use this assembly.");
                return;
            }

            Game.OnEnd += Game_OnEnd;
            Game.OnUpdate += Game_OnUpdate;

            Chat.Print("<font color = \"#00D8FF\"><b>AFK Bot for summoners rift:</b></font> Loaded.");
            Chat.Print("<font color = \"#00D8FF\"><b>All Credits to:</b></font> Definitely not Kappa.");
        }

        private static void Game_OnEnd(GameEndEventArgs args)
        {
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                Game.QuitGame();
            });
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var nowTick = Environment.TickCount;
            if (nowTick - _LastOnUpdateTick < 333)
            {
                return;
            }

            _LastOnUpdateTick = Environment.TickCount;

            if (ObjectManager.Player.IsDead)
            {
                return;
            }

            if (PlayerPosition == Vector3.Zero ||
                WardPosition == Vector3.Zero)
            {
                return;
            }

            if (ObjectManager.Player.IsInShopRange())
            {
                //ObjectManager.Player.BuyItem(ItemId.Vision_Ward);
                Shop.BuyItem(ItemId.Control_Ward);
            }

            if (ObjectManager.Player.Distance(PlayerPosition) > 50)
            {
                if (ObjectManager.Player.CanMove)
                {
                    //ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, PlayerPosition);
                    Player.IssueOrder(GameObjectOrder.MoveTo, PlayerPosition);
                }
            }
            else
            {
                var target = ObjectManager.Get<Obj_AI_Base>()
                            .FirstOrDefault(x => !x.IsDead && !x.IsAlly && Extensions.InAutoAttackRange(x));
                //if (FogOfWar.InFog(WardPosition))
                if (Player.Instance.HasItem(ItemId.Control_Ward))
                {
                    Extensions.UseItem((int) ItemId.Control_Ward, WardPosition);
                }
                if (target != null)
                {

                        if (ObjectManager.Player.CanAttack)
                        {
                            Player.IssueOrder(GameObjectOrder.AttackUnit, target,true);
                        }
                    }
                }
            }
        }
    }
