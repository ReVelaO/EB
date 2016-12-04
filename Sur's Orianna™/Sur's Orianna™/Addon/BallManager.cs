namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using SharpDX;
    using System;
    internal class BallManager
    {
        public static Vector3 Ball;
        public static void Load()
        {
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }
        internal class WBall
        {
            public static int CountEnemyMinionsNear => Ball != null ? Ball.CountEnemyMinionsInRangeWithPrediction(250, 250) : 0;
            public static int CountEnemyHeroesNear => Ball != null ? Ball.CountEnemyHeroesInRangeWithPrediction(250, 250) : 0;
            public static bool IsInBall(Obj_AI_Base e) => e.IsInRange(Ball, 250);
        }
        internal class RBall
        {
            public static int CountEnemyMinionsNear => Ball != null ? Ball.CountEnemyMinionsInRangeWithPrediction(410, 750) : 0;
            public static int CountEnemyHeroesNear => Ball != null ? Ball.CountEnemyHeroesInRangeWithPrediction(410, 750) : 0;
            public const int CastDelay = 750;
            public static bool IsInBall(Obj_AI_Base e) => e.IsInRange(Ball, 410);
        }
        static void Game_OnUpdate(EventArgs args)
        {
            if (MenuManager.mdrawings["ball"].Cast<CheckBox>().CurrentValue)
            {
                if (Player.HasBuff("orianaghostself"))
                {
                    Ball = Player.Instance.Position;
                }
            }
        }
        static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.Slot == SpellSlot.Q)
                {
                    Ball = args.End;
                }
            }
        }
    }
}
