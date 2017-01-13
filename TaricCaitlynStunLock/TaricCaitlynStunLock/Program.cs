using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.Sandbox;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace TaricCaitlynStunLock
{
    class Program
    {
        public static Spell.Skillshot W;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoading;
        }

        public static void OnLoading(EventArgs args)
        {
            var username = SandboxConfig.Username;
            if (Player.Instance.Hero != Champion.Caitlyn) return;
                Chat.Print("Hello" + username);
            Game.OnTick += OnTick;
            InitializeSpells();
        }

        public static void InitializeSpells()
        {
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 1600)
            {
                Width = 20
            };
        }
        public static void OnTick(EventArgs args)
        {
            var target = TargetSelector.GetTarget(W.Range, DamageType.Physical, Player.Instance.Position);
            if (target.HasBuff("dazzle") && W.IsReady())
                W.Cast(target);
        }
    }
}
