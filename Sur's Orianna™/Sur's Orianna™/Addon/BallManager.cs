namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using SharpDX;
    using System;
    using System.Linq;

    public static class BallManager
    {
        //mah stuff ok, :?

        public static Vector3 Ball;
        public static bool IsInFloor;

        private static AIHeroClient Orianna => Player.Instance;

        public static bool HasBall(this Obj_AI_Base obj) => obj.HasBuff("orianaghostself");
        public static bool NotMeBall(this Obj_AI_Base obj) => obj.HasBuff("OrianaGhost");

        public static void Load()
        {
            Game.OnUpdate += Game_OnUpdate;
            GameObject.OnCreate += OnCreate;
        }

        public static class WBall
        {
            public static int CountEnemyMinionsNear => Ball != null ? Ball.CountEnemyMinionsInRangeWithPrediction(250, 250) : 0;
            public static int CountEnemyHeroesNear => Ball != null ? Ball.CountEnemyHeroesInRangeWithPrediction(250, 250) : 0;
            public const int CastDelay = 250;
            public const int Width = 250;

            public static bool IsInBall(Obj_AI_Base e) => e.IsInRange(Ball, Width);
        }

        public static class RBall
        {
            public static int CountEnemyMinionsNear => Ball != null ? Ball.CountEnemyMinionsInRangeWithPrediction(410, 750) : 0;
            public static int CountEnemyHeroesNear => Ball != null ? Ball.CountEnemyHeroesInRangeWithPrediction(410, 750) : 0;
            public const int CastDelay = 750;
            public const int Width = 410;

            public static bool IsInBall(Obj_AI_Base e) => e.IsInRange(Ball, Width);
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Player.Instance.HasBall())
            {
                IsInFloor = false;
                Ball = Orianna.Position;
                SpellManager.Q.RangeCheckSource = Orianna.Position;
            }

            var ally = EntityManager.Heroes
                .Allies
                    .FirstOrDefault(x => x.NotMeBall());

            if (ally != null)
            {
                IsInFloor = false;
                Ball = ally.Position;
                SpellManager.Q.RangeCheckSource = ally.Position;
            }
        }

        private static void OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name.ToLower() == "orianna_base_q_yomu_ring_green.troy")
            {
                IsInFloor = true;
                Ball = sender.Position;
                SpellManager.Q.RangeCheckSource = sender.Position;
            }
        }
    }
}