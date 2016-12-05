using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Spells;
using Timer = System.Timers.Timer;

namespace Galio
{
    public static class DamageHandler
    {
        private static Dictionary<MissileClient, Geometry.Polygon> Missiles =
            new Dictionary<MissileClient, Geometry.Polygon>();

        private static List<Geometry.Polygon> Joined = new List<Geometry.Polygon>();

        private static Timer timer = new Timer(500);

        public static void DamageHandlerLoad()
        {
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;

            timer.Elapsed += delegate
            {
                UpdatePolygons();
            };
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;
            if (missile == null) return;

            var missileInfo =
                SpellDatabase.GetSpellInfoList(missile.SpellCaster).FirstOrDefault(s => s.RealSlot == missile.Slot);
            if (missileInfo == null) return;

            switch (missileInfo.Type)
            {
                case SpellType.Self:
                    break;
                case SpellType.Circle:
                    var polCircle = new Geometry.Polygon.Circle(missile.Position, missileInfo.Radius);
                    Missiles.Add(missile, polCircle);
                    break;
                case SpellType.Line:
                    var polLine = new Geometry.Polygon.Rectangle(missile.StartPosition,
                        missile.StartPosition.Extend(missile.EndPosition, missileInfo.Range).To3D(), 5);
                    Missiles.Add(missile, polLine);
                    break;
                case SpellType.Cone:
                    var polCone = new Geometry.Polygon.Sector(missile.StartPosition, missile.EndPosition, missileInfo.Radius, missileInfo.Range, 80);
                    Missiles.Add(missile, polCone);
                    break;
                case SpellType.Ring:
                    break;
                case SpellType.Arc:
                    break;
                case SpellType.MissileLine:
                    var polMissileLine = new Geometry.Polygon.Rectangle(missile.StartPosition,
                        missile.StartPosition.Extend(missile.EndPosition, missileInfo.Range).To3D(), 5);
                    Missiles.Add(missile, polMissileLine);
                    break;
                case SpellType.MissileAoe:
                    var polMissileAoe = new Geometry.Polygon.Rectangle(missile.StartPosition,
                        missile.StartPosition.Extend(missile.EndPosition, missileInfo.Range).To3D(), 5);
                    Missiles.Add(missile, polMissileAoe);
                    break;
            }

            var polygons = new List<Geometry.Polygon>();
            polygons.AddRange(Missiles.Values);

            Joined = polygons.JoinPolygons();
        }

        private static void UpdatePolygons()
        {
            foreach (var missile in Missiles)
            {
                var missileInfo =
                    SpellDatabase.GetSpellInfoList(missile.Key.SpellCaster)
                        .FirstOrDefault(s => s.RealSlot == missile.Key.Slot);
                if (missileInfo == null) return;

                switch (missileInfo.Type)
                {
                    case SpellType.Self:
                        break;
                    case SpellType.Circle:
                        var polCircle = new Geometry.Polygon.Circle(missile.Key.Position, missileInfo.Radius);
                        Missiles[missile.Key] = polCircle;
                        break;
                    case SpellType.Line:
                        var polLine = new Geometry.Polygon.Rectangle(missile.Key.StartPosition,
                            missile.Key.StartPosition.Extend(missile.Key.EndPosition, missileInfo.Range).To3D(), 5);
                        Missiles[missile.Key] = polLine;
                        break;
                    case SpellType.Cone:
                        var polCone = new Geometry.Polygon.Sector(missile.Key.StartPosition, missile.Key.EndPosition, missileInfo.Radius, missileInfo.Range, 80);
                        Missiles[missile.Key] = polCone;
                        break;
                    case SpellType.Ring:
                        break;
                    case SpellType.Arc:
                        break;
                    case SpellType.MissileLine:
                        var polMissileLine = new Geometry.Polygon.Rectangle(missile.Key.StartPosition,
                            missile.Key.StartPosition.Extend(missile.Key.EndPosition, missileInfo.Range).To3D(), 5);
                        Missiles[missile.Key] = polMissileLine;
                        break;
                    case SpellType.MissileAoe:
                        var polMissileAoe = new Geometry.Polygon.Rectangle(missile.Key.StartPosition,
                            missile.Key.StartPosition.Extend(missile.Key.EndPosition, missileInfo.Range).To3D(), 5);
                        Missiles[missile.Key] = polMissileAoe;
                        break;
                }
            }

            var polygons = new List<Geometry.Polygon>();
            polygons.AddRange(Missiles.Values);

            Joined = polygons.JoinPolygons();
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;
            if (missile == null) return;

            var sameMissile = Missiles.FirstOrDefault(m => m.Key == missile);

            if (sameMissile.Key == null) return;

            Missiles.Remove(sameMissile.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="healthPercent"></param>
        /// <param name="range">The less, the more it will wait the missile get closer</param>
        /// <returns></returns>


        public static bool IsInDanger(this AIHeroClient hero, int healthPercent = 30, int range = 300)
        {
            return hero.HealthPercent < healthPercent && Missiles.Values.Any(p => p.IsInside(hero)) &&
                   Missiles.Keys.Any(m => m.IsInRange(hero, 300));
        }

        public static AIHeroClient GetAllyInDanger(uint spellRange, int healthPecent = 30, int range = 300)
        {
            return EntityManager.Heroes.Allies.FirstOrDefault(a => a.IsInDanger(healthPecent, range) && a.IsValidTarget(spellRange));
        }
    }
}