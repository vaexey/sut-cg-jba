using Godot;
using System;

public partial class HikingInstinctCC : CrowdControlEffect
{
    public HikingInstinctCC()
    {
        Time = 10;
    }

    public override void Start(Entity effected)
    {
        effected.PassiveAttributes.SpeedModifierMultiplicative += 1;
    }

    public override void End(Entity effected)
    {
        effected.PassiveAttributes.SpeedModifierMultiplicative -= 1;
    }
}
