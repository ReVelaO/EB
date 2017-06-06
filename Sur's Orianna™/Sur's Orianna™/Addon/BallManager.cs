namespace Orianna.Addon
{
    using EloBuddy;
    using EloBuddy.SDK;
    using SharpDX;
    using System;
    using System.Linq;

    public static class BallManager
    {
        public static Vector3 Ball;
        public static bool IsInFloor;

        private static AIHeroClient Orianna => Player.Instance;

        public static bool HasBall(this Obj_AI_Base obj) => obj.HasBuff("orianaghostself");

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
                Ball = Orianna.Position;
                IsInFloor = false;
            }

            var ally = EntityManager.Heroes
                .Allies
                    .FirstOrDefault(x => x.HasBuff("OrianaGhost"));

            if (ally != null)
            {
                Ball = ally.Position;
                IsInFloor = false;
            }
        }

        private static void OnCreate(GameObject obj, EventArgs args)
        {
            var particle = obj as Obj_GeneralParticleEmitter;
            if (particle != null && 
                particle.Name == "Orianna_Base_Q_yomu_ring_green.troy")
            {
                Ball = particle.Position;
                IsInFloor = true;
            }
        }
    }
}