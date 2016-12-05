namespace Galio
{
    using System;
    using System.Linq;
    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Spells;
    internal class Smite
    {
        static Spell.Targeted Castigo = new Spell.Targeted(SummonerSpells.Smite.Slot, 570);
        static Menu menu;
        static bool HasSmite => SummonerSpells.PlayerHas(SummonerSpellsEnum.Smite);
        public static void LoadSmite()
        {
            if (HasSmite)
            {
                menu = MainMenu.AddMenu("Smite", "index0");
                menu.Add("smite", new CheckBox("Use Smite"));
                menu.AddSeparator(14);
                menu.AddLabel("¿Which monster to smite?");
                menu.AddSeparator(14);
                menu.Add("sblue", new CheckBox("Smite Blue"));
                menu.Add("sred", new CheckBox("Smite Red"));
                menu.Add("sdragons", new CheckBox("Smite Dragons"));
                menu.Add("shb", new CheckBox("Smite Herald/Baron"));

                Game.OnTick += Game_OnTick;
            }
            else
            {
                Chat.Print("Smite was not detected, unloading.");
            }
        }
        static void Game_OnTick(EventArgs args)
        {
            if (menu["smite"].Cast<CheckBox>().CurrentValue)
            {
                var m = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(x => GetMonsterNames().Contains(x.BaseSkinName) && x.IsValidTarget(Castigo.Range)).OrderBy(o=>o.Distance(Player.Instance)).FirstOrDefault();
                if (m != null)
                {
                    if (Castigo.IsReady())
                    {
                        if (HPrediction(m, Castigo.CastDelay) < SmiteDMG(m))
                        {
                            Castigo.Cast(m);
                        }
                    }
                }
            }
        }
        static float SmiteDMG(Obj_AI_Base e) => DamageLibrary.GetSummonerSpellDamage(Player.Instance, e, DamageLibrary.SummonerSpells.Smite);
        static float HPrediction(Obj_AI_Base e, int d) => Prediction.Health.GetPrediction(e, d);
        static string GetMonsterNames()
        {
            string x = null;
            if (menu["sblue"].Cast<CheckBox>().CurrentValue)
            {
                x += "SRU_Blue";
            }
            if (menu["sred"].Cast<CheckBox>().CurrentValue)
            {
                x += "SRU_Red";
            }
            if (menu["sdragons"].Cast<CheckBox>().CurrentValue)
            {
                x += "SRU_Dragon_Water" + "SRU_Dragon_Fire" + "SRU_Dragon_Earth" + "SRU_Dragon_Air" + "SRU_Dragon_Elder";
            }
            if (menu["shb"].Cast<CheckBox>().CurrentValue)
            {
                x += "SRU_RiftHerald" + "SRU_Baron";
            }
            return x;
        }
    }
}
