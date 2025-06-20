using Godot;
using System;

public partial class WaterInPantsCC : CrowdControlEffect
{
    public WaterInPantsCC()
    {
        Time = 10;
    }

    public override void Start(Entity effected)
    {
        GD.Print(effected);
        effected.PassiveAttributes.SpeedModifierMultiplicative -= 0.5f;
    }

    public override void End(Entity effected)
    {
        effected.PassiveAttributes.SpeedModifierMultiplicative += 0.5f;
    }
}
