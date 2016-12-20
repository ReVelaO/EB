using EloBuddy;

namespace Quinnsharp.Addon.Orb
{
    public static class Flee
    {
        private static AIHeroClient Quinn => Player.Instance;

        public static void Get()
        {
            if (SpellHandler.R.IsReady())
            {
                if (SpellHandler.R.Name.ToLower() == "quinnrfinale"
                    || SpellHandler.R.Name.ToLower() == "quinnrreturntoquinn") return;

                SpellHandler.R.Cast();
            }
        }
    }
}