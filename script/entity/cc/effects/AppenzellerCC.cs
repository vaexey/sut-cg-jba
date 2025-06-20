using Godot;
using System;

public partial class AppenzellerCC : CrowdControlEffect
{
    public AppenzellerCC()
    {
        Time = 10;
    }

    public override void Start(Entity effected)
    {
        effected.PassiveAttributes.JumpModifierMultiplicative += 0.5f;
    }

    public override void End(Entity effected)
    {
        effected.PassiveAttributes.JumpModifierMultiplicative -= 0.5f;
    }
}