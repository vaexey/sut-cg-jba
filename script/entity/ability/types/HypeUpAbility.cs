using Godot;
using System;

public partial class HypeUpAbility : Ability
{
    // Franzl hypes himself up, regaining 50% of missing stamina, at a cost of 50% remaining inspiration.

    public override double GetUseCostTotal(Entity ent)
    {
        return ent.Inspiration.Value * 0.5;
    }

    public override void Cast(IEntityContainer owner)
    {
        var ent = owner.Entity;

        ent.Stamina.Value += (ent.Stamina.Max - ent.Stamina.Value) * 0.5;
    }
}
