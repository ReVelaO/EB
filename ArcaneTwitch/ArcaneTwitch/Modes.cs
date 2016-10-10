using EloBuddy;
using EloBuddy.SDK;

namespace ArcaneTwitch
{
    public static class Modes
    {
        public static void Combo()
        {
            var Target = TargetSelector.GetTarget(Program._Elixir.Range, DamageType.Physical);
            if (Target == null)
            {
                return;
            }
            Functions.Elixir(Target);
            Functions.Frustation(Target);
        }
    }
}
