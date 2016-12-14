using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace Twitchsharp.Addon
{
    public static class DamageIndicator
    {
        //Offsets Properties
        private const float YOff = 9.8f;

        private const float XOff = 0;

        //Line Properties
        private const float Width = 104;

        private const float thick = 9.82f;

        //Initialize Damage Indicator
        public static void Load()
        {
            Chat.Print("<font color='#FF4444'>[Twitch#:</font> <font color='#A66EFF'>Damage Indicator</font><font color='#FF4444'>] => Successfully loaded</font>");
            Drawing.OnEndScene += OnEndScene;
        }

        private static void OnEndScene(EventArgs args)
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.VisibleOnScreen && x.IsVenom()))
            {
                //Our Source Damage
                var damage = DamageHandler.SD(enemy);

                if (damage < 1) return;

                //Our Menu Value for enable Damage Indicator
                if (MenuHandler.drawings["di"].Cast<CheckBox>().CurrentValue)
                {
                    //Health Calculations
                    var dmgPer = (enemy.TotalShieldHealth() - damage > 0 ? enemy.TotalShieldHealth() - damage : 0) / enemy.TotalShieldMaxHealth();
                    var currentHPperc = enemy.TotalShieldHealth() / enemy.TotalShieldMaxHealth();

                    //Points
                    var start = new Vector2((int)(enemy.HPBarPosition.X + XOff + dmgPer * Width),
                        (int)enemy.HPBarPosition.Y + YOff);
                    var end = new Vector2((int)(enemy.HPBarPosition.X + XOff + currentHPperc * Width) + 1,
                        (int)enemy.HPBarPosition.Y + YOff);

                    //Start Drawing Line
                    Drawing.DrawLine(start, end, thick, Color.FromArgb(170, Color.OrangeRed));

                    //Offsets
                    var posXPer = (int)enemy.HPBarPosition[0] - 28;
                    var posYPer = (int)enemy.HPBarPosition[1];

                    //Health Calculations
                    var damagepercent = (damage / enemy.TotalShieldHealth()) * 100;
                    var percent = damagepercent > 100 ? 100 : damagepercent;

                    //Start Drawing Text
                    Drawing.DrawText(posXPer, posYPer, Color.WhiteSmoke, string.Concat(Math.Ceiling(percent), "%"));
                }
            }
        }
    }
}