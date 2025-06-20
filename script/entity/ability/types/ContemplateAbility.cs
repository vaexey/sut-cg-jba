using Godot;
using System;

public partial class ContemplateAbility : Ability
{
    // Franzl stops to contemplate, slowing himself and gaining flat value of 50% inspiration at a cost of 90% of remaining stamina.

    public override double GetUseCostTotal(Entity ent)
    {
        return ent.Stamina.Value * 0.9;
    }

    public override void Cast(IEntityContainer owner)
    {
        var ent = owner.Entity;

        ent.Inspiration.Value += ent.Inspiration.Max * 0.5;
    }
}