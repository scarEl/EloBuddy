using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using PortedPrediction.All.Prediction;
using SharpDX;
using Color = System.Drawing;
using System.Drawing;

//using OneKeyToWin_AIO_Sebby.Core;

namespace PortedPrediction.All.PCommon
{
    public static class PCommon
    {
        private static Obj_AI_Base Player { get { return ObjectManager.Player; } }

        private static int LastAATick = Core.GameTickCount;
        public static bool YasuoInGame = false;
        public static bool Thunderlord = false;

        public static bool
            blockMove = false,
            blockAttack = false,
            blockSpells = false;

        private static List<UnitIncomingDamage> IncomingDamageList = new List<UnitIncomingDamage>();
        private static List<Obj_AI_Base> ChampionList = new List<Obj_AI_Base>();
        private static YasuoWall yasuoWall = new YasuoWall();

        #region PathRelated

        internal static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();

            if (unit.IsVisible)
            {
                result.Add(unit.ServerPosition.To2D());
                var path = unit.Path;
                if (path.Length > 0)
                {
                    var first = path[0].To2D();
                    if (first.Distance(result[0], true) > 40)
                    {
                        result.Add(first);
                    }

                    for (int i = 1; i < path.Length; i++)
                    {
                        result.Add(path[i].To2D());
                    }
                }
            }
            //else if (WaypointTracker.StoredPaths.ContainsKey(unit.NetworkId))
            //{
            //    var path = WaypointTracker.StoredPaths[unit.NetworkId];
            //    var timePassed = (Utils.TickCount - WaypointTracker.StoredTick[unit.NetworkId]) / 1000f;
            //    if (path.PathLength() >= unit.MoveSpeed * timePassed)
            //    {
            //        result = CutPath(path, (int)(unit.MoveSpeed * timePassed));
            //    }
            //}

            return result;
        }

        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            var Distance = distance;
            if (distance < 0)
            {
                path[0] = path[0] + distance * (path[1] - path[0]).Normalized();
                return path;
            }

            for (var i = 0; i < path.Count - 1; i++)
            {
                var dist = path[i].Distance(path[i + 1]);
                if (dist > Distance)
                {
                    result.Add(path[i] + Distance * (path[i + 1] - path[i]).Normalized());
                    for (var j = i + 1; j < path.Count; j++)
                    {
                        result.Add(path[j]);
                    }

                    break;
                }
                Distance -= dist;
            }
            return result.Count > 0 ? result : new List<Vector2> { path.Last() };
        }

        #endregion PathRelated
        static PCommon()
        {
            foreach (var hero in ObjectManager.Get<Obj_AI_Base>())
            {
                ChampionList.Add(hero);
                if (hero.IsEnemy && EloBuddy.Player.Instance.ChampionName == "Yasuo")
                    YasuoInGame = true;
            }
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            EloBuddy.Player.OnIssueOrder += Obj_AI_Base_OnIssueOrder;
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Obj_AI_Base.OnDamage += Obj_AI_Base_OnDamage;
            EloBuddy.Player.OnSpellCast += Obj_AI_Base_OnDoCast;
            Game.OnWndProc += Game_OnWndProc;
        }

        private static void Obj_AI_Base_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (sender is Obj_AI_Base)
            {
                float time = Game.Time - 2;
                IncomingDamageList.RemoveAll(damage => time < damage.Time || ((int)damage.Damage == (int)args.Damage && damage.TargetNetworkId == sender.NetworkId));
            }
        }

        public static void debug(string msg)
        {
            if (true)
            {
                Console.WriteLine(msg);
            }
            if (false)
            {
                Chat.Print(msg);
            }
        }
    

    public static bool CanHarras()
        {
            if ( !Player.IsUnderTurret() && Orbwalker.CanMove)
                return true;
            else
                return false;
        }
        public static bool ShouldWait()
        {
            var attackCalc = (int)(Player.AttackDelay * 1000);
            return
                Cache.Cache.GetMinions(Player.Position, 0, EntityManager.UnitTeam.Enemy).Any(
                    minion => HealthPrediction.HealthPrediction.LaneClearHealthPrediction(minion, attackCalc, 30) <= Player.GetAutoAttackDamage(minion));
        }


        public static float GetEchoLudenDamage(Obj_AI_Base target)
        {
            float totalDamage = 0;

            if (Player.HasBuff("itemmagicshankcharge"))
            {
                if (Player.GetBuff("itemmagicshankcharge").Count == 100)
                {
                    totalDamage += (float)EloBuddy.Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, ((100 /*+ 0.1*/) * EloBuddy.Player.Instance.FlatMagicDamageMod));
                }
            }
            return totalDamage;
        }
        public static bool IsSpellHeroCollision(Obj_AI_Base t, Spell.Skillshot QWER, int extraWith = 50)
        {
            foreach (var hero in EntityManager.Enemies.FindAll(hero => hero.IsValidTarget(QWER.Range + QWER.Width, true, QWER.RangeCheckSource) && t.NetworkId != hero.NetworkId))
            {
                var prediction = QWER.GetPrediction(hero);
                var powCalc = Math.Pow((QWER.Width + extraWith + hero.BoundingRadius), 2);
                                                            //QWER.From.To2D()
                if (prediction.UnitPosition.To2D().Distance(EloBuddy.Player.Instance.Position.To2D(), QWER.GetPrediction(t).CastPosition.To2D(), true, true) <= powCalc)
                {
                    return true;
                }                                               //QWER.From.To2D()
                else if (prediction.UnitPosition.To2D().Distance(EloBuddy.Player.Instance.Position.To2D(), t.ServerPosition.To2D(), true, true) <= powCalc)
                {
                    return true;
                }

            }
            return false;
        }

        public static bool CanHitSkillShot(Obj_AI_Base target, Vector3 Start, Vector3 End, SpellData SData)
        {
            if (target.IsValidTarget(float.MaxValue, false))
            {
                if (target.Position.Distance(Start) > 1600)
                    return false;

                var pred = Prediction.Prediction.GetPrediction(target, 0.25f).CastPosition;
                if (pred == null)
                    return false;

                if (SData.LineWidth > 0)
                {
                    var powCalc = Math.Pow(SData.LineWidth + target.BoundingRadius, 2);
                    if (pred.To2D().Distance(End.To2D(), Start.To2D(), true, true) <= powCalc || target.ServerPosition.To2D().Distance(End.To2D(), Start.To2D(), true, true) <= powCalc)
                    {
                        return true;
                    }
                }
                else if (target.Distance(End) < 50 + target.BoundingRadius || pred.Distance(End) < 50 + target.BoundingRadius)
                {
                    return true;
                }
            }
            return false;
        }

        public static float GetKsDamage(Obj_AI_Base t, Spell.SimpleSkillshot QWER, bool includeIncomingDamage = true)
        {
            var totalDmg = QWER.GetSpellDamage(t) - t.AllShield;
            totalDmg += GetEchoLudenDamage(t);
            totalDmg -= t.HPRegenRate;

            if (totalDmg > t.Health)
            {
                if (Player.HasBuff("summonerexhaust"))
                    totalDmg = totalDmg * 0.6f;

                if (t.HasBuff("ferocioushowl"))
                    totalDmg = totalDmg * 0.7f;

                if (t.BaseSkinName == "Blitzcrank" && !t.HasBuff("BlitzcrankManaBarrierCD") && !t.HasBuff("ManaBarrier"))
                {
                    totalDmg -= t.Mana / 2f;
                }
            }
            //if (Thunderlord && !Player.HasBuff( "masterylordsdecreecooldown"))
            //totalDmg += (float)Player.CalcDamage(t, Damage.DamageType.Magical, 10 * Player.Level + 0.1 * Player.FlatMagicDamageMod + 0.3 * Player.FlatPhysicalDamageMod);
            if (includeIncomingDamage)
                totalDmg += (float)GetIncomingDamage2(t);
            return totalDmg;
        }

        public static bool ValidUlt(Obj_AI_Base target)
        {
            if (target.HasBuffOfType(BuffType.PhysicalImmunity) || target.HasBuffOfType(BuffType.SpellImmunity)
                || target.IsZombie || target.IsInvulnerable || target.HasBuffOfType(BuffType.Invulnerability) || target.HasBuff("kindredrnodeathbuff")
                || target.HasBuffOfType(BuffType.SpellShield) || target.Health - GetIncomingDamage2(target) < 1)
                return false;
            else
                return true;
        }

        public static bool CanMove(Obj_AI_Base target)
        {
            if (target.MoveSpeed < 50 || target.IsStunned || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Fear) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Knockup) || target.HasBuff("Recall") ||
                target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Suppression))
            {
                return false;
            }
            else
                return true;
        }

        public static int GetBuffCount(Obj_AI_Base target, string buffName)
        {
            foreach (var buff in target.Buffs.Where(buff => buff.Name.ToLower() == buffName.ToLower()))
            {
                if (buff.Count == 0)
                    return 1;
                else
                    return buff.Count;
            }
            return 0;
        }

        public static int CountEnemyMinions(Obj_AI_Base target, float range, EntityManager.UnitTeam team)
        {
            var allMinions = Cache.Cache.GetMinions(target.Position, range, team);
            if (allMinions != null)
                return allMinions.Count;
            else
                return 0;
        }

        public static float GetPassiveTime(Obj_AI_Base target, string buffName)
        {
            return
                target.Buffs.OrderByDescending(buff => buff.EndTime - Game.Time)
                    .Where(buff => buff.Name.ToLower() == buffName.ToLower())
                    .Select(buff => buff.EndTime)
                    .FirstOrDefault() - Game.Time;
        }

        public static Vector3 GetTrapPos(float range)
        {
            foreach (var enemy in EntityManager.Enemies.Where(enemy => enemy.IsValid && enemy.Distance(Player.Position) < range && (enemy.HasBuff("BardRStasis") || enemy.HasBuffOfType(BuffType.Invulnerability))))
            {
                return enemy.Position;
            }

            foreach (var obj in ObjectManager.Get<Obj_GeneralParticleEmitter>().Where(obj => obj.IsValid && obj.Position.Distance(Player.Position) < range))
            {
                var name = obj.Name.ToLower();

                if (name.Contains("gatemarker_red.troy") || name.Contains("global_ss_teleport_target_red.troy") || (name.Contains("lifeaura") && EntityManager.Enemies.Any(x => x.IsValid && x.Position.Distance(obj.Position) < 200))
                    || name.Contains("r_indicator_red.troy"))
                    return obj.Position;
            }

            return Vector3.Zero;
        }

        public static bool IsMovingInSameDirection(Obj_AI_Base source, Obj_AI_Base target)
        {
            var sourceLW = source.GetWaypoints().Last().To3D();

            if (sourceLW == source.Position || !source.IsMoving)
                return false;

            var targetLW = target.GetWaypoints().Last().To3D();

            if (targetLW == target.Position || !target.IsMoving)
                return false;

            Vector2 pos1 = sourceLW.To2D() - source.Position.To2D();
            Vector2 pos2 = targetLW.To2D() - target.Position.To2D();
            var getAngle = pos1.AngleBetween(pos2);

            if (getAngle < 20)
                return true;
            else
                return false;
        }

        public static bool CollisionYasuo(Vector3 from, Vector3 to)
        {
            if (!YasuoInGame)
                return false;

            if (Game.Time - yasuoWall.CastTime > 4)
                return false;

            var level = yasuoWall.WallLvl;
            var wallWidth = (350 + 50 * level);
            var wallDirection = (yasuoWall.CastPosition.To2D() - yasuoWall.YasuoPosition.To2D()).Normalized().Perpendicular();
            var wallStart = yasuoWall.CastPosition.To2D() + wallWidth / 2f * wallDirection;
            var wallEnd = wallStart - wallWidth * wallDirection;

            if (wallStart.Intersection(wallEnd, to.To2D(), from.To2D()).Intersects)
            {
                return true;
            }
            return false;

        }

        public static void DrawTriangleOKTW(float radius, Vector3 position, System.Drawing.Color color, float bold = 1)
        {
            var positionV2 = Drawing.WorldToScreen(position);
            Vector2 a = new Vector2(positionV2.X + radius, positionV2.Y + radius / 2);
            Vector2 b = new Vector2(positionV2.X - radius, positionV2.Y + radius / 2);
            Vector2 c = new Vector2(positionV2.X, positionV2.Y - radius);
            Drawing.DrawLine(a[0], a[1], b[0], b[1], bold, color);
            Drawing.DrawLine(b[0], b[1], c[0], c[1], bold, color);
            Drawing.DrawLine(c[0], c[1], a[0], a[1], bold, color);
        }

        public static void DrawLineRectangle(Vector3 start2, Vector3 end2, int radius, float width, System.Drawing.Color color)
        {
            Vector2 start = start2.To2D();
            Vector2 end = end2.To2D();
            var dir = (end - start).Normalized();
            var pDir = dir.Perpendicular();

            var rightStartPos = start + pDir * radius;
            var leftStartPos = start - pDir * radius;
            var rightEndPos = end + pDir * radius;
            var leftEndPos = end - pDir * radius;

            var rStartPos = Drawing.WorldToScreen(new Vector3(rightStartPos.X, rightStartPos.Y, ObjectManager.Player.Position.Z));
            var lStartPos = Drawing.WorldToScreen(new Vector3(leftStartPos.X, leftStartPos.Y, ObjectManager.Player.Position.Z));
            var rEndPos = Drawing.WorldToScreen(new Vector3(rightEndPos.X, rightEndPos.Y, ObjectManager.Player.Position.Z));
            var lEndPos = Drawing.WorldToScreen(new Vector3(leftEndPos.X, leftEndPos.Y, ObjectManager.Player.Position.Z));

            Drawing.DrawLine(rStartPos, rEndPos, width, color);
            Drawing.DrawLine(lStartPos, lEndPos, width, color);
            Drawing.DrawLine(rStartPos, lStartPos, width, color);
            Drawing.DrawLine(lEndPos, rEndPos, width, color);
        }

        public static List<Vector3> CirclePoints(float CircleLineSegmentN, float radius, Vector3 position)
        {
            List<Vector3> points = new List<Vector3>();
            for (var i = 1; i <= CircleLineSegmentN; i++)
            {
                var angle = i * 2 * Math.PI / CircleLineSegmentN;
                var point = new Vector3(position.X + radius * (float)Math.Cos(angle), position.Y + radius * (float)Math.Sin(angle), position.Z);
                points.Add(point);
            }
            return points;
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == 123 && blockMove)
            {
                blockMove = false;
                blockAttack = false;
                /*Orbwalking.Attack = true;
                Orbwalking.Move = true;*/
                EloBuddy.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }

        //BUG
        /*public static double GetIncomingDamage(Obj_AI_Base target)
        {
            return OKTWtracker.ChampionInfoList.FirstOrDefault(x => x.Hero.NetworkId == target.NetworkId).IncomingDamage;
        }*/

        public static double GetIncomingDamage2(Obj_AI_Base target, float time = 0.5f, bool skillshots = true)
        {
            double totalDamage = 0;

            foreach (var damage in IncomingDamageList.Where(damage => damage.TargetNetworkId == target.NetworkId && Game.Time - time < damage.Time))
            {
                if (skillshots)
                {
                    totalDamage += damage.Damage;
                }
                else
                {
                    if (!damage.Skillshot)
                        totalDamage += damage.Damage;
                }
            }
            double damage2 = 0;

            foreach (var missile in Cache.Cache.MissileList.Where(missile => missile.IsValid && missile.SpellCaster != null && missile.SData != null && missile.SpellCaster.Team != target.Team))
            {
                if (missile.Target != null)
                {
                    if (missile.Target.NetworkId == target.NetworkId)
                    {
                        var damageExtra = missile.SData.SpellDamageRatio;
                        if (damageExtra == 0)
                            damageExtra += target._FlatHPModPerLevel * 3;
                        damage2 = damageExtra;
                    }
                }
                //BUG
                /*else if (skillshots)
                {
                    if (CanHitSkillShot(target, missile.StartPosition, missile.EndPosition, missile.SData))
                    {
                        if (target.HasBuffOfType(BuffType.Slow) || !CanMove(target))
                        {
                            damage2 += missile.GetSpellDamage((Obj_AI_Base)missile.Target, missile.SData.Name);
                        }
                    }
                }*/
            }

            if (damage2 > totalDamage)
                totalDamage = damage2;

            if (target.HasBuffOfType(BuffType.Poison))
                totalDamage += target._FlatHPModPerLevel * 5;
            if (target.HasBuffOfType(BuffType.Damage))
                totalDamage += target._FlatHPModPerLevel * 6;

            return totalDamage;
        }


        private static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Target != null && args.SData != null)
            {
                if (args.Target.Type == GameObjectType.AIHeroClient && !sender.IsMelee && args.Target.Team != sender.Team)
                {
                    IncomingDamageList.Add(new UnitIncomingDamage { Damage = EloBuddy.Player.Instance.GetSpellDamage((Obj_AI_Base)args.Target, args.Slot), TargetNetworkId = args.Target.NetworkId, Time = Game.Time, Skillshot = false });
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData == null)
            {
                return;
            }
            /////////////////  HP prediction
            var targed = args.Target as Obj_AI_Base;

            if (targed != null)
            {
                if (targed.Type == GameObjectType.AIHeroClient && targed.Team != sender.Team && (sender.IsMelee || !args.SData.IsAutoAttack()))
                {
                    IncomingDamageList.Add(new UnitIncomingDamage { Damage = EloBuddy.Player.Instance.GetSpellDamage(targed,args.Slot), TargetNetworkId = args.Target.NetworkId, Time = Game.Time, Skillshot = false });
                }
            }
            else
            {
                foreach (var champion in ChampionList.Where(champion => !champion.IsDead && champion.IsVisible && champion.Team != sender.Team && champion.Distance(sender) < 2000))
                {
                    if (champion.HasBuffOfType(BuffType.Slow)/* || champion.IsWindingUp*/ || !CanMove(champion))
                    {
                        if (CanHitSkillShot(champion, args.Start, args.End, args.SData))
                        {
                            IncomingDamageList.Add(new UnitIncomingDamage { Damage = EloBuddy.Player.Instance.GetSpellDamage(champion, args.Slot), TargetNetworkId = champion.NetworkId, Time = Game.Time, Skillshot = true });
                        }
                    }
                }

                if (!YasuoInGame)
                    return;

                if (!sender.IsEnemy || sender.IsMinion || args.SData.IsAutoAttack() || sender.Type != GameObjectType.AIHeroClient)
                    return;

                if (args.SData.Name.Contains("YasuoWMovingWall"))
                {
                    yasuoWall.CastTime = Game.Time;
                    //BUG
                    ///yasuoWall.CastPosition = sender.Position.Extend(args.End,400);
                    yasuoWall.YasuoPosition = sender.Position;
                    yasuoWall.WallLvl = sender.Spellbook.Spells[1].Level;
                }
            }
        }

        private static void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (blockSpells)
            {
                args.Process = false;
            }
        }

        private static void Obj_AI_Base_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (!sender.IsMe)
                return;

            if (blockMove && args.Order != GameObjectOrder.AttackUnit)
            {
                args.Process = false;
            }
            if (blockAttack && args.Order == GameObjectOrder.AttackUnit)
            {
                args.Process = false;
            }
        }

    }

    class UnitIncomingDamage
    {
        public int TargetNetworkId { get; set; }
        public float Time { get; set; }
        public double Damage { get; set; }
        public bool Skillshot { get; set; }
    }

    class YasuoWall
    {
        public Vector3 YasuoPosition { get; set; }
        public float CastTime { get; set; }
        public Vector3 CastPosition { get; set; }
        public float WallLvl { get; set; }

        public YasuoWall()
        {
            CastTime = 0;
        }
    }
}