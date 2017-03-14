using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Color = System.Drawing;

namespace OuijaTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }
        //Define Spell
        public static Spell.Skillshot Q;
        public static Spell.Active E, W;
        public static Spell.Targeted R;
        public static Spell.Targeted Ignite;
        //Define Menu
        public static Menu FirstMenu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu LaneMenu;
        public static Menu LastHitMenu;
        public static Menu MiscMenu;
        public static Menu Drawings;

        private static void OnLoad(EventArgs args)
        {
            if (Player.Instance.ChampionName != "InsertChampNameHere") return;

            MenuLoad();
            SkillsLoad();

            Drawing.OnDraw += OnDraw;
            Game.OnUpdate += OnUpdate;

            if (Player.Instance.GetSpellSlotFromName("SummonerDot") == SpellSlot.Summoner1) { Ignite = new Spell.Targeted(SpellSlot.Summoner1, 600, DamageType.True ); }
            if (Player.Instance.GetSpellSlotFromName("SummonerDot") == SpellSlot.Summoner2) { Ignite = new Spell.Targeted(SpellSlot.Summoner2, 600, DamageType.True); }
        }

        private static void MenuLoad()
        {
            FirstMenu = MainMenu.AddMenu("Ouija's " + Player.Instance.ChampionName, Player.Instance.ChampionName.ToLower());
            ComboMenu = FirstMenu.AddSubMenu("Combo Settings");
            HarassMenu = FirstMenu.AddSubMenu("Harass Settings");
            LaneMenu = FirstMenu.AddSubMenu("Lane Settings");
            LastHitMenu = FirstMenu.AddSubMenu("LastHit Settings");
            MiscMenu = FirstMenu.AddSubMenu("Misc Settings");
            Drawings = FirstMenu.AddSubMenu("Drawings Settings");

            ComboMenu.AddGroupLabel("-> Combo");
            ComboMenu.Add("useQ", new CheckBox("Use Q (Combo)"));

            HarassMenu.AddGroupLabel("-> Harass");
            HarassMenu.Add("useQ", new CheckBox("Use Q (Harass)"));

            LaneMenu.AddGroupLabel("-> LaneClear");
            LaneMenu.Add("sliderSpell", new Slider("Use Spells if will hit {0} Minions", 2, 1, 10));
            LaneMenu.Add("useQ", new CheckBox("Use Q (LaneClear)"));

            LastHitMenu.AddGroupLabel("-> LastHit");
            LastHitMenu.AddLabel("This will lasthit the Minion if his HP is under your X Spell");
            LastHitMenu.Add("useQ", new CheckBox("Use Q (LastHit)"));

            MiscMenu.AddGroupLabel("-> Misc");
            ComboMenu.Add("gapcloser", new CheckBox("Gapcloser (Misc)"));
            ComboMenu.Add("igniteusage", new CheckBox("Use Ignite if damage from Ignite is higher than enemy HP"));

            Drawings.AddGroupLabel("-> Drawings");
            Drawings.Add("drawQ", new CheckBox("Draw Q (Draw)"));
            Drawings.Add("drawW", new CheckBox("Draw W (Draw)"));
            Drawings.Add("drawE", new CheckBox("Draw E (Draw)"));
            Drawings.Add("drawR", new CheckBox("Draw R (Draw)"));
        }

        private static void SkillsLoad()
        {//examples
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1000, 40);
            W = new Spell.Active(SpellSlot.W, 1000, DamageType.Mixed);
            E = new Spell.Active(SpellSlot.E, 1000, DamageType.Mixed);
            R = new Spell.Targeted(SpellSlot.R, 100, DamageType.Mixed);
        }

        private static void OnDraw(EventArgs args)
        {
            bool QDrawCheck = Drawings["drawQ"].Cast<CheckBox>().CurrentValue;
            bool WDrawCheck = Drawings["drawW"].Cast<CheckBox>().CurrentValue;
            bool EDrawCheck = Drawings["drawE"].Cast<CheckBox>().CurrentValue;
            bool RDrawCheck = Drawings["drawR"].Cast<CheckBox>().CurrentValue;

            if (QDrawCheck && Q.IsReady()) { Drawing.DrawCircle(Player.Instance.Position, Q.Range, Color.Color.Coral); }
            if (WDrawCheck && W.IsReady()) { Drawing.DrawCircle(Player.Instance.Position, W.Range, Color.Color.Coral); }
            if (EDrawCheck && E.IsReady()) { Drawing.DrawCircle(Player.Instance.Position, E.Range, Color.Color.Coral); }
            if (RDrawCheck && R.IsReady()) { Drawing.DrawCircle(Player.Instance.Position, R.Range, Color.Color.Coral); }
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ComboExec();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneExec();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) HarassExec();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHitExec();

            var target = TargetSelector.GetTarget(Q.Range, DamageType.True, Player.Instance.Position);
            var ignitedamage = Ignite.GetSpellDamage(target);
            if (MiscMenu["igniteusage"].Cast<CheckBox>().CurrentValue && Ignite.IsReady() && target.Health + 50 < ignitedamage) { Ignite.Cast(target); }
        }

        private static void ComboExec()
        {//without prediction!

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Mixed);

            if (!target.IsValid || target.IsDead)
            {
                if (ComboMenu["useQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {Q.Cast(target);}

                if (ComboMenu["useW"].Cast<CheckBox>().CurrentValue && W.IsReady())
                {W.Cast(target);}
            }
        }

        private static void HarassExec()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Mixed);

            if (!target.IsValid || target.IsDead)
            {
                if (HarassMenu["useQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
                {Q.Cast(target);}

                if (HarassMenu["useW"].Cast<CheckBox>().CurrentValue && W.IsReady())
                {W.Cast(target);}
            }
        }

        private static void LaneExec()
        {
            //only if spell is area based

            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range);

            foreach (var laneenemies in lanemonster)
            {                                             //only if Q is skillshot (linear)
                if (laneenemies.IsValid && Q.IsReady() && Q.CastIfItWillHit(LaneMenu["sliderSpell"].Cast<Slider>().CurrentValue))
                {
                    Q.Cast(laneenemies);
                }
            }
        }
        //!experimental!
        private static void LastHitExec()
        {//straight from Marksman AIO
            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range);
            var lasthitminion = lanemonster.Where(x => x.IsValidTarget(Q.Range) 
                && (lanemonster.Count(k => k.Distance(x) <= 150 
                && (Prediction.Health.GetPrediction(k, 350) < Q.GetSpellDamage(k))) > 2));

            if (LastHitMenu["useQ"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                foreach (var laster in lasthitminion.OrderBy(x => x.Health))
                {
                    Q.Cast(laster);
                }
            }
        }
    }
}
