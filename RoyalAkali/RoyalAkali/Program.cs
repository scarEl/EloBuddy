using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Spells;
using SharpDX;
using Color = System.Drawing;

namespace RoyalAkali
{
    //TODO
    /*
     * Better last hit(e prediction)
     * Use W if < % HP and № enemies around
     * Use W for bush vision(configure ward\W)
     * Dont dive with ulti under towers unless you can kill enemy with R so you could get out with the stack you gain
    */
    class Program
    {
        public static Menu FirstMenu;
        public static Menu Combo;
        public static Menu Harass;
        public static Menu Clear;
        public static Menu Jungle;
        public static Menu Misc;
        public static Menu Drawings;

        private static readonly Obj_AI_Base player = ObjectManager.Player;
        public static Spell.Targeted Q;
        public static Spell.Active E, W;
        public static Spell.Targeted R;
        public static Spell.Targeted Ignite;
        private static Obj_AI_Base rektmate = default(Obj_AI_Base);
        private static SpellSlot IgniteSlot = EloBuddy.Player.Instance.GetSpellSlotFromName("SummonerDot");
        private static float assignTime = 0f;
        private static JumpUnit first_unit = new JumpUnit(player.Position, player), second_unit = first_unit;
        private static bool gotPath = false;
        static readonly string localVersion = "1.04";

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnGameLoad;
        }

        static void OnGameLoad(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Akali") return;
            UpdateChecks();
            LoadMenu();
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 700);
            E = new Spell.Active(SpellSlot.E, 290);
            R = new Spell.Targeted(SpellSlot.R, 790);

            Drawing.OnDraw += onDraw;
            Game.OnUpdate += onUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnCast;

            if (IgniteSlot != SpellSlot.Unknown)
            {
                 Ignite = new Spell.Targeted(IgniteSlot, 600);
            }

            Chat.Print("RoyalAkali by Ouija Loaded.");
            Console.WriteLine("\a \a \a");
        }
        private static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy) return;
            if (args.SData.Name == "TrinketTotemLvl3B" || args.SData.Name == "VisionWard" )
            {
                if (args.End.Distance(player.Position) < 1200)
                    //Utility.DelayAction.Add(200, () => AntiPink(args.End));
                    Core.DelayAction(delegate
                    {
                        AntiPink(args.End);
                    }, 200);
            }
        }
        private static void LoadMenu()
        {
            FirstMenu = MainMenu.AddMenu("Royal" + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower());
            Combo = FirstMenu.AddSubMenu("Combo Settings");
            Harass = FirstMenu.AddSubMenu("Harass Settings");
            Clear = FirstMenu.AddSubMenu("Lane Settings");
            Jungle = FirstMenu.AddSubMenu("Jungle Settings");
            Misc = FirstMenu.AddSubMenu("Misc Settings");
            Drawings = FirstMenu.AddSubMenu("Drawings Settings");

            Combo.AddGroupLabel("Combo Options");
            Combo.Add("useQ", new CheckBox("Use Q in Combo"));
            Combo.Add("useW", new CheckBox("Use W in Combo"));
            Combo.Add("useE", new CheckBox("Use E in Combo"));
            Combo.Add("useR", new CheckBox("Use R in Combo"));
            
            Harass.AddGroupLabel("Harass Options");
            Harass.Add("useQ", new CheckBox("Use Q in Harass", true));
            Harass.Add("useE", new CheckBox("Use E in Harass", true));

            Clear.AddGroupLabel("Lane Clear Options");
            Clear.Add("useQ", new CheckBox("Use Q in Laneclear", true));
            Clear.Add("useE", new CheckBox("Use E in Laneclear", true));
            Clear.Add("hitCounter", new Slider("Use E if will hit {0} minions", 3, 1, 6));

            Jungle.AddGroupLabel("Jungle Clear Options");
            Jungle.Add("useQ", new CheckBox("Use Q in Laneclear", true));
            Jungle.Add("useE", new CheckBox("Use E in Laneclear", true));

            Misc.AddGroupLabel("Miscellaneous");
            Misc.Add("escape", new KeyBind("Escape Key", false, KeyBind.BindTypes.HoldActive, 'G'));
            Misc.Add("RCounter", new Slider("Do not escape if R<", 1, 1, 3));
            Misc.Add("TowerDive", new Slider("Do not tower dive if your HP is under {0}", 25, 1, 100));
            Misc.Add("Enemies", new Slider("Do not rape if there is {0} enemies around target", 0, 0, 5));
            Misc.Add("RKillsteal", new CheckBox("Always try to KS with R"));
            Misc.Add("PanicW", new Slider("If {0} enemies around, cast W", 1, 1, 5));
            Misc.Add("PanicWN", new Slider("If your HP is under {0}%", 25, 0, 100));

            Drawings.AddGroupLabel("Drawings");
            Drawings.Add("Qrange", new CheckBox("Q Range"));
            Drawings.Add("Wrange", new CheckBox("W Range"));
            Drawings.Add("Erange", new CheckBox("E Range"));
            Drawings.Add("Rrange", new CheckBox("R Range"));
            Drawings.Add("RAPE", new CheckBox("Draw instakill target"));

            /*var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after a rotation").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit += hero => (float)IsRapeble(hero);
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate (object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };*/
        }

        private static void UpdateChecks()
        {
            Chat.Print("--------------------------------------------------------------------------------");
            WebClient client = new WebClient();
            string version = client.DownloadString("https://raw.githubusercontent.com/Ouija01/Elobuddy/master/RoyalAkali/Version");
            if (version.Remove(4).Equals(localVersion))
                Chat.Print("== Your copy of Royal Rapist Akali is updated! GL & HF! ==");
            else
                Chat.Print("== Royal Akali got an update. Reload the Addon in your Loader and Press F5! ==");
        }

        private static void onUpdate(EventArgs args)
        {
            if (Misc["RKillsteal"].Cast<CheckBox>().CurrentValue)
                foreach (Obj_AI_Base enemy in ObjectManager.Get<Obj_AI_Base>())
                    if (enemy.IsEnemy && Vector3.Distance(player.Position, enemy.Position) <= R.Range && EloBuddy.Player.Instance.GetSpellDamage(enemy, SpellSlot.R) > enemy.Health && ultiCount() > 0 && R.IsReady())
                        R.Cast(enemy);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                RapeTime();
            };

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Harass["useQ"].Cast<CheckBox>().CurrentValue)
                    castQ(true);
                if (Harass["useE"].Cast<CheckBox>().CurrentValue)
                    castE(true);
            };

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Clear["useQ"].Cast<CheckBox>().CurrentValue)
                    castQ(false);
                if (Clear["useE"].Cast<CheckBox>().CurrentValue)
                    castE(false);
            };

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (Jungle["useQ"].Cast<CheckBox>().CurrentValue)
                    castQ(false);
                if (Jungle["useE"].Cast<CheckBox>().CurrentValue)
                    castE(false);
            };

            if (Misc["escape"].Cast<KeyBind>().CurrentValue) Escape();

            /*switch (Orbwalker.ActiveModesFlags)
            { 
                case Orbwalker.ActiveModes.Combo:
                    RapeTime();
                    break;

                case Orbwalker.ActiveModes.Harass:
                    if (Harass["useQ"].Cast<CheckBox>().CurrentValue)
                        castQ(true);
                    if (Harass["useE"].Cast<CheckBox>().CurrentValue)
                        castE(true);
                    break;

                case Orbwalker.ActiveModes.LaneClear:
                    if (Clear["useQ"].Cast<CheckBox>().CurrentValue)
                        castQ(false);
                    if (Clear["useE"].Cast<CheckBox>().CurrentValue)
                        castE(false);
                    break;
            }*/
        }

        private static void onDraw(EventArgs args)
        {
            if (Misc["escape"].Cast<KeyBind>().CurrentValue)
            {
                Drawing.DrawCircle(Game.CursorPos, 200, W.IsReady() ? Color.Color.Blue : Color.Color.Red);
                Drawing.DrawCircle(Player.Instance.Position, R.Range, R.IsReady() ? Color.Color.Blue : Color.Color.Red);
            }

            #region SpellDrawings

            if (Drawings["Qrange"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Instance.Position, Q.Range, Color.Color.Gold);
            }

            if (Drawings["Wrange"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(player.Position, W.Range, Color.Color.Purple);
            }

            if (Drawings["Erange"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Instance.Position, E.Range, Color.Color.GhostWhite);
            }

            if (Drawings["Rrange"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(Player.Instance.Position, R.Range, Color.Color.Aquamarine);
            }


            if (Drawings["RAPE"].Cast<CheckBox>().CurrentValue && rektmate != default(Obj_AI_Base))
                Drawing.DrawCircle(rektmate.Position, 50, Color.Color.ForestGreen);
            if (rektmate == default(Obj_AI_Base)) return;
            Drawing.DrawLine(Drawing.WorldToScreen(player.Position), Drawing.WorldToScreen(first_unit.Position), 3, Color.Color.OrangeRed);
            if (first_unit.unit != rektmate)
                Drawing.DrawLine(Drawing.WorldToScreen(first_unit.Position), Drawing.WorldToScreen(second_unit.Position), 3, Color.Color.LightGoldenrodYellow);
            if (second_unit.unit != rektmate)
                Drawing.DrawLine(Drawing.WorldToScreen(second_unit.Position), Drawing.WorldToScreen(rektmate.Position), 3, Color.Color.DarkGreen);

            /*
            Drawing.DrawText(Drawing.WorldToScreen(debugTarget).X, Drawing.WorldToScreen(debugTarget).Y, Color.Color.PowderBlue, debugTargetDist.ToString());
            Drawing.DrawText(Drawing.WorldToScreen(debugJump).X, Drawing.WorldToScreen(debugJump).Y, Color.Color.PowderBlue, debugJumpDist.ToString());
            */

            #endregion
        }

        private static void AntiPink(Vector3 position2)
        {
            float pd = player.Distance(position2);
            foreach (var item in player.InventoryItems)
                switch (item.Name)
                {
                    case "TrinketSweeperLvl1":
                        if (pd < 800)
                            item.Cast(V2E(player.Position, position2, 400).To3D());
                        break;
                    case "TrinketSweeperLvl2":
                        if (pd < 1200)
                            item.Cast(V2E(player.Position, position2, 600).To3D());
                        break;
                    case "TrinketSweeperLvl3":
                        if (pd < 1200)
                            item.Cast(V2E(player.Position, position2, 600).To3D());
                        break;
                }
        }

        private static void castQ(bool mode)
        {
            if (!Q.IsReady()) return;
            if (mode)
            {
                Obj_AI_Base target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                if (!target.IsValidTarget(Q.Range)) return;
                Q.Cast(target);
            }
            else
            {
                foreach (
                    Obj_AI_Base minion in
                        EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,player.Position, Q.Range))
                    if (hasBuff(minion, "AkaliMota") &&
                        ObjectManager.Player.GetAutoAttackDamage(player) >= Vector3.Distance(player.Position, minion.Position))
                        Orbwalker.ForcedTarget = minion;

                foreach (
                    Obj_AI_Base minion in
                        EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position, Q.Range))
                    if (Prediction.Health.GetPrediction(minion,
                            (int)((Vector3.Distance(player.Position, minion.Position) / E.CastDelay)) * 1000) < 
                        Player.Instance.GetSpellDamage(minion, SpellSlot.Q) &&
                        Prediction.Health.GetPrediction(minion,
                            (int)((Vector3.Distance(player.Position, minion.Position) / E.CastDelay)) * 1000) > 0 &&
                        Vector3.Distance(player.Position, minion.Position) > ObjectManager.Player.GetAutoAttackDamage(player))
                        Q.Cast(minion);

                foreach (Obj_AI_Base minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,player.ServerPosition, Q.Range))
                    if (Vector3.Distance(player.Position, minion.Position) <= Q.Range)
                        Q.Cast(minion);


            }
        }

        private static void castE(bool mode)
        {
            if (!E.IsReady()) return;
            if (mode)
            {
                Obj_AI_Base target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                if (target == null || !target.IsValidTarget(E.Range)) return;
                if (hasBuff(target, "AkaliMota") && !E.IsReady() &&
                    ObjectManager.Player.GetAutoAttackDamage(player) >= player.Distance(target))
                    Orbwalker.ForcedTarget = target;
                else
                    E.Cast(target);
            }
            else
            {   //Minions in E range                                                                            >= Value in menu
                if (Player.Instance.CountEnemyMinionsInRange(E.Range) >= Clear["hitCounter"].Cast<Slider>().CurrentValue)
                {
                    foreach (Obj_AI_Base minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,player.ServerPosition, Q.Range))
                        if (Vector3.Distance(player.Position, minion.Position) <= E.Range)
                            E.Cast();
                }
            }
        }
        //-------------------======================================================================================-----------------
        private static void RapeTime()
        {
            Obj_AI_Base possibleVictim = TargetSelector.GetTarget(R.Range * 2 + ObjectManager.Player.GetAutoAttackDamage(player), DamageType.Magical);
            try
            {
                if (rektmate.IsDead || Game.Time - assignTime > 1.5)
                {
                    //Console.WriteLine("Unassign - " + rektmate.ChampionName + " dead: " + rektmate.IsDead + "\n\n");
                    gotPath = false;
                    rektmate = default(Obj_AI_Base);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            try
            {
                if (rektmate == default(Obj_AI_Base) && IsRapeble(possibleVictim) > possibleVictim.Health)
                {
                    rektmate = possibleVictim;
                    assignTime = Game.Time;
                    gotPath = gapclosePath(possibleVictim);
                    //Console.WriteLine("Assign - " + rektmate.ChampionName + " time: " + assignTime+"\n\n");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            if (rektmate != default(Obj_AI_Base))
            {
                //!(menu.SubMenu("misc").Item("TowerDive").GetValue<Slider>().Value < player.Health/player.MaxHealth && Utility.UnderTurret(rektmate, true)) && 
                if (player.Distance(rektmate) < R.Range * 2 + ObjectManager.Player.GetAutoAttackRange(player) && player.Distance(rektmate) > Q.Range)
                    CastR(rektmate.Position);
                else if (player.Distance(rektmate) < Q.Range)
                    RaperinoCasterino(rektmate);
                else rektmate = default(Obj_AI_Base);//Target is out of range. Unassign.
            }
            else
            {
                if (Combo["useQ"].Cast<CheckBox>().CurrentValue)
                    castQ(true);
                if (Combo["useE"].Cast<CheckBox>().CurrentValue)
                    castE(true);
                if (Combo["useW"].Cast<CheckBox>().CurrentValue)
                    castW();
                if (Combo["useR"].Cast<CheckBox>().CurrentValue)
                {
                    Obj_AI_Base target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                    if (target.IsValidTarget(R.Range) && target.Distance(player) > ObjectManager.Player.GetAutoAttackDamage(player)) //|| R.isKillable(target))
                        R.Cast(target);
                }
            }
        }

        private static void castW()
        {
            //
            //menu.SubMenu("misc").AddItem(new MenuItem("PanicW", "In combo if № of enemies around").SetValue(new Slider(0, 0, 5)));
            //menu.SubMenu("misc").AddItem(new MenuItem("PanicWN", "In combo in %HP < ").SetValue(new Slider(25, 0, 100)));
            //
            byte enemiesAround = 0;
            foreach (Obj_AI_Base enemy in ObjectManager.Get<Obj_AI_Base>())
                if (enemy.IsEnemy && enemy.Distance(player) < 400) enemiesAround++;
            if (Misc["PanicW"].Cast<Slider>().CurrentValue > enemiesAround && Misc["PanicWN"].Cast<Slider>().CurrentValue < (int)(player.Health / player.MaxHealth * 100))
                return;
            W.Cast(player.Position);
        }


        private static void CastR(Vector3 position, bool mouseJump = false)
        {
            Obj_AI_Base target = player;
            foreach (Obj_AI_Base minion in ObjectManager.Get<Obj_AI_Base>())
                if (minion.IsValidTarget(R.Range, true) && player.Distance(position, true) > minion.Distance(position, true) && minion.Distance(position, true) < target.Distance(position, true))
                    if (mouseJump)
                    {
                        if (minion.Distance(position) < 200)
                            target = minion;
                    }
                    else
                        target = minion;
            if (R.IsReady() && R.IsInRange(target.Position) && !target.IsMe)
                if (mouseJump && target.Distance(position) < 200)
                    R.Cast(target);
                else if (player.Distance(position, true) > target.Distance(position, true))
                    R.Cast(target);
        }


        private static void RaperinoCasterino(Obj_AI_Base victim)
        {
            try
            {
                //Orbwalker.SetAttacks(!Q.IsReady() && !E.IsReady() && player.Distance(victim) < 800f);
                Orbwalker.ForcedTarget=victim;
                foreach (var item in player.InventoryItems)
                    switch ((int)item.Id)
                    {
                        case 3144:
                            if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready) item.Cast(victim);
                            break;
                        case 3146:
                            if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready) item.Cast(victim);
                            break;
                        case 3128:
                            if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready) item.Cast(victim);
                            break;
                    }
                if (Q.IsReady() && Q.IsInRange(victim)) Q.Cast(victim);
                if (E.IsReady() && E.IsInRange(victim)) E.Cast();
                if (W.IsReady() && Combo["useW"].Cast<CheckBox>().CurrentValue && W.IsInRange(victim.Position) &&
                    !(hasBuff(victim, "AkaliMota") &&
                      player.Distance(victim) > ObjectManager.Player.GetAutoAttackDamage(player)))
                    W.Cast(victim);//(V2E(player.Position, victim.Position, player.Distance(victim) + W.Width * 2 - 20));
                if (R.IsReady() && R.IsInRange(victim)) R.Cast(victim);
                if (IgniteSlot != SpellSlot.Unknown && EloBuddy.Player.Instance.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready) Player.Instance.Spellbook.CastSpell(IgniteSlot, victim);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static double IsRapeble(Obj_AI_Base victim)
        {
            double comboDamage = 0d;
            if (Q.IsReady()) comboDamage += Q.GetSpellDamage(victim) + Player.Instance.CalculateDamageOnUnit(victim,DamageType.Magical, (float)(45 + 35 * Q.Level + 0.5 * player.FlatMagicDamageMod));
            if (E.IsReady()) comboDamage += E.GetSpellDamage(victim);

            if (hasBuff(victim, "AkaliMota")) comboDamage += Player.Instance.CalculateDamageOnUnit(victim, DamageType.Magical, (float)(45 + 35 * Q.Level + 0.5 * player.FlatMagicDamageMod));
            //comboDamage += player.GetAutoAttackDamage(victim, true);

            comboDamage += player.CalculateDamageOnUnit(victim, DamageType.Magical, (float)CalcPassiveDmg());
            comboDamage += player.CalculateDamageOnUnit(victim, DamageType.Magical, (float)CalcItemsDmg(victim));

            foreach (var item in player.InventoryItems)
                if ((int)item.Id == 3128)
                    if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready)
                        comboDamage *= 1.2;
            if (hasBuff(victim, "deathfiregraspspell")) comboDamage *= 1.2;

            if (ultiCount() > 0) comboDamage += Player.Instance.GetSpellDamage(victim,SpellSlot.Unknown) * (ultiCount() - (int)(victim.Distance(player.Position) / R.Range));
            if (IgniteSlot != SpellSlot.Unknown && player.Spellbook.CanUseSpell(IgniteSlot) == SpellState.Ready)
                comboDamage += ObjectManager.Player.GetSummonerSpellDamage(victim, DamageLibrary.SummonerSpells.Ignite);
            return comboDamage;
        }

        private static double CalcPassiveDmg()
        {
            return (0.06 + 0.01 * (player.FlatMagicDamageMod / 6)) * (player.FlatPhysicalDamageMod + player.BaseAttackDamage);
        }

        private static double CalcItemsDmg(Obj_AI_Base victim)
        {
            double result = 0d;
            foreach (var item in player.InventoryItems)
                switch ((int)item.Id)
                {
                    case 3100: // LichBane
                        if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready)
                            result += player.BaseAttackDamage * 0.75 + player.FlatMagicDamageMod * 0.5;
                        break;
                    case 3057://Sheen
                        if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready)
                            result += player.BaseAttackDamage;
                        break;
                    case 3144:
                        if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready)
                            result += 100d;
                        break;
                    case 3146:
                        if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready)
                            result += 150d + player.FlatMagicDamageMod * 0.4;
                        break;
                    case 3128:
                        if (player.Spellbook.CanUseSpell((SpellSlot)item.Slot) == SpellState.Ready)
                            result += victim.MaxHealth * 0.15;
                        break;
                }

            return result;
        }

        private static void Escape()
        {
            Vector3 cursorPos = Game.CursorPos;
            Vector2 pos = V2E(player.Position, cursorPos, R.Range);
            Vector2 pass = V2E(player.Position, cursorPos, 120);
            if (Misc["Rcounter"].Cast<Slider>().CurrentValue > ultiCount()) return;
            if (!IsWall(pos) && IsPassWall(player.Position, pos.To3D()) && cursorPos.CountEnemyMinionsInRange(300) <1)
                if (W.IsReady()) W.Cast(cursorPos);
            castREscape(cursorPos, true);
        }

        private static void castREscape(Vector3 position, bool mouseJump = false)
        {
            Obj_AI_Base target = player;
            foreach (Obj_AI_Base minion in ObjectManager.Get<Obj_AI_Base>())
                if (minion.IsValidTarget(R.Range, true) && player.Distance(position, true) > minion.Distance(position, true) && minion.Distance(position, true) < target.Distance(position, true))
                    if (mouseJump)
                    {
                        if (minion.Distance(position) < 200)
                            target = minion;
                    }
                    else
                    {
                        Console.WriteLine("Distance T-M: " + minion.Distance(position) + "  Distance T-P: " + player.Distance(position));
                        Console.WriteLine("Minion - X:" + minion.Position.X + "Y: " + minion.Position.Y + ",  player - X:" + player.Position.X + "Y: " + player.Position.Y + ",  position - X:" + position.X + "Y: " + position.Y);
                        target = minion;
                    }
            if (R.IsReady() && R.IsInRange(target.Position))
                if (mouseJump)
                {
                    if (target.Distance(position) < 200)
                        R.Cast(target);
                }
                else if (player.Distance(position, true) > target.Distance(position, true) && ((int)(player.Distance(position) / R.Range)) < ultiCount())
                    R.Cast(target);

        }

        private static bool IsPassWall(Vector3 start, Vector3 end)
        {
            double count = Vector3.Distance(start, end);
            for (uint i = 0; i <= count; i += 10)
            {
                Vector2 pos = V2E(start, end, i);
                if (IsWall(pos)) return true;
            }
            return false;
        }

        private static int ultiCount()
        {
            foreach (BuffInstance buff in player.Buffs)
                if (buff.Name == "AkaliShadowDance")
                    return buff.Count;
            return 0;
        }

        private static bool IsWall(Vector2 pos)
        {
            return (NavMesh.GetCollisionFlags(pos.X, pos.Y) == CollisionFlags.Wall ||
                    NavMesh.GetCollisionFlags(pos.X, pos.Y) == CollisionFlags.Building);
        }

        private static Vector2 V2E(Vector3 from, Vector3 direction, float distance)
        {
            return from.To2D() + distance * Vector3.Normalize(direction - from).To2D();
        }

        private static bool hasBuff(Obj_AI_Base target, string buffName)
        {
            foreach (BuffInstance buff in target.Buffs)
                if (buff.Name == buffName) return true;
            return false;
        }

        private static bool gapclosePath(Obj_AI_Base target)
        {
            first_unit = new JumpUnit(player.Position, player);
            second_unit = new JumpUnit(target.Position, target);
            if (player.Distance(target) < 800) return true;
            Chat.Print("First check done. Proceed to find units.");
            foreach (Obj_AI_Base candidate in ObjectManager.Get<Obj_AI_Base>())
                if (candidate.IsValidTarget(R.Range, true) &&
                    first_unit.unit.Distance(target, true) > candidate.Distance(target, true)) first_unit = new JumpUnit(candidate.Position, candidate);
            Chat.Print("First unit found. Checks to return.");
            if (first_unit.unit.Distance(target) < 800) return true;
            Chat.Print("First unit found. Proceed to find another.");
            foreach (Obj_AI_Base candidate in ObjectManager.Get<Obj_AI_Base>())
                if (candidate.IsValidTarget(R.Range, true) &&
                    second_unit.unit.Distance(player, true) > candidate.Distance(target, true)) second_unit = new JumpUnit(candidate.Position, candidate);
            Chat.Print("Second unit found. Proceed to checks.");
            if (first_unit.unit.Distance(second_unit.Position) > 800)
                return false;
            if (first_unit.unit.Distance(target) < second_unit.unit.Distance(target))
            {
                first_unit = second_unit;
                second_unit = new JumpUnit(target.Position, target);
            }
            Chat.Print("Checks done. Returning.");
            return true;
        }
    }

    class JumpUnit
    {
        public Vector3 Position;
        public Obj_AI_Base unit;
        public JumpUnit(Vector3 pos, Obj_AI_Base u)
        {
            Position = pos;
            unit = u;
        }
    }
}
