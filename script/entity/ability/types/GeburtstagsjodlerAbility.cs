using Godot;
using System;

public partial class GeburtstagsjodlerAbility : Ability
{
    [Export] public double HealPerSecondFlat { get; set; } = 10;

    public override void ProcessOwner(Entity owner, double delta)
    {
        base.ProcessOwner(owner, delta);

        if (IsCasting)
        {
            owner.Beverage.Value = Mathf.MoveToward(
                owner.Beverage.Value,
                owner.Beverage.Max,
                delta * HealPerSecondFlat
            );
        }
    }

    public override void ConsumeCost(Entity ent)
    {
        // Don't
    }

    public override void StopCast(Entity owner)
    {
        CooldownLeft = Cooldown;
        base.ConsumeCost(owner);
    }

}
