using Godot;
using System;
using System.Linq;

public partial class MussDasSeinCC : CrowdControlEffect
{
    public override void Effect(Entity effected, double delta)
    {
        var effects = effected.CC.GetEffects().Where(e => e != this);

        if (effects.Count() > 0)
        {
            foreach (var cc in effects)
            {
                cc.End(effected);
                cc.QueueFree();
            }

            var ability = (MussDasSeinAbility)effected.Abilities.All
                .Where(a => a.GetType().IsAssignableTo(
                    typeof(MussDasSeinAbility)))
                .First();

            ability.OnReflectedSignal();
            QueueFree();
        }
    }

    public override void End(Entity effected)
    {
        var ability = (MussDasSeinAbility)effected.Abilities.All
            .Where(a => a.GetType().IsAssignableTo(
                typeof(MussDasSeinAbility)))
            .First();

        ability.OnFailedSignal();

        var slow = CrowdControlLibrary.WaterInPants.Make();

        effected.CC.AddEffect(slow, 1);
    }
}
